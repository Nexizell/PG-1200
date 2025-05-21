using System;
using System.Globalization;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class AndroidCloudSlotSynchronizer : CloudSlotSynchronizer
	{
		private string _accumulatedMerge;

		private Task<string> _pullFuture;

		private Task<ISavedGameMetadata> _pushFuture;

		private string _lastSuccessfulResult;

		private string _encryptedPlayerPrefsKey;

		private readonly string _filename;

		private readonly Func<string, string, string> _merger;

		private readonly Lazy<EncryptedPlayerPrefs> _encryptedPlayerPrefs;

		public override string CurrentResult
		{
			get
			{
				Task<string> pullFuture = _pullFuture;
				if (pullFuture == null)
				{
					return _lastSuccessfulResult;
				}
				if (((Task)pullFuture).IsFaulted || ((Task)pullFuture).IsCanceled)
				{
					return _lastSuccessfulResult;
				}
				if (!((Task)pullFuture).IsCompleted)
				{
					return _lastSuccessfulResult;
				}
				string result = pullFuture.Result;
				if (result != null)
				{
					_lastSuccessfulResult = result;
					_pullFuture = null;
				}
				return _lastSuccessfulResult;
			}
		}

		public Exception LastPullException
		{
			get
			{
				Task<string> pullFuture = _pullFuture;
				if (pullFuture == null)
				{
					return null;
				}
				if (!((Task)pullFuture).IsFaulted)
				{
					return null;
				}
				return (Exception)(object)((Task)pullFuture).Exception;
			}
		}

		public override bool Pulled
		{
			get
			{
				return CurrentResult != null;
			}
		}

		private EncryptedPlayerPrefs EncryptedPlayerPrefs
		{
			get
			{
				return _encryptedPlayerPrefs.Value;
			}
		}

		private string PendingMergeKey
		{
			get
			{
				if (string.IsNullOrEmpty(_encryptedPlayerPrefsKey))
				{
					_encryptedPlayerPrefsKey = "PendingMerge/" + _filename;
				}
				return _encryptedPlayerPrefsKey;
			}
		}

		public AndroidCloudSlotSynchronizer(string filename, Func<string, string, string> merger)
		{
			_filename = filename ?? string.Empty;
			_merger = merger ?? new Func<string, string, string>(TrivialMerge);
			_encryptedPlayerPrefs = new Lazy<EncryptedPlayerPrefs>(CreateEncryptedPlayerPrefs);
			try
			{
				string @string = EncryptedPlayerPrefs.GetString(PendingMergeKey);
				if (!string.IsNullOrEmpty(@string))
				{
					_accumulatedMerge = @string;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		public override Task<CloudPullResult> Pull(bool silent = true)
		{
			Task<CloudPullResult> result = Task.FromResult<CloudPullResult>(CloudPullResult.Failed);
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				Debug.LogWarningFormat("Skipping {0}.Pull({1}) on {2}", GetType(), silent, BuildSettings.BuildTargetPlatform);
				return result;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
			{
				Debug.LogWarningFormat("Skipping {0}.Pull({1}) on {2}", GetType(), silent, Defs.AndroidEdition);
				return result;
			}
			if (Application.isEditor)
			{
				Debug.LogFormat("{0}.Pull({1})", GetType(), silent);
				return result;
			}
			if (_pullFuture != null && !((Task)_pullFuture).IsCompleted)
			{
				Debug.LogFormat("Skipping {0}.Pull({1}) because previous request not completed", GetType(), silent);
				return result;
			}
			if (_pullFuture != null && !((Task)_pullFuture).IsFaulted && !((Task)_pullFuture).IsCanceled)
			{
				string result2 = _pullFuture.Result;
				if (result2 != null)
				{
					_lastSuccessfulResult = result2;
				}
			}
			TaskCompletionSource<string> val = new TaskCompletionSource<string>();
			GooglePullHelper googlePullHelper = new GooglePullHelper(_filename, silent, _merger, val);
			googlePullHelper.Conflict += HandleMerge;
			googlePullHelper.Run();
			_pullFuture = val.Task;
			return _pullFuture.ContinueWith<CloudPullResult>((Func<Task<string>, CloudPullResult>)delegate(Task<string> t)
			{
				if (((Task)t).IsCanceled)
				{
					return CloudPullResult.Failed;
				}
				if (!((Task)t).IsFaulted)
				{
					return CloudPullResult.Successful;
				}
				if (((Exception)(object)((Task)t).Exception).InnerException is NotAuthenticatedException)
				{
					return CloudPullResult.LoginFailed;
				}
				return (!GpgFacade.Instance.IsAuthenticated()) ? CloudPullResult.LoginFailed : CloudPullResult.Failed;
			});
		}

		public override void Push(string data)
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				Debug.LogWarningFormat("Skipping {0}.Push() on {1}", GetType(), BuildSettings.BuildTargetPlatform);
				return;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
			{
				Debug.LogWarningFormat("Skipping {0}.Push() on {1}", GetType(), Defs.AndroidEdition);
				return;
			}
			if (Application.isEditor)
			{
				Debug.LogFormat("{0}.Push()", GetType());
				return;
			}
			if (_pushFuture != null && !((Task)_pushFuture).IsCompleted)
			{
				Debug.LogFormat("Skipping {0}.Push() because previous request not completed", GetType());
				return;
			}
			string data2 = ((_accumulatedMerge == null) ? data : _merger(_accumulatedMerge, data));
			TaskCompletionSource<ISavedGameMetadata> val = new TaskCompletionSource<ISavedGameMetadata>();
			GooglePushHelper googlePushHelper = new GooglePushHelper(_filename, _merger, data2, val);
			googlePushHelper.Conflict += HandleMerge;
			googlePushHelper.Run();
			_pushFuture = val.Task;
			_pushFuture.ContinueWith((Action<Task<ISavedGameMetadata>>)HandlePushCompleted);
		}

		public override string ToString()
		{
			return string.Format("{0}(`{1}`)", new object[2]
			{
				GetType().Name,
				_filename
			});
		}

		private static string TrivialMerge(string left, string right)
		{
			return left;
		}

		private void HandleMerge(object sender, MergeEventArgs e)
		{
			if (e == null || e.Merge == null)
			{
				return;
			}
			using (new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "AndroidCloudSlotSynchronizer(`{0}`).HandleMerge({1} chars)", _filename, e.Merge.Length), Defs.IsDeveloperBuild))
			{
				_accumulatedMerge = ((_accumulatedMerge == null) ? e.Merge : _merger(_accumulatedMerge, e.Merge));
				EncryptedPlayerPrefs.SetString(PendingMergeKey, _accumulatedMerge ?? string.Empty);
			}
		}

		private void HandlePushCompleted(Task<ISavedGameMetadata> future)
		{
			string text = (((Task)future).IsFaulted ? GetExceptionDescription(((Task)future).Exception) : future.Result.GetDescription());
			using (new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "AndroidCloudSlotSynchronizer(`{0}`).HandlePushCompleted(`{1}`)", _filename, text), Defs.IsDeveloperBuild))
			{
				if (!((Task)future).IsFaulted)
				{
					EncryptedPlayerPrefs.SetString(PendingMergeKey, string.Empty);
				}
			}
		}

		private static string GetExceptionDescription(AggregateException aggregateException)
		{
			if (aggregateException == null)
			{
				return "aggregateException == null";
			}
			Exception innerException = ((Exception)(object)aggregateException).InnerException;
			if (innerException == null)
			{
				return "innerException == null";
			}
			if (innerException.Message == null)
			{
				return ((object)innerException).GetType().Name;
			}
			return string.Format("{0}(\"{1}\")", new object[2]
			{
				((object)innerException).GetType(),
				innerException.Message
			});
		}

		private static EncryptedPlayerPrefs CreateEncryptedPlayerPrefs()
		{
			HiddenSettings hiddenSettings = ((MiscAppsMenu.Instance != null) ? MiscAppsMenu.Instance.misc : null);
			byte[] array = null;
			if (hiddenSettings == null)
			{
				array = Convert.FromBase64String("SZ+5SPQlvW7vBura/S2myrrtowD8kkhW16FVk4cHtLhdxwhUh0KZog==");
			}
			else
			{
				try
				{
					array = Convert.FromBase64String(hiddenSettings.CloudSynchronizerKey);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					array = Convert.FromBase64String("SZ+5SPQlvW7vBura/S2myrrtowD8kkhW16FVk4cHtLhdxwhUh0KZog==");
				}
			}
			return new EncryptedPlayerPrefs(array);
		}
	}
}

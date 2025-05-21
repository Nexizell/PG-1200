using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class GooglePushHelper
	{
		private const string SavedGameClientIsNullMessage = "[Rilisoft] SavedGame client is null.";

		private bool _invoked;

		private DataSource _currentDataSource = DataSource.ReadNetworkOnly;

		private string _accumulatedMerge;

		private readonly string _filename;

		private readonly Func<string, string, string> _merger;

		private readonly string _data;

		private readonly TaskCompletionSource<ISavedGameMetadata> _promise;

		private string Filename
		{
			get
			{
				return _filename;
			}
		}

		private Task<ISavedGameMetadata> Future
		{
			get
			{
				return _promise.Task;
			}
		}

		private static ISavedGameClient SavedGame
		{
			get
			{
				return null;
			}
		}

		internal event EventHandler<MergeEventArgs> Conflict;

		internal GooglePushHelper(string filename, Func<string, string, string> merger, string data, TaskCompletionSource<ISavedGameMetadata> promise)
		{
			if (merger == null)
			{
				throw new ArgumentNullException("merger");
			}
			if (promise == null)
			{
				throw new ArgumentNullException("promise");
			}
			_filename = filename ?? string.Empty;
			_merger = merger;
			_data = data;
			_promise = promise;
		}

		internal void Run()
		{
			bool flag = GpgFacade.Instance.IsAuthenticated();
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePushHelper(`{0}`).Run(); isAuthenticated: `{1}`", Filename, flag), Defs.IsDeveloperBuild);
			try
			{
				if (!_invoked)
				{
					_invoked = true;
					if (flag)
					{
						RunAuthenticated();
					}
					else
					{
						_promise.TrySetException((Exception)new NotAuthenticatedException("Not authenticated"));
					}
				}
			}
			finally
			{
				((IDisposable)scopeLogger).Dispose();
			}
		}

		private void RunAuthenticated()
		{
			if (SavedGame == null)
			{
				_promise.TrySetException((Exception)new InvalidOperationException("[Rilisoft] SavedGame client is null."));
			}
			else
			{
				SavedGame.OpenWithManualConflictResolution(Filename, _currentDataSource, true, HandleOpenConflict, HandleOpenCompleted);
			}
		}

		private void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
		{
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePushHelper(`{0}`).HandleOpenConflict(`{1}`, `{2}`)", Filename, GooglePullHelper.GetDescription(original), GooglePullHelper.GetDescription(unmerged)), Defs.IsDeveloperBuild);
			try
			{
				if (originalData == null)
				{
					resolver.ChooseMetadata(unmerged);
					return;
				}
				if (unmergedData == null)
				{
					resolver.ChooseMetadata(original);
					return;
				}
				string text = Encoding.UTF8.GetString(originalData, 0, originalData.Length) ?? string.Empty;
				string text2 = Encoding.UTF8.GetString(unmergedData, 0, unmergedData.Length) ?? string.Empty;
				string text3 = _merger(text, text2);
				if ((object)text3 == text)
				{
					resolver.ChooseMetadata(original);
					return;
				}
				if ((object)text3 == text2)
				{
					resolver.ChooseMetadata(unmerged);
					return;
				}
				if (_accumulatedMerge == null)
				{
					_accumulatedMerge = text3;
				}
				else
				{
					_accumulatedMerge = _merger(_accumulatedMerge, text3);
				}
				EventHandler<MergeEventArgs> conflict = this.Conflict;
				if (conflict != null)
				{
					conflict(this, new MergeEventArgs(_accumulatedMerge));
				}
				ISavedGameMetadata chosenMetadata = ((original.LastModifiedTimestamp >= unmerged.LastModifiedTimestamp) ? original : unmerged);
				resolver.ChooseMetadata(chosenMetadata);
			}
			catch (Exception ex)
			{
				_promise.TrySetException(ex);
			}
			finally
			{
				if (Defs.IsDeveloperBuild && ((Task)Future).IsFaulted)
				{
					Debug.LogFormat("Request failed with `{0}`: {1}", ((object)((Task)Future).Exception).GetType(), ((Exception)(object)((Task)Future).Exception).Message);
				}
				scopeLogger.Dispose();
			}
		}

		private void HandleOpenCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
		{
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePushHelper(`{0}`).HandleOpenCompleted(`{1}`, `{2}`)", Filename, requestStatus, GooglePullHelper.GetDescription(metadata)), Defs.IsDeveloperBuild);
			try
			{
				switch (requestStatus)
				{
				case SavedGameRequestStatus.Success:
				{
					string text = ((_accumulatedMerge != null) ? _merger(_accumulatedMerge, _data) : _data);
					string text2 = ((_accumulatedMerge != null) ? "merged" : "none");
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("conflict: {0}, result: {1}", text2, text ?? string.Empty);
					}
					string description = string.Format(CultureInfo.InvariantCulture, "device:'{0}', conflict:'{1}'", SystemInfo.deviceModel, text2);
					SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
					byte[] bytes = Encoding.UTF8.GetBytes(text ?? string.Empty);
					SavedGame.CommitUpdate(metadata, updateForMetadata, bytes, HandleCommitCompleted);
					break;
				}
				case SavedGameRequestStatus.TimeoutError:
					if (_currentDataSource == DataSource.ReadCacheOrNetwork)
					{
						_promise.TrySetException((Exception)new SavedGameRequestStatusException(requestStatus));
						break;
					}
					_currentDataSource = DataSource.ReadCacheOrNetwork;
					SavedGame.OpenWithManualConflictResolution(Filename, _currentDataSource, true, HandleOpenConflict, HandleOpenCompleted);
					break;
				default:
					_promise.TrySetException((Exception)new SavedGameRequestStatusException(requestStatus));
					break;
				}
			}
			catch (Exception ex)
			{
				_promise.TrySetException(ex);
			}
			finally
			{
				if (Defs.IsDeveloperBuild && ((Task)Future).IsFaulted)
				{
					Debug.LogFormat("Request failed with `{0}`: {1}", ((object)((Task)Future).Exception).GetType(), ((Exception)(object)((Task)Future).Exception).Message);
				}
				scopeLogger.Dispose();
			}
		}

		private void HandleCommitCompleted(SavedGameRequestStatus requestStatus, ISavedGameMetadata metadata)
		{
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePushHelper(`{0}`).HandleCommitCompleted(`{1}`, `{2}`)", Filename, requestStatus, GooglePullHelper.GetDescription(metadata)), Defs.IsDeveloperBuild);
			try
			{
				if (requestStatus == SavedGameRequestStatus.Success)
				{
					_promise.TrySetResult(metadata);
				}
				else
				{
					_promise.TrySetException((Exception)new SavedGameRequestStatusException(requestStatus));
				}
			}
			catch (Exception ex)
			{
				_promise.TrySetException(ex);
			}
			finally
			{
				if (Defs.IsDeveloperBuild && ((Task)Future).IsFaulted)
				{
					Debug.LogFormat("Request failed with `{0}`: {1}", ((object)((Task)Future).Exception).GetType(), ((Exception)(object)((Task)Future).Exception).Message);
				}
				scopeLogger.Dispose();
			}
		}
	}
}

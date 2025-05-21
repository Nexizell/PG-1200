using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class GooglePullHelper
	{
		private const string SavedGameClientIsNullMessage = "[Rilisoft] SavedGame client is null.";

		private bool _invoked;

		private DataSource _currentDataSource = DataSource.ReadNetworkOnly;

		private string _accumulatedMerge;

		private readonly bool _silent;

		private readonly string _filename;

		private readonly Func<string, string, string> _merger;

		private readonly TaskCompletionSource<string> _promise;

		private string Filename
		{
			get
			{
				return _filename;
			}
		}

		private Task<string> Future
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

		internal GooglePullHelper(string filename, bool silent, Func<string, string, string> merger, TaskCompletionSource<string> promise)
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
			_silent = silent;
			_merger = merger;
			_promise = promise;
		}

		internal void Run()
		{
			bool flag = GpgFacade.Instance.IsAuthenticated();
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePullHelper(`{0}`).Run(); isAuthenticated: `{1}`", Filename, flag), Defs.IsDeveloperBuild);
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
						GpgFacade.Instance.Authenticate(HandleAuthenticated, _silent);
					}
				}
			}
			finally
			{
				((IDisposable)scopeLogger).Dispose();
			}
		}

		internal static string GetDescription(ISavedGameMetadata metadata)
		{
			if (metadata == null)
			{
				return string.Empty;
			}
			return metadata.Description ?? string.Empty;
		}

		private static string ShortenSubstring(string s, int length)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			if (length <= 0)
			{
				return string.Empty;
			}
			if (length >= s.Length)
			{
				return s;
			}
			return s.Substring(0, length) + "...";
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

		private void HandleAuthenticated(bool authenticated)
		{
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePullHelper(`{0}`).HandleAuthenticated(`{1}`)", Filename, authenticated), Defs.IsDeveloperBuild);
			try
			{
				if (!authenticated)
				{
					_promise.TrySetException((Exception)new NotAuthenticatedException("Authentication callback failed"));
				}
				else if (!GpgFacade.Instance.IsAuthenticated())
				{
					_promise.TrySetException((Exception)new NotAuthenticatedException("Authentication callback succeeded, but user still not authenticated"));
				}
				else
				{
					RunAuthenticated();
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

		private void HandleOpenConflict(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
		{
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePullHelper(`{0}`).HandleOpenConflict(`{1}`, `{2}`)", Filename, GetDescription(original), GetDescription(unmerged)), Defs.IsDeveloperBuild);
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
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePullHelper(`{0}`).HandleOpenCompleted(`{1}`, `{2}`)", Filename, requestStatus, GetDescription(metadata)), Defs.IsDeveloperBuild);
			try
			{
				switch (requestStatus)
				{
				case SavedGameRequestStatus.Success:
					SavedGame.ReadBinaryData(metadata, HandleReadCompleted);
					break;
				case SavedGameRequestStatus.TimeoutError:
					if (_currentDataSource == DataSource.ReadCacheOrNetwork)
					{
						if (_accumulatedMerge != null)
						{
							_promise.TrySetResult(_accumulatedMerge);
						}
						else
						{
							_promise.TrySetException((Exception)new SavedGameRequestStatusException(requestStatus));
						}
					}
					else
					{
						_currentDataSource = DataSource.ReadCacheOrNetwork;
						SavedGame.OpenWithManualConflictResolution(Filename, _currentDataSource, true, HandleOpenConflict, HandleOpenCompleted);
					}
					break;
				case SavedGameRequestStatus.AuthenticationError:
					GpgFacade.Instance.Authenticate(HandleAuthenticated, true);
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

		private void HandleReadCompleted(SavedGameRequestStatus requestStatus, byte[] data)
		{
			ScopeLogger scopeLogger = new ScopeLogger(string.Format(CultureInfo.InvariantCulture, "GooglePullHelper(`{0}`).HandleReadCompleted(`{1}`, {2} bytes)", Filename, requestStatus, (data != null) ? data.Length : 0), Defs.IsDeveloperBuild);
			try
			{
				switch (requestStatus)
				{
				case SavedGameRequestStatus.Success:
				{
					string text = ((data != null) ? Encoding.UTF8.GetString(data, 0, data.Length) : string.Empty);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("unmergedResult: {0}", text);
					}
					string text2 = ((_accumulatedMerge != null) ? _merger(_accumulatedMerge, text) : text);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("result:         {0}", text2);
					}
					_promise.TrySetResult(text2);
					break;
				}
				case SavedGameRequestStatus.AuthenticationError:
					GpgFacade.Instance.Authenticate(HandleAuthenticated, true);
					break;
				default:
					if (_accumulatedMerge != null)
					{
						_promise.TrySetResult(_accumulatedMerge);
					}
					else
					{
						_promise.TrySetException((Exception)new SavedGameRequestStatusException(requestStatus));
					}
					break;
				}
			}
			catch (Exception ex)
			{
				_promise.TrySetException(ex);
			}
			finally
			{
				if (Defs.IsDeveloperBuild && ((Task)Future).IsCompleted)
				{
					if (((Task)Future).IsFaulted)
					{
						Debug.LogFormat("Request failed with `{0}`: {1}", ((object)((Task)Future).Exception).GetType(), ((Exception)(object)((Task)Future).Exception).Message);
					}
					else if (!((Task)Future).IsCanceled)
					{
						Debug.LogFormat("Request succeeded: `{0}`", ShortenSubstring(Future.Result, 32));
					}
				}
				scopeLogger.Dispose();
			}
		}
	}
}

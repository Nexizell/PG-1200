using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using UnityEngine;

[DisallowMultipleComponent]
public class NewsLobbyItem : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CLoadPreviewPicture_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NewsLobbyItem _003C_003E4__this;

		public string picLink;

		private Task<bool> _003CcurrentlyRunningRequest_003E5__1;

		private WWW _003CloadPic_003E5__2;

		private TaskCompletionSource<bool> _003Cpromise_003E5__3;

		private string _003CcachePath_003E5__4;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CLoadPreviewPicture_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (_003C_003E4__this.previewPic.mainTexture != null && _003C_003E4__this.previewPicUrl == picLink)
				{
					return false;
				}
				_003C_003E4__this.previewPic.width = 100;
				if (_003C_003E4__this.previewPic.mainTexture != null)
				{
					UnityEngine.Object.Destroy(_003C_003E4__this.previewPic.mainTexture);
				}
				if (s_currentlyRunningRequests.TryGetValue(picLink, out _003CcurrentlyRunningRequest_003E5__1))
				{
					if (Defs.IsDeveloperBuild && ((Task)_003CcurrentlyRunningRequest_003E5__1).IsCompleted)
					{
						UnityEngine.Debug.LogFormat("Request is completed: {0}", picLink);
					}
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					goto IL_0103;
				}
				goto IL_0110;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0103;
			case 2:
			{
				_003C_003E1__state = -1;
				Texture2D texture2D = null;
				return false;
			}
			case 3:
				_003C_003E1__state = -1;
				_003C_003E4__this.StartCoroutine(_003C_003E4__this.LoadPreviewPicture(picLink));
				return false;
			case 4:
			{
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003CloadPic_003E5__2.error))
				{
					_003Cpromise_003E5__3.TrySetException((Exception)new InvalidOperationException(_003CloadPic_003E5__2.error));
					s_currentlyRunningRequests.Remove(picLink);
					UnityEngine.Debug.LogWarning("Download preview pic error: " + _003CloadPic_003E5__2.error);
					if (_003CloadPic_003E5__2.error.StartsWith("Resolving host timed out"))
					{
						_003C_003E2__current = new WaitForSeconds(1f);
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_037d;
				}
				_003C_003E4__this.previewPicUrl = picLink;
				Texture2D texture2D2 = new Texture2D(2, 2);
				texture2D2.LoadImage(_003CloadPic_003E5__2.bytes);
				texture2D2.filterMode = FilterMode.Point;
				_003C_003E4__this.previewPic.mainTexture = texture2D2;
				_003C_003E4__this.previewPic.width = 100;
				if (!string.IsNullOrEmpty(_003CcachePath_003E5__4))
				{
					try
					{
						if (Defs.IsDeveloperBuild)
						{
							string text2 = (Application.isEditor ? ("<color=magenta>" + _003CcachePath_003E5__4 + "</color>") : _003CcachePath_003E5__4);
							UnityEngine.Debug.LogFormat("Trying to save preview to cache '{0}'", text2);
						}
						string directoryName = Path.GetDirectoryName(_003CcachePath_003E5__4);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						byte[] bytes = _003CloadPic_003E5__2.bytes;
						File.WriteAllBytes(_003CcachePath_003E5__4, bytes);
						_003Cpromise_003E5__3.TrySetResult(true);
					}
					catch (IOException ex)
					{
						UnityEngine.Debug.LogWarning("Caught IOException while saving preview to cache. See next message for details.");
						UnityEngine.Debug.LogException(ex);
						_003Cpromise_003E5__3.TrySetException((Exception)ex);
						s_currentlyRunningRequests.Remove(picLink);
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogWarning("Caught exception while saving preview to cache. See next message for details.");
						UnityEngine.Debug.LogException(ex2);
						_003Cpromise_003E5__3.TrySetException(ex2);
						s_currentlyRunningRequests.Remove(picLink);
					}
				}
				else
				{
					_003Cpromise_003E5__3.TrySetException((Exception)new InvalidOperationException("Cache path is null or empty."));
					s_currentlyRunningRequests.Remove(picLink);
				}
				using (new ScopeLogger("Dispose " + picLink, Defs.IsDeveloperBuild))
				{
					_003CloadPic_003E5__2.Dispose();
					_003CloadPic_003E5__2 = null;
				}
				return false;
			}
			case 5:
				{
					_003C_003E1__state = -1;
					if (Application.isEditor && FriendsController.isDebugLogWWW)
					{
						UnityEngine.Debug.Log("Reloading timed out pic");
					}
					_003C_003E4__this.StartCoroutine(_003C_003E4__this.LoadPreviewPicture(picLink));
					goto IL_037d;
				}
				IL_0110:
				_003CcachePath_003E5__4 = PersistentCache.Instance.GetCachePathByUri(picLink);
				if (!string.IsNullOrEmpty(_003CcachePath_003E5__4))
				{
					bool flag = File.Exists(_003CcachePath_003E5__4);
					if (Defs.IsDeveloperBuild && !flag)
					{
						string text = (Application.isEditor ? string.Format("<color=magenta>{0}</color>", new object[1] { _003CcachePath_003E5__4 }) : _003CcachePath_003E5__4);
						UnityEngine.Debug.LogFormat("Cache miss: '{0}'", text);
					}
					if (flag)
					{
						Texture2D texture2D = new Texture2D(2, 2);
						try
						{
							byte[] data = File.ReadAllBytes(_003CcachePath_003E5__4);
							texture2D.LoadImage(data);
							texture2D.filterMode = FilterMode.Point;
							_003C_003E4__this.previewPicUrl = picLink;
							_003C_003E4__this.previewPic.mainTexture = texture2D;
							_003C_003E4__this.previewPic.width = 100;
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogWarning("Caught exception while reading cached preview. See next message for details.");
							UnityEngine.Debug.LogException(exception);
						}
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
				}
				_003CloadPic_003E5__2 = Tools.CreateWwwIfNotConnected(picLink);
				if (_003CloadPic_003E5__2 == null)
				{
					_003C_003E2__current = new WaitForSeconds(60f);
					_003C_003E1__state = 3;
					return true;
				}
				_003Cpromise_003E5__3 = new TaskCompletionSource<bool>();
				s_currentlyRunningRequests[picLink] = _003Cpromise_003E5__3.Task;
				_003C_003E2__current = _003CloadPic_003E5__2;
				_003C_003E1__state = 4;
				return true;
				IL_0103:
				if (!((Task)_003CcurrentlyRunningRequest_003E5__1).IsCompleted)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0110;
				IL_037d:
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public GameObject indicatorNew;

	public UILabel headerLabel;

	public UILabel shortDescLabel;

	public UILabel dateLabel;

	public UITexture previewPic;

	public string previewPicUrl;

	public UIToggle currentToogle;

	private static readonly Dictionary<string, Task<bool>> s_currentlyRunningRequests = new Dictionary<string, Task<bool>>();

	public void LoadPreview(string url)
	{
		StartCoroutine(LoadPreviewPicture(url));
	}

	private IEnumerator LoadPreviewPicture(string picLink)
	{
		if (previewPic.mainTexture != null && previewPicUrl == picLink)
		{
			yield break;
		}
		previewPic.width = 100;
		if (previewPic.mainTexture != null)
		{
			UnityEngine.Object.Destroy(previewPic.mainTexture);
		}
		Task<bool> currentlyRunningRequest;
		if (s_currentlyRunningRequests.TryGetValue(picLink, out currentlyRunningRequest))
		{
			if (Defs.IsDeveloperBuild && ((Task)currentlyRunningRequest).IsCompleted)
			{
				UnityEngine.Debug.LogFormat("Request is completed: {0}", picLink);
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			while (!((Task)currentlyRunningRequest).IsCompleted)
			{
				yield return null;
			}
		}
		string cachePath = PersistentCache.Instance.GetCachePathByUri(picLink);
		if (!string.IsNullOrEmpty(cachePath))
		{
			bool flag = File.Exists(cachePath);
			if (Defs.IsDeveloperBuild && !flag)
			{
				string text = (Application.isEditor ? string.Format("<color=magenta>{0}</color>", new object[1] { cachePath }) : cachePath);
				UnityEngine.Debug.LogFormat("Cache miss: '{0}'", text);
			}
			if (flag)
			{
				Texture2D texture2D = new Texture2D(2, 2);
				try
				{
					byte[] data = File.ReadAllBytes(cachePath);
					texture2D.LoadImage(data);
					texture2D.filterMode = FilterMode.Point;
					previewPicUrl = picLink;
					previewPic.mainTexture = texture2D;
					previewPic.width = 100;
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogWarning("Caught exception while reading cached preview. See next message for details.");
					UnityEngine.Debug.LogException(exception);
				}
				yield return null;
				yield break;
			}
		}
		WWW loadPic = Tools.CreateWwwIfNotConnected(picLink);
		if (loadPic == null)
		{
			yield return new WaitForSeconds(60f);
			StartCoroutine(LoadPreviewPicture(picLink));
			yield break;
		}
		TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
		s_currentlyRunningRequests[picLink] = promise.Task;
		yield return loadPic;
		if (!string.IsNullOrEmpty(loadPic.error))
		{
			promise.TrySetException((Exception)new InvalidOperationException(loadPic.error));
			s_currentlyRunningRequests.Remove(picLink);
			UnityEngine.Debug.LogWarning("Download preview pic error: " + loadPic.error);
			if (loadPic.error.StartsWith("Resolving host timed out"))
			{
				yield return new WaitForSeconds(1f);
				if (Application.isEditor && FriendsController.isDebugLogWWW)
				{
					UnityEngine.Debug.Log("Reloading timed out pic");
				}
				StartCoroutine(LoadPreviewPicture(picLink));
			}
			yield break;
		}
		previewPicUrl = picLink;
		Texture2D texture2D2 = new Texture2D(2, 2);
		texture2D2.LoadImage(loadPic.bytes);
		texture2D2.filterMode = FilterMode.Point;
		previewPic.mainTexture = texture2D2;
		previewPic.width = 100;
		if (!string.IsNullOrEmpty(cachePath))
		{
			try
			{
				if (Defs.IsDeveloperBuild)
				{
					string text2 = (Application.isEditor ? ("<color=magenta>" + cachePath + "</color>") : cachePath);
					UnityEngine.Debug.LogFormat("Trying to save preview to cache '{0}'", text2);
				}
				string directoryName = Path.GetDirectoryName(cachePath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				byte[] bytes = loadPic.bytes;
				File.WriteAllBytes(cachePath, bytes);
				promise.TrySetResult(true);
			}
			catch (IOException ex)
			{
				UnityEngine.Debug.LogWarning("Caught IOException while saving preview to cache. See next message for details.");
				UnityEngine.Debug.LogException(ex);
				promise.TrySetException((Exception)ex);
				s_currentlyRunningRequests.Remove(picLink);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogWarning("Caught exception while saving preview to cache. See next message for details.");
				UnityEngine.Debug.LogException(ex2);
				promise.TrySetException(ex2);
				s_currentlyRunningRequests.Remove(picLink);
			}
		}
		else
		{
			promise.TrySetException((Exception)new InvalidOperationException("Cache path is null or empty."));
			s_currentlyRunningRequests.Remove(picLink);
		}
		using (new ScopeLogger("Dispose " + picLink, Defs.IsDeveloperBuild))
		{
			loadPic.Dispose();
		}
	}

	private void OnDisable()
	{
		if (previewPic.mainTexture != null)
		{
			UnityEngine.Object.Destroy(previewPic.mainTexture);
			previewPic.mainTexture = null;
		}
	}

	public void OnNewsItemClick()
	{
		if (!currentToogle.value)
		{
			return;
		}
		ButtonClickSound.TryPlayClick();
		for (int i = 0; i < NewsLobbyController.sharedController.newsList.Count; i++)
		{
			if (NewsLobbyController.sharedController.newsList[i].Equals(this))
			{
				NewsLobbyController.sharedController.SetNewsIndex(i);
				break;
			}
		}
	}
}

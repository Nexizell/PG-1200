using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class AchievementsGUIController : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CPopulateViews_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AchievementsGUIController _003C_003E4__this;

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
			public _003CPopulateViews_003Ed__6(int _003C_003E1__state)
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
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (!Singleton<AchievementsManager>.Instance.IsReady)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				foreach (Achievement availableAchiement in Singleton<AchievementsManager>.Instance.AvailableAchiements)
				{
					_003C_003E4__this.CreateView(availableAchiement);
					_003C_003E4__this._grid.Reposition();
					_003C_003E4__this._scrollView.ResetPosition();
				}
				Singleton<AchievementsManager>.Instance.AvailableAchiements.OnItemInserted += _003C_003E4__this.OnAchievementAdded;
				Singleton<AchievementsManager>.Instance.AvailableAchiements.OnItemRemoved += _003C_003E4__this.OnAchievementRemoved;
				return false;
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

		[SerializeField]
		protected internal UIScrollView _scrollView;

		[SerializeField]
		protected internal UIGrid _grid;

		[SerializeField]
		protected internal AchievementView _viewPrefab;

		[ReadOnly]
		[SerializeField]
		protected internal List<AchievementView> _views = new List<AchievementView>();

		[SerializeField]
		protected internal AchievementInfoView _infoView;

		private void Awake()
		{
			StartCoroutine(PopulateViews());
			AchievementView.OnClicked += AchievementView_OnClicked;
		}

		private IEnumerator PopulateViews()
		{
			while (!Singleton<AchievementsManager>.Instance.IsReady)
			{
				yield return null;
			}
			foreach (Achievement availableAchiement in Singleton<AchievementsManager>.Instance.AvailableAchiements)
			{
				CreateView(availableAchiement);
				_grid.Reposition();
				_scrollView.ResetPosition();
			}
			Singleton<AchievementsManager>.Instance.AvailableAchiements.OnItemInserted += OnAchievementAdded;
			Singleton<AchievementsManager>.Instance.AvailableAchiements.OnItemRemoved += OnAchievementRemoved;
		}

		private AchievementView CreateView(Achievement ach)
		{
			AchievementView achievementView = UnityEngine.Object.Instantiate(_viewPrefab);
			achievementView.Achievement = ach;
			_views.Add(achievementView);
			achievementView.gameObject.transform.SetParent(_grid.gameObject.transform);
			achievementView.gameObject.transform.localPosition = Vector3.zero;
			achievementView.gameObject.transform.localScale = Vector3.one;
			return achievementView;
		}

		private void OnAchievementAdded(int pos, Achievement ach)
		{
			CreateView(ach).gameObject.transform.SetSiblingIndex(pos);
			_grid.Reposition();
		}

		private void OnAchievementRemoved(int pos, Achievement ach)
		{
			AchievementView achievementView = _views.FirstOrDefault((AchievementView v) => v.Achievement == ach);
			if (achievementView != null)
			{
				_views.Remove(achievementView);
				achievementView.gameObject.transform.SetParent(base.gameObject.transform);
				achievementView.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(achievementView);
			}
		}

		private void AchievementView_OnClicked(AchievementView obj)
		{
			_infoView.Show(obj.Achievement);
		}

		private void OnDestroy()
		{
			AchievementView.OnClicked -= AchievementView_OnClicked;
		}
	}
}

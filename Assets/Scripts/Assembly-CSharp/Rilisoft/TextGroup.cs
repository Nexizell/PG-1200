using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using UnityEngine;

namespace Rilisoft
{
	[ExecuteInEditMode]
	public class TextGroup : MonoBehaviour
	{
		[SerializeField]
		[ReadOnly]
		protected internal List<UILabel> _labels = new List<UILabel>();

		[SerializeField]
		protected internal string _text;

		[SerializeField]
		protected internal string _localizationKey;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				if (_labels == null || !_labels.Any())
				{
					SetLabels();
				}
				RiliExtensions.ForEach(_labels, delegate(UILabel l)
				{
					l.text = _text;
				});
			}
		}

		public string LocalizationKey
		{
			get
			{
				return _localizationKey;
			}
			set
			{
				_localizationKey = value;
				if (!_localizationKey.IsNullOrEmpty())
				{
					if (UseLocalizationComponents)
					{
						SetLocalizeComponents();
					}
					else
					{
						Text = LocalizationStore.Get(value);
					}
				}
				else
				{
					Text = _text;
				}
			}
		}

		public bool UseLocalizationComponents
		{
			get
			{
				int num = 0;
				Localize[] components = GetComponents<Localize>();
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i].Term == "Key_04B_03")
					{
						num++;
					}
				}
				return components.Length < num;
			}
		}

		private void OnEnable()
		{
			LocalizationStore.AddEventCallAfterLocalize(HandleLocalizationChanged);
			SetLabels();
			if (UseLocalizationComponents)
			{
				SetLocalizeComponents();
			}
			else if (!LocalizationKey.IsNullOrEmpty())
			{
				Text = LocalizationStore.Get(LocalizationKey);
			}
			else
			{
				Text = _text;
			}
		}

		private void OnDisable()
		{
			LocalizationStore.DelEventCallAfterLocalize(HandleLocalizationChanged);
		}

		private void SetLabels()
		{
			if (_labels == null)
			{
				_labels = new List<UILabel>();
			}
			else
			{
				_labels.Clear();
			}
			_labels.AddRange(GetComponentsInChildren<UILabel>(true));
		}

		private void HandleLocalizationChanged()
		{
			if (!LocalizationKey.IsNullOrEmpty())
			{
				if (UseLocalizationComponents)
				{
					SetLocalizeComponents();
				}
				else
				{
					Text = LocalizationStore.Get(LocalizationKey);
				}
			}
			else
			{
				Text = _text;
			}
		}

		private void SetLocalizeComponents()
		{
			foreach (UILabel label in _labels)
			{
				Localize localize = label.gameObject.GetComponent<Localize>();
				if (localize == null)
				{
					localize = label.gameObject.AddComponent<Localize>();
				}
				localize.Term = LocalizationKey;
			}
			Text = LocalizationStore.Get(LocalizationKey);
		}
	}
}

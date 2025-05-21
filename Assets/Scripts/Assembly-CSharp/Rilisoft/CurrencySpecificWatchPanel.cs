using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public sealed class CurrencySpecificWatchPanel : MonoBehaviour
	{
		[SerializeField]
		protected internal UILabel watchHeader;

		[SerializeField]
		protected internal UILabel watchTimer;

		[SerializeField]
		protected internal UIButton watchButton;

		public UILabel WatchHeader
		{
			get
			{
				return watchHeader;
			}
		}

		public UILabel WatchTimer
		{
			get
			{
				return watchTimer;
			}
		}

		public UIButton WatchButton
		{
			get
			{
				return watchButton;
			}
		}
	}
}

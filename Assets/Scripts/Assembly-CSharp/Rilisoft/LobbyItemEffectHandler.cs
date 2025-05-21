using System;
using UnityEngine;

namespace Rilisoft
{
	public abstract class LobbyItemEffectHandler : MonoBehaviour
	{
		[ReadOnly]
		[SerializeField]
		protected internal string _lobbyItemId = "not setted";

		protected LobbyItem LobbyItem { get; private set; }

		protected string StorragerKey
		{
			get
			{
				if (LobbyItem == null)
				{
					return null;
				}
				return string.Format("lobby_item_effect_{0}", new object[1] { LobbyItem.Info.Id });
			}
		}

		public void Inicialize(LobbyItem lobbyItem)
		{
			if (lobbyItem == null)
			{
				throw new ArgumentNullException("LobbyItemEffectHandler.Inicialize fail: lobbyItem is null");
			}
			LobbyItem = lobbyItem;
			_lobbyItemId = LobbyItem.Info.Id;
			Setup();
		}

		public virtual void Remove()
		{
		}

		protected virtual void Setup()
		{
		}

		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void OnDisable()
		{
		}

		public LobbyItemEffectHandler()
		{
		}
	}
}

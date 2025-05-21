using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public sealed class TicketsPointMemento : AdPointMementoBase
	{
		public int Reward { get; private set; }

		public bool SimplifiedInterface { get; private set; }

		public static int DefaultReward
		{
			get
			{
				return 1;
			}
		}

		public static bool DefaultSimplifiedInterface
		{
			get
			{
				return true;
			}
		}

		internal static TicketsPointMemento FromObject(string id, object obj)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (obj == null)
			{
				return null;
			}
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return null;
			}
			TicketsPointMemento ticketsPointMemento = new TicketsPointMemento(id);
			ticketsPointMemento.Reset(dictionary);
			int? @int = ParsingHelper.GetInt32(dictionary, "reward");
			if (@int.HasValue)
			{
				ticketsPointMemento.Reward = @int.Value;
			}
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "simplifiedInterface");
			if (boolean.HasValue)
			{
				ticketsPointMemento.SimplifiedInterface = boolean.Value;
			}
			return ticketsPointMemento;
		}

		public TicketsPointMemento(string id)
			: base(id)
		{
			Reward = DefaultReward;
			SimplifiedInterface = DefaultSimplifiedInterface;
		}

		public int GetFinalReward(string category)
		{
			int? int32Override = GetInt32Override("reward", category);
			if (int32Override.HasValue)
			{
				return int32Override.Value;
			}
			return Reward;
		}

		public bool GetFinalSimplifiedInterface(string category)
		{
			bool? booleanOverride = GetBooleanOverride("simplifiedInterface", category);
			if (booleanOverride.HasValue)
			{
				return booleanOverride.Value;
			}
			return SimplifiedInterface;
		}
	}
}

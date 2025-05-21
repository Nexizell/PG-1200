using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public sealed class FreeCheckpointsPointMemento : AdPointMementoBase
	{
		public int Reward { get; private set; }

		public static int DefaultReward
		{
			get
			{
				return 1;
			}
		}

		public FreeCheckpointsPointMemento(string id)
			: base(id)
		{
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

		internal static FreeCheckpointsPointMemento FromObject(string id, object obj)
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
			FreeCheckpointsPointMemento freeCheckpointsPointMemento = new FreeCheckpointsPointMemento(id);
			freeCheckpointsPointMemento.Reset(dictionary);
			int? @int = ParsingHelper.GetInt32(dictionary, "reward");
			if (@int.HasValue)
			{
				freeCheckpointsPointMemento.Reward = @int.Value;
			}
			return freeCheckpointsPointMemento;
		}
	}
}

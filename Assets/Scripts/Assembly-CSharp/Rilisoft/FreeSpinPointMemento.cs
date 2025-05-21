using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public sealed class FreeSpinPointMemento : AdPointMementoBase
	{
		public int Reward { get; private set; }

		public static int DefaultReward
		{
			get
			{
				return 1;
			}
		}

		public static double DefaultTimeoutBetweenShowInMinutes
		{
			get
			{
				return 1440.0;
			}
		}

		[Obsolete("Сейчас не используется.")]
		public double TimeoutBetweenShowInMinutes { get; private set; }

		public FreeSpinPointMemento(string id)
			: base(id)
		{
			Reward = DefaultReward;
			TimeoutBetweenShowInMinutes = DefaultTimeoutBetweenShowInMinutes;
		}

		[Obsolete("Сейчас не используется.")]
		public double GetFinalTimeoutBetweenShowInMinutes(string category)
		{
			double? doubleOverride = GetDoubleOverride("timeoutBetweenShowMinutes", category);
			if (doubleOverride.HasValue)
			{
				return doubleOverride.Value;
			}
			return TimeoutBetweenShowInMinutes;
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

		internal static FreeSpinPointMemento FromObject(string id, object obj)
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
			FreeSpinPointMemento freeSpinPointMemento = new FreeSpinPointMemento(id);
			freeSpinPointMemento.Reset(dictionary);
			int? @int = ParsingHelper.GetInt32(dictionary, "reward");
			if (@int.HasValue)
			{
				freeSpinPointMemento.Reward = @int.Value;
			}
			double? @double = ParsingHelper.GetDouble(dictionary, "timeoutBetweenShowMinutes");
			if (@double.HasValue)
			{
				freeSpinPointMemento.TimeoutBetweenShowInMinutes = @double.Value;
			}
			return freeSpinPointMemento;
		}
	}
}

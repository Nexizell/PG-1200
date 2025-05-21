using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	public sealed class SurviveWaveInArenaEventArgs : EventArgs
	{
		public int WaveNumber { get; set; }

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object> { { "waveNumber", WaveNumber } };
		}

		public override string ToString()
		{
			return Json.Serialize(ToJson());
		}
	}
}

using System;
using GooglePlayGames.BasicApi.SavedGame;

namespace Rilisoft
{
	internal sealed class SavedGameRequestStatusException : Exception
	{
		private readonly SavedGameRequestStatus _requestStatus;

		public SavedGameRequestStatus Status
		{
			get
			{
				return _requestStatus;
			}
		}

		public SavedGameRequestStatusException(SavedGameRequestStatus requestStatus)
			: base(FormatMesage(requestStatus))
		{
			_requestStatus = requestStatus;
		}

		private static string FormatMesage(SavedGameRequestStatus requestStatus)
		{
			return "SavedGameRequestStatus." + requestStatus;
		}
	}
}

using UnityEngine;

public class UniPasteBoard
{
	public static string GetClipBoardString()
	{
		Debug.LogWarning("Get clip board content is forbidden in WP8. Returining empty string");
		return "";
	}

	public static void SetClipBoardString(string text)
	{
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugButtonsManager : MonoBehaviour
{
	internal class TopBarButton
	{
		public bool NeedShow = true;

		public string Text { get; private set; }

		public int Width { get; private set; }

		public Action Act { get; private set; }

		public int SortIdx { get; set; }

		public TopBarButton(string text, int width, Action act, int sortIdx)
		{
			Text = text;
			Width = width;
			Act = act;
		}
	}

	private static DebugButtonsManager _instance;

	private static bool _topPanelOpened = true;

	private static readonly List<TopBarButton> _topBarButtons = new List<TopBarButton>();

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public static void ShowTopBarButton(string text, int width, Action onClickAction, int sortIdx = 0)
	{
		if (_instance == null)
		{
			_instance = new GameObject("DebugButtonsManager").AddComponent<DebugButtonsManager>();
		}
		TopBarButton topBarButton = _topBarButtons.FirstOrDefault((TopBarButton b) => b.Text == text);
		if (topBarButton != null)
		{
			topBarButton.NeedShow = true;
			topBarButton.SortIdx = sortIdx;
		}
		else
		{
			TopBarButton item = new TopBarButton(text, width, onClickAction, sortIdx);
			_topBarButtons.Add(item);
		}
	}
}

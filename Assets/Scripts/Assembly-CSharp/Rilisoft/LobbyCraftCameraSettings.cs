using System;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyCraftCameraSettings : MonoBehaviour
	{
		[SerializeField]
		protected internal UIToggle _toggleCommon;

		[SerializeField]
		protected internal UIToggle _toggleLow;

		[SerializeField]
		protected internal UIToggle _toggleHight;

		private MainMenuHeroCamera.CameraPositionPreset presetOnEnter;

		private void OnEnable()
		{
			MainMenuHeroCamera.CameraPositionPreset currentCameraPresset = MainMenuHeroCamera.Instance.GetCurrentCameraPresset();
			switch (currentCameraPresset)
			{
			case MainMenuHeroCamera.CameraPositionPreset.Common:
				_toggleCommon.Set(true);
				break;
			case MainMenuHeroCamera.CameraPositionPreset.Low:
				_toggleLow.Set(true);
				break;
			case MainMenuHeroCamera.CameraPositionPreset.Hight:
				_toggleHight.Set(true);
				break;
			}
			EventDelegate.Add(_toggleCommon.onChange, OnCommonChanged);
			EventDelegate.Add(_toggleLow.onChange, OnLowChanged);
			EventDelegate.Add(_toggleHight.onChange, OnHightChanged);
			presetOnEnter = currentCameraPresset;
		}

		private void OnDisable()
		{
			EventDelegate.Remove(_toggleCommon.onChange, OnCommonChanged);
			EventDelegate.Remove(_toggleLow.onChange, OnLowChanged);
			EventDelegate.Remove(_toggleHight.onChange, OnHightChanged);
			try
			{
				MainMenuHeroCamera.CameraPositionPreset currentCameraPresset = MainMenuHeroCamera.Instance.GetCurrentCameraPresset();
				if (presetOnEnter != currentCameraPresset)
				{
					AnalyticsStuff.HousingSettings(currentCameraPresset);
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in sending HousingSettings: {0}", ex);
			}
		}

		private void ChangePreset(MainMenuHeroCamera.CameraPositionPreset preset)
		{
			if (MainMenuHeroCamera.Instance.GetCurrentCameraPresset() != preset)
			{
				MainMenuHeroCamera.Instance.SetCameraPreset(preset);
			}
		}

		private void OnCommonChanged()
		{
			if (_toggleCommon.value)
			{
				ChangePreset(MainMenuHeroCamera.CameraPositionPreset.Common);
			}
		}

		private void OnLowChanged()
		{
			if (_toggleLow.value)
			{
				ChangePreset(MainMenuHeroCamera.CameraPositionPreset.Low);
			}
		}

		private void OnHightChanged()
		{
			if (_toggleHight.value)
			{
				ChangePreset(MainMenuHeroCamera.CameraPositionPreset.Hight);
			}
		}
	}
}

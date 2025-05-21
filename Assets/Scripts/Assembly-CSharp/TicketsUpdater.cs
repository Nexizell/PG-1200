using System.Collections.Generic;
using Holoville.HOTween;
using Rilisoft;
using UnityEngine;

public class TicketsUpdater : MonoBehaviour
{
	public List<UILabel> labels;

	public UISprite blinkObj;

	public bool ignoreX3;

	public AudioClip clipTicketsAdded;

	private static Color? m_blinkColor;

	private float m_lastUpdateTime = float.MinValue;

	private const string BLINK_SEQUENCE_ID = "TicketsUpdater_blink_sequence";

	private static Color BlinkColor
	{
		get
		{
			if (!m_blinkColor.HasValue)
			{
				m_blinkColor = new Color(1f, 0f, 0f, 40f / 51f);
			}
			return m_blinkColor.Value;
		}
	}

	private void Start()
	{
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		UpdateTickets();
		blinkObj.color = NormalColor();
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - m_lastUpdateTime >= 1f)
		{
			UpdateTickets();
		}
	}

	private void OnEnable()
	{
		BankController.onUpdateMoney += UpdateTickets;
		BankController.TicketsAdded += BankController_TicketsAdded;
		PromoActionsManager.EventX3Updated += PromoActionsManager_EventX3Updated;
	}

	private void PromoActionsManager_EventX3Updated()
	{
		StopBlinking();
		blinkObj.color = NormalColor();
	}

	private void BankController_TicketsAdded(int count)
	{
		StopBlinking();
		TweenParms p_parms = new TweenParms().Prop("color", BlinkColor).Ease(EaseType.Linear);
		TweenParms p_parms2 = new TweenParms().Prop("color", NormalColor()).Ease(EaseType.Linear);
		Sequence sequence = new Sequence(new SequenceParms().Id("TicketsUpdater_blink_sequence").Loops(15).UpdateType(UpdateType.TimeScaleIndependentUpdate));
		float p_duration = 0.1f;
		sequence.Append(HOTween.To(blinkObj, p_duration, p_parms));
		sequence.Append(HOTween.To(blinkObj, p_duration, p_parms2));
		sequence.Play();
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(clipTicketsAdded);
		}
	}

	private void StopBlinking()
	{
		HOTween.Kill("TicketsUpdater_blink_sequence");
		blinkObj.color = NormalColor();
	}

	private void OnDisable()
	{
		BankController.onUpdateMoney -= UpdateTickets;
		BankController.TicketsAdded -= BankController_TicketsAdded;
		PromoActionsManager.EventX3Updated -= PromoActionsManager_EventX3Updated;
		StopBlinking();
	}

	private void OnDestroy()
	{
		StopBlinking();
	}

	private void UpdateTickets()
	{
		m_lastUpdateTime = Time.realtimeSinceStartup;
		RiliExtensions.ForEach(labels, delegate(UILabel l)
		{
			l.text = BankController.NumOfTickets().ToString();
		});
	}

	private Color NormalColor()
	{
		if (ignoreX3 || !(PromoActionsManager.sharedManager != null) || !PromoActionsManager.sharedManager.IsEventX3Active)
		{
			return new Color(0f, 0f, 0f, 23f / 51f);
		}
		return new Color(1f, 0f, 0f, 0.5882353f);
	}
}

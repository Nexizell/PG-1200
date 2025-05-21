using UnityEngine;

namespace Rilisoft
{
	[RequireComponent(typeof(PetEngine))]
	public class PetHealingComponent : MonoBehaviour
	{
		private PetEngine _engine;

		private float _startWaitTime;

		private PetState _currentState;

		private float CurrentTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		private void OnEnable()
		{
			_engine = GetComponent<PetEngine>();
			if (_engine == null)
			{
				Debug.LogErrorFormat("PET: '{0}' PetEngine not found", base.gameObject.name);
				base.enabled = false;
			}
			else
			{
				_engine.OnStateChanged -= EngineOnStateChanged;
				_engine.OnStateChanged += EngineOnStateChanged;
				_startWaitTime = CurrentTime;
			}
		}

		private void EngineOnStateChanged(PetState currState, PetState prevState)
		{
			_currentState = currState;
		}

		private void Update()
		{
			if (!(_engine == null) && _engine.enabled && _engine.IsMine && !(_engine.Owner == null) && _engine.Info != null && !(_engine.Info.HealDelaySecs < 1f) && (!_engine.Info.HealPowerSelf.IsZero() || !_engine.Info.HealPowerOwner.IsZero()))
			{
				if (!_engine.CanShowPet() || _currentState == PetState.respawn || _currentState == PetState.death)
				{
					_startWaitTime = CurrentTime;
				}
				else if (CurrentTime - _startWaitTime >= _engine.Info.HealDelaySecs)
				{
					_startWaitTime = CurrentTime;
					Heal();
				}
			}
		}

		public void Heal()
		{
			if (!(_engine == null) && _engine.enabled && !(_engine.Owner == null) && _engine.IsMine && _engine.Info != null && _engine.CanShowPet())
			{
				if (_engine.Info.HealPowerSelf > 0f)
				{
					_engine.AddCurrentHealth(_engine.Info.HealPowerSelf);
				}
				if (_engine.Owner != null && _engine.Info.HealPowerOwner > 0f)
				{
					_engine.Owner.AddHealth(_engine.Info.HealPowerOwner);
				}
			}
		}
	}
}

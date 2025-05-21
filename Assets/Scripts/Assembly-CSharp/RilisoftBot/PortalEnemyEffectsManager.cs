using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RilisoftBot
{
	[RequireComponent(typeof(BaseBot))]
	public class PortalEnemyEffectsManager : MonoBehaviour, IEnemyEffectsManager
	{
		[CompilerGenerated]
		internal sealed class _003CShowSpawnMaterialsCoroutine_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public PortalEnemyEffectsManager _003C_003E4__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CShowSpawnMaterialsCoroutine_003Ed__9(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				case 1:
				{
					_003C_003E1__state = -1;
					Renderer[] componentsInChildren = _003C_003E4__this.GetComponentsInChildren<Renderer>();
					foreach (Renderer rend in componentsInChildren)
					{
						_003C_003E4__this.StartCoroutine(_003C_003E4__this.AnimateMaterial(rend));
					}
					return false;
				}
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		internal sealed class _003CAnimateMaterial_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public Renderer rend;

			public PortalEnemyEffectsManager _003C_003E4__this;

			private float _003CtimeElapsed_003E5__1;

			private Material _003CbaseMaterial_003E5__2;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CAnimateMaterial_003Ed__10(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CbaseMaterial_003E5__2 = rend.material;
					if (rend.gameObject.GetComponent<BotChangeDamageMaterial>() != null)
					{
						string key = _003C_003E4__this._bot.name + "_Level" + CurrentCampaignGame.currentLevel;
						Texture texture = SkinsManagerPixlGun.sharedManager.skins[key] as Texture;
						if (texture != null)
						{
							_003CbaseMaterial_003E5__2.mainTexture = texture;
						}
					}
					rend.material = new Material(_003C_003E4__this._portalMaterialPref);
					rend.material.mainTexture = _003CbaseMaterial_003E5__2.mainTexture;
					rend.material.SetFloat("_Burn", 0.25f);
					_003CtimeElapsed_003E5__1 = 0f;
					goto IL_014a;
				case 1:
					_003C_003E1__state = -1;
					goto IL_014a;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_014a:
					if (_003CtimeElapsed_003E5__1 < 1f)
					{
						_003CtimeElapsed_003E5__1 += Time.deltaTime;
						float value = _003CtimeElapsed_003E5__1 * 1.25f / 1f;
						rend.material.SetFloat("_Burn", value);
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					rend.material = _003CbaseMaterial_003E5__2;
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private BaseBot _bot;

		private Material _portalMaterialPref;

		private const string SpawnShaderParamName = "_Burn";

		private const float SpawnPlayTime = 1f;

		private const float SpawnBurnAmountStart = 0.25f;

		private const float SpawnBurnAmountEnd = 1.25f;

		private void Awake()
		{
			_bot = GetComponent<BaseBot>();
			_portalMaterialPref = Resources.Load<Material>("Enemy_Portal");
			if (_portalMaterialPref == null)
			{
				UnityEngine.Debug.LogError("material not found");
			}
		}

		public void ShowSpawnEffect()
		{
			ShowSpawnMaterials();
			ShowSpawnPortal();
		}

		private void ShowSpawnMaterials()
		{
			StartCoroutine(ShowSpawnMaterialsCoroutine());
		}

		private IEnumerator ShowSpawnMaterialsCoroutine()
		{
			yield return null;
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			foreach (Renderer rend in componentsInChildren)
			{
				StartCoroutine(AnimateMaterial(rend));
			}
		}

		private IEnumerator AnimateMaterial(Renderer rend)
		{
			Material baseMaterial = rend.material;
			if (rend.gameObject.GetComponent<BotChangeDamageMaterial>() != null)
			{
				string key = _bot.name + "_Level" + CurrentCampaignGame.currentLevel;
				Texture texture = SkinsManagerPixlGun.sharedManager.skins[key] as Texture;
				if (texture != null)
				{
					baseMaterial.mainTexture = texture;
				}
			}
			rend.material = new Material(_portalMaterialPref);
			rend.material.mainTexture = baseMaterial.mainTexture;
			rend.material.SetFloat("_Burn", 0.25f);
			float timeElapsed = 0f;
			while (timeElapsed < 1f)
			{
				timeElapsed += Time.deltaTime;
				float value = timeElapsed * 1.25f / 1f;
				rend.material.SetFloat("_Burn", value);
				yield return null;
			}
			rend.material = baseMaterial;
			yield return null;
		}

		private void ShowSpawnPortal()
		{
			EnemyPortal portal = EnemyPortalStackController.sharedController.GetPortal();
			if (!(portal == null))
			{
				portal.Show(base.gameObject.transform.position);
			}
		}
	}
}

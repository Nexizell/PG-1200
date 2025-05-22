using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class DestroyRubble : MonoBehaviour
{
    /*[Serializable]
    [CompilerGenerated]
    internal sealed class _0024Start_002438 : GenericGenerator<WaitForSeconds>
    {
        [Serializable]
        [CompilerGenerated]
        internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
        {
            internal int _0024i_002439;

            internal DestroyRubble _0024self__002440;

            public _0024(DestroyRubble self_)
            {
                _0024self__002440 = self_;
            }

            public override bool MoveNext()
            {
                int result;
                switch (_state)
                {
                    default:
                        result = (Yield(2, new WaitForSeconds(_0024self__002440.time)) ? 1 : 0);
                        break;
                    case 2:
                        _0024i_002439 = 0;
                        goto IL_0095;
                    case 3:
                        UnityEngine.Object.Destroy(_0024self__002440.gameObject);
                        _0024i_002439++;
                        goto IL_0095;
                    case 1:
                        {
                            result = 0;
                            break;
                        }
                    IL_0095:
                        if (_0024i_002439 < Extensions.get_length((System.Array)_0024self__002440.particleEmitters))
                        {
                            _0024self__002440.particleEmitters[_0024i_002439].emit = false;
                            result = (Yield(3, new WaitForSeconds(_0024self__002440.maxTime)) ? 1 : 0);
                            break;
                        }
                        YieldDefault(1);
                        goto case 1;
                }
                return (byte)result != 0;
            }
        }

        internal DestroyRubble _0024self__002441;

        public _0024Start_002438(DestroyRubble self_)
        {
            _0024self__002441 = self_;
        }

        public override IEnumerator<WaitForSeconds> GetEnumerator()
        {
            return new _0024(_0024self__002441);
        }
    }*/

    public float maxTime;

    public ParticleSystem[] particleEmitters;

    public float time;

    public DestroyRubble()
    {
        maxTime = 3f;
    }

    public virtual IEnumerator Start()
    {
        yield return new WaitForSeconds(this.time);
        for (int i = 0; i < this.particleEmitters.Length; i++)
        {
            this.particleEmitters[i].Stop();
            yield return new WaitForSeconds(this.maxTime);
        }
        Destroy(this.gameObject);
        //return new _0024Start_002438(this).GetEnumerator();
    }

    public virtual void Main()
    {
    }
}

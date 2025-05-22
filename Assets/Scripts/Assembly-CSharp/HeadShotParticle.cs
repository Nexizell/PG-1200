using UnityEngine;

public class HeadShotParticle : MonoBehaviour
{
    private float liveTime = -1f;

    public float maxliveTime = 1.5f;

    public bool isUseMine;

    private Transform myTransform;

    public ParticleSystem myParticleSystem;

    private void Start()
    {
        myTransform = base.transform;
        myTransform.position = new Vector3(-10000f, -10000f, -10000f);
        myParticleSystem.Stop();
    }

    public void StartShowParticle(Vector3 pos, Quaternion rot, bool _isUseMine)
    {
        isUseMine = _isUseMine;
        liveTime = maxliveTime;
        myTransform.position = pos;
        myTransform.rotation = rot;
        myParticleSystem.Play();
    }

    private void Update()
    {
        if (!(liveTime < 0f))
        {
            liveTime -= Time.deltaTime;
            if (liveTime < 0f)
            {
                myTransform.position = new Vector3(-10000f, -10000f, -10000f);
                myParticleSystem.Stop();
                isUseMine = false;
            }
        }
    }
}

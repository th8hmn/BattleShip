using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject particleSysObj;
    private ParticleSystem particle;
    // particle sysytemÇ™playingÇÃÇ∆Ç´1ÅAstoppedÇÃÇ∆Ç´0
    private bool preStatus;
    private bool status;
    private bool particleFall;

    // Start is called before the first frame update
    void Start()
    {
        particle = particleSysObj.GetComponent<ParticleSystem>();
        particle.Stop();
        status = false;
        preStatus = false;
        particleFall = false;
    }

    // Update is called once per frame
    void Update()
    {
        //UnityEngine.Debug.Log(GetParticleSystemStatus());
    }

    public void PlayExplosionEffect(Vector3 position)
    {
        particleSysObj.transform.position = position;
        particle.Play();
    }

    public bool GetParticleSystemStatus()
    {
        status = particle.isPlaying;
        if (preStatus && !status)
        {
            particleFall = true;
        }
        else
        {
            particleFall = false;
        }
        preStatus = status;

        return particleFall;
    }
}

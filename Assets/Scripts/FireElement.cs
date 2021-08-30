using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElement : MonoBehaviour
{
    public ParticleSystem FireAdditive;
    public ParticleSystem FireAlphaBlend;

    public void PlayFire()
    {
        FireAdditive.Play();
        FireAlphaBlend.Play();
    }

    public void StopFire()
    {
        FireAdditive.Stop();
        FireAlphaBlend.Stop();
    }
}

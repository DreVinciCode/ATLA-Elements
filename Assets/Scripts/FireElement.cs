using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElement : MonoBehaviour
{
    private AudioSource[] allAudioSources;

    public ParticleSystem FireAdditive;
    public ParticleSystem FireAlphaBlend;

    private void Start()
    {
        HandTracking handEvents = FindObjectOfType<HandTracking>();
        handEvents.OnSnapDetected += HandEvents_OnSnapDetected;
    }

    private void HandEvents_OnSnapDetected(object sender, System.EventArgs e)
    {
        FireAdditive.Play();
        FireAlphaBlend.Play();
        SoundManager.PlaySound(SoundManager.Sound.FireStart);
        SoundManager.PlaySound(SoundManager.Sound.FireContinuous);

    }

    public void StopFire()
    {
        FireAdditive.Stop();
        FireAlphaBlend.Stop();
        SoundManager.StopSound();
    }

    public void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }
}

using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Dependencies")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _globalSfxSource;

    public void PlayGlobalSfx(AudioClip clip, float volume = 1, float pitch = 1, float pitchVariation = 0)
    {
        _globalSfxSource.volume = volume;
        _globalSfxSource.pitch = pitch + Random.Range(-pitchVariation * 0.5f, pitchVariation * 0.5f);
        _globalSfxSource.PlayOneShot(clip);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0168 // Variable is declared but never used
public class SoundManager : MonoBehaviour
{
    public List<AudioObject> MusicClips;
    public List<AudioObject> SFXClips;

    /// <summary>
    /// Whether or not sound is muted.
    /// </summary>
    public static bool ShouldPlaySound { get; private set; }
    /// <summary>
    /// Whether or not music is muted.
    /// </summary>
    public static bool ShouldPlayerMusic { get; private set; }

    private static SoundManager _instance;
    private static Dictionary<string, AudioObject> musicDict;
    private static Dictionary<string, AudioObject> sfxDict;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        musicDict = new Dictionary<string, AudioObject>();
        foreach (AudioObject audioObject in MusicClips)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioObject.AudioClip;
            audioSource.hideFlags = HideFlags.HideInInspector;
            audioObject.AudioSource = audioSource;
            musicDict[audioObject.Name] = audioObject;
        }
        sfxDict = new Dictionary<string, AudioObject>();
        foreach (AudioObject audioObject in SFXClips)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioObject.AudioClip;
            audioSource.hideFlags = HideFlags.HideInInspector;
            audioObject.AudioSource = audioSource;
            sfxDict[audioObject.Name] = audioObject;
        }

        ShouldPlaySound = PlayerPrefs.GetInt("PlaySound", 1) == 0 ? false : true;
        ShouldPlayerMusic = PlayerPrefs.GetInt("PlayMusic", 1) == 0 ? false : true;
    }

    /// <summary>
    /// Sets whether or not sound is muted. Stored in PlayerPrefs.
    /// </summary>
    /// <param name="shouldPlaySound">Sound muted or not</param>
    public static void SetSoundMuted(bool shouldPlaySound)
    {
        ShouldPlaySound = shouldPlaySound;
        PlayerPrefs.SetInt("PlaySound", shouldPlaySound ? 1 : 0);

        if (shouldPlaySound)
            return;

        foreach (AudioObject audioObject in _instance.SFXClips)
            audioObject.AudioSource.Stop();
    }

    /// <summary>
    /// Sets whether or not music is muted. Stored in PlayerPrefs.
    /// </summary>
    /// <param name="shouldPlayMusic">Music muted or not</param>
    public static void SetMusicMuted(bool shouldPlayMusic)
    {
        ShouldPlayerMusic = shouldPlayMusic;
        PlayerPrefs.SetInt("PlayMusic", shouldPlayMusic ? 1 : 0);

        if (shouldPlayMusic)
            return;

        foreach (AudioObject audioObject in _instance.MusicClips)
            audioObject.AudioSource.Stop();
    }

    /// <summary>
    /// Play a sound based on name and execute callback when done.
    /// </summary>
    /// <param name="name">Name of sound clip</param>
    /// <param name="callback">Callback action</param>
    /// <returns>AudioSource playing the sound</returns>
    public static AudioSource PlaySound(string name, Action callback = null)
    {
        if (ShouldPlaySound)
        {
            try
            {
                AudioObject ao = sfxDict[name];
                ao.AudioSource.Play();
                DelayUtil.WaitForSeconds(ao.AudioClip.length, callback);
                return ao.AudioSource;
            }
            catch (KeyNotFoundException knfEx)
            {
                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// Play music based on name and execute callback when done.
    /// </summary>
    /// <param name="name">Name of music clip</param>
    /// <param name="callback">Callback action</param>
    /// <returns>AudioSource playing the music</returns>
    public static AudioSource PlayMusic(string name, Action callback = null)
    {
        if (ShouldPlayerMusic)
        {
            try
            {
                AudioObject ao = musicDict[name];
                ao.AudioSource.Play();
                DelayUtil.WaitForSeconds(ao.AudioClip.length, callback);
                return ao.AudioSource;
            }
            catch (KeyNotFoundException knfEx)
            {
                return null;
            }
        }

        return null;
    }

    [Serializable]
    public class AudioObject
    {
        public string Name;
        public AudioClip AudioClip;
        [HideInInspector] public AudioSource AudioSource;
    }
}

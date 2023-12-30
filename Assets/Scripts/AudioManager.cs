using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ToggleSavePosition(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " is not found!");
            return;
        }

        s.isSaved = !s.isSaved;
        Debug.Log("Save position for sound " + name + ": " + s.isSaved);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " is not found!");
            return;
        }

        if (s.isSaved && s.savedTime > 0f && s.source.time != s.savedTime)
        {
            s.source.time = s.savedTime;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " is not found!");
            return;
        }

        // 再生位置を保存
        if (s.isSaved)
        {
            s.savedTime = s.source.time;
        }

        s.source.Stop();
    }

    // 全てのAudioの再生を止める
    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            Stop(s.name);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Play("bgm_" + scene.name);
    }

    public void ChangeBGMVolume(float newVolume)
    {
        foreach (Sound s in sounds)
        {
            if (s.type == Sound.AudioType.BGM)
            {
                s.volume = Mathf.Clamp01(newVolume);
                s.source.volume = s.volume;
            }
        }

        Debug.Log("Change BGM for volume: " + newVolume);
    }

    public void ChangeSEVolume(float newVolume)
    {
        foreach (Sound s in sounds)
        {
            if (s.type == Sound.AudioType.SE)
            {
                s.volume = Mathf.Clamp01(newVolume);
                s.source.volume = s.volume;
            }
        }

        Debug.Log("Change SE for volume: " + newVolume);
    }
}

// Sound class
[System.Serializable]
public class Sound
{
    public string name;
    public enum AudioType
    {
        BGM,
        SE
    }
    public AudioType type;
    public AudioClip clip;

    [Range(0, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    public bool loop;

    [Header("再生位置を記録")]
    public bool isSaved;
    public float savedTime;

}

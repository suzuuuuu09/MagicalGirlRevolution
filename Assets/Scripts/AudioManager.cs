using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Start()
    {
        Play("bgm");
    }

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

        // �Đ��ʒu��ۑ�
        if (s.isSaved)
        {
            s.savedTime = s.source.time;
        }

        s.source.Stop();
    }

    // �S�Ă�Audio�̍Đ����~�߂�
    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            Stop(s.name);
        }
    }
}

// Sound class
[System.Serializable]
public class Sound
{
    public string name;
    public enum audioType
    {
        BGM,
        SE
    }
    public audioType type;
    public AudioClip clip;

    [Range(0, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    public bool loop;

    [Header("�Đ��ʒu���L�^")]
    public bool isSaved;
    public float savedTime;

}

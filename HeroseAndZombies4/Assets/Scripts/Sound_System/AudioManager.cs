using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;
        [HideInInspector] public AudioSource source;
    }

    [SerializeField] private List<Sound> sounds = new List<Sound>();
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            soundDictionary.Add(s.name, s);
        }
    }

    public void Play(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public void Stop(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            sound.source.Stop();
        }
    }

    public void PlayAtPosition(string soundName, Vector3 position)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            AudioSource.PlayClipAtPoint(sound.clip, position, sound.volume);
        }
    }

    public void SetVolume(string soundName, float volume)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            sound.source.volume = Mathf.Clamp01(volume);
        }
    }

    public void FadeOut(string soundName, float duration)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            StartCoroutine(FadeAudio(sound.source, duration, 0f));
        }
    }

    private IEnumerator FadeAudio(AudioSource source, float duration, float targetVolume)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        source.volume = targetVolume;
        if (targetVolume <= 0) source.Stop();
    }
}
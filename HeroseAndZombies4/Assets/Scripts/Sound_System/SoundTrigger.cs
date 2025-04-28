using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string soundName;
    [SerializeField] private bool playOnAwake;
    [SerializeField] private bool playOnTriggerEnter;
    [SerializeField] private bool playOnCollisionEnter;
    [SerializeField] private bool playOnlyOnce;

    private bool hasPlayed;

    void Awake()
    {
        if (playOnAwake)
            PlaySound();
    }

    void OnTriggerEnter(Collider other)
    {
        if (playOnTriggerEnter && ShouldPlay(other.gameObject))
            PlaySound();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (playOnCollisionEnter && ShouldPlay(collision.gameObject))
            PlaySound();
    }

    bool ShouldPlay(GameObject other)
    {
        return !playOnlyOnce || (playOnlyOnce && !hasPlayed);
    }

    void PlaySound()
    {
        AudioManager.Instance.Play(soundName);
        hasPlayed = true;
    }
}
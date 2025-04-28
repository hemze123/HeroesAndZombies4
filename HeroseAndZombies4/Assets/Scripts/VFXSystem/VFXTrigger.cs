using UnityEngine;

public class VFXTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string effectName;
    [SerializeField] private bool playOnAwake;
    [SerializeField] private bool playOnTriggerEnter;
    [SerializeField] private bool playOnCollisionEnter;
    [SerializeField] private bool playOnlyOnce;
    [SerializeField] private Vector3 positionOffset;

    private bool hasPlayed;

    void Awake()
    {
        if (playOnAwake)
            PlayEffect();
    }

    void OnTriggerEnter(Collider other)
    {
        if (playOnTriggerEnter && ShouldPlay(other.gameObject))
            PlayEffect();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (playOnCollisionEnter && ShouldPlay(collision.gameObject))
            PlayEffect();
    }

    bool ShouldPlay(GameObject other)
    {
        return !playOnlyOnce || (playOnlyOnce && !hasPlayed);
    }

    void PlayEffect()
    {
        VFXManager.Instance.PlayEffect(effectName, transform.position + positionOffset);
        hasPlayed = true;
    }
}
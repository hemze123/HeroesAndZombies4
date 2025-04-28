using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    [System.Serializable]
    public class SurfaceSound
    {
        public LayerMask layer;
        public string soundName;
        public ParticleSystem effect;
    }

    [SerializeField] private SurfaceSound[] surfaces;
    [SerializeField] private float stepInterval = 0.4f;
    [SerializeField] private Transform raycastOrigin;

    private float lastStepTime;
    private RaycastHit hit;

    void Update()
    {
        if(Time.time - lastStepTime > stepInterval && IsMoving())
        {
            PlayFootstep();
            lastStepTime = Time.time;
        }
    }

    void PlayFootstep()
    {
        if(Physics.Raycast(raycastOrigin.position, Vector3.down, out hit, 1f))
        {
            SurfaceSound surface = System.Array.Find(surfaces, s => IsInLayerMask(hit.collider.gameObject, s.layer));
            if(surface != null)
            {
                AudioManager.Instance.Play(surface.soundName);
                if(surface.effect != null)
                    Instantiate(surface.effect, hit.point, Quaternion.identity);
            }
        }
    }

    bool IsMoving() => GetComponent<PlayerController>().IsMoving;
    bool IsInLayerMask(GameObject obj, LayerMask mask) => (mask & (1 << obj.layer)) != 0;
}
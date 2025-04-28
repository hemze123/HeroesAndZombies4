using UnityEngine;

public interface ICollectible
{
    void Collect();
    GameObject gameObject { get; } // Unity nesne erişimi için
}
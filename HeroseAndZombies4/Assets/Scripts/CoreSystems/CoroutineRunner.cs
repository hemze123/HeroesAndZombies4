using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CoroutineRunner");
                _instance = obj.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public new Coroutine StartCoroutine(IEnumerator routine)
    {
        return base.StartCoroutine(WrapCoroutine(routine));
    }

    private IEnumerator WrapCoroutine(IEnumerator routine)
    {
        yield return routine;
        
        if (transform.childCount == 0 && GetComponents<Component>().Length == 1)
        {
            Destroy(gameObject);
        }
    }
}
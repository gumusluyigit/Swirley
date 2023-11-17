using System.Collections;
using UnityEngine;
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning(typeof(T).ToString() + " is NULL.");
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"Duplicate instance of {typeof(T)} found. Destroying the new instance.");
            Destroy(gameObject);
        }
    }
    //public abstract IEnumerator Initialize();
}

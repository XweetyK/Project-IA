using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static Singleton<T> instance = null;

    public static T Instance {
        get{
            if (instance = null)
                instance = FindObjectOfType<Singleton<T>>();
            return (T)instance;
        }
    }

    protected virtual void Initialize(){

    }

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);

        instance = this;

        Initialize();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance;
    private static readonly object padlock = new object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GenerateInstance();
                    }
                }
            }
            return instance;
        }
    }

    private static void GenerateInstance()
    {
        GameObject singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
        instance = singletonObject.AddComponent<T>() as T;
        DontDestroyOnLoad(singletonObject);
    }

    protected virtual void Awake()
    {
        if(instance == null)
        {
            GenerateInstance();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// don't know what else could monobehaviour be other than component but
public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        // lazy instantiat, assign it only when needs to use it
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T> ();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if(instance == null)
        {
            // check type
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<T>();

                if (!_instance)
                {
                    Debug.LogError($"{typeof(T).Name} component doesn't exist.");
                }
            }

            return _instance;
        }
    }
}
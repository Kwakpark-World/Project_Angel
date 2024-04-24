using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoSingleton<ButtonManager>
{
    private static ButtonManager instance;

    public static ButtonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ButtonManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(ButtonManager).Name);
                    instance = obj.AddComponent<ButtonManager>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as ButtonManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Exit()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

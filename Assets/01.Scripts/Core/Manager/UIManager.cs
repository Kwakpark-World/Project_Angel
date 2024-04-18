using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    private SceneFade _fadePanel;

    private void Awake()
    {
        _fadePanel = FindObjectOfType<SceneFade>();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName + "Scene");
        _fadePanel.StartFadeOut(sceneName);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

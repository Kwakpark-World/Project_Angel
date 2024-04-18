using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private Image _fadePanel;
    [SerializeField]
    private float _fadeDuration;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += (scene, loadSceneMode) => OnSceneLoaded();
    }

    public void LoadScene(string sceneName)
    {
        _fadePanel.DOFade(1f, _fadeDuration)
            .OnStart(() =>
            {
                _fadePanel.raycastTarget = true;
            })
            .OnComplete(() =>
            {
                StartCoroutine(LoadSceneAsync(sceneName));
            });
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        float pastTime = 0f;
        float percentage = 0f;

        while (!async.isDone)
        {
            yield return null;

            pastTime += Time.deltaTime;

            if (percentage >= 90f)
            {
                percentage = Mathf.Lerp(percentage, 100, pastTime);

                if (percentage >= 100f)
                {
                    async.allowSceneActivation = true;
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, pastTime);

                if (percentage >= 90f)
                {
                    pastTime = 0f;
                }
            }
        }
    }

    private void OnSceneLoaded()
    {
        _fadePanel.DOFade(0f, _fadeDuration)
            .OnComplete(() =>
            {
                _fadePanel.raycastTarget = false;
            });
    }
}

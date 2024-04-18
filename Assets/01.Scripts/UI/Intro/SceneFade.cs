using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onFadeEnd;
    [SerializeField]
    private float _fadeDuration = 1f; // 페이드에 걸리는 시간

    private Image _fadePanel; // 이미지 컴포넌트
    private float _fadeTimer; // 타이머

    private void Start()
    {
        _fadePanel = GetComponent<Image>(); // 이미지 컴포넌트 가져오기

        DontDestroyOnLoad(transform.parent.gameObject);
        gameObject.SetActive(false);
    }

    public void StartFadeIn()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitUntil(() => gameObject.activeSelf);

        _fadeTimer = 0f;

        while (_fadeTimer < _fadeDuration)
        {
            _fadeTimer += Time.deltaTime;
            _fadePanel.color = new Color(_fadePanel.color.r, _fadePanel.color.g, _fadePanel.color.b, _fadeTimer / _fadeDuration);

            yield return null;
        }

        _onFadeEnd?.Invoke();
    }

    private IEnumerator FadeOut(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        _fadeTimer = _fadeDuration;

        while (_fadeTimer > 0f)
        {
            _fadeTimer -= Time.deltaTime;
            _fadePanel.color = new Color(_fadePanel.color.r, _fadePanel.color.g, _fadePanel.color.b, _fadeTimer / _fadeDuration);

            yield return null;
        }

        _onFadeEnd?.Invoke();
    }
}

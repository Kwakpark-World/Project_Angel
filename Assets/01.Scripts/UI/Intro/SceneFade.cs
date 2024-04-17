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
    private float _fadeDuration = 1f; // ���̵忡 �ɸ��� �ð�

    private Image _fadePanel; // �̹��� ������Ʈ
    private float _fadeTimer; // Ÿ�̸�

    private void Start()
    {
        _fadePanel = GetComponent<Image>(); // �̹��� ������Ʈ ��������

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

using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearManager : MonoBehaviour
{
    [SerializeField]
    private Image _fadePanel;
    [SerializeField]
    private float _fadeDuration;

    [Header("EndingText")]
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI mainText;
    [SerializeField]
    private RectTransform textContainer; 
    [SerializeField]
    private float scrollDuration = 11f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("2");
            ClearPanel();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("123");
            ClearPanel();
        }
    }

    public void ClearPanel()
    {
        _fadePanel.gameObject.SetActive(true);
        _fadePanel.DOFade(1f, _fadeDuration) 
          .OnStart(() =>
          {
              _fadePanel.raycastTarget = true;
          })
          .OnComplete(() =>
          {
              StartCoroutine(ScrollTextAndLoadScene());
          });
    }

    private IEnumerator ScrollTextAndLoadScene()
    {
        Vector3 startPosition = textContainer.anchoredPosition;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + Screen.height + textContainer.rect.height);

        textContainer.DOAnchorPosY(endPosition.y, scrollDuration).SetEase(Ease.Linear);

        yield return new WaitForSeconds(scrollDuration);

        _fadePanel.DOFade(1f, _fadeDuration)
          .OnStart(() =>
          {
              _fadePanel.raycastTarget = true;
          })
          .OnComplete(() =>
          {
              StartCoroutine(SceneLoad("IntroScene"));
          });
    }

    public IEnumerator SceneLoad(string sceneName)
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
}

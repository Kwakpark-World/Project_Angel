using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DieUI : MonoBehaviour
{
    [SerializeField]
    private Image _backgroundImage;
    [SerializeField]
    private TextMeshProUGUI _dieText;
    [SerializeField]
    private string _nextSceneName;

    public void OnDie()
    {
        _backgroundImage.gameObject.SetActive(true);

        Color backGroundColor = _backgroundImage.color;
        backGroundColor.a = 0f;
        _backgroundImage.color = backGroundColor;

        _backgroundImage.DOFade(0.7f, 1.5f);

        Color textColor = _dieText.color;
        textColor.a = 0f;
        _dieText.color = textColor;

        _dieText.DOFade(0.9f, 1.5f);

        StartCoroutine(ChangeScene(3.5f));
    }

    private IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        UIManager.Instance.LoadScene(_nextSceneName);
    }
}
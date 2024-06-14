using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image backGround;


    public void OnDie()
    {
        backGround.gameObject.SetActive(true);

        Color backGroundColor = backGround.color;
        backGroundColor.a = 0f;
        backGround.color = backGroundColor;

        backGround.DOFade(0.7f, 1.5f);

        Color textColor = text.color;
        textColor.a = 0f;
        text.color = textColor;

        text.DOFade(0.9f, 1.5f);

        StartCoroutine(ChangeScene(3.5f));
    }

    IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }
}

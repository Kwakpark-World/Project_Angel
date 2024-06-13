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

        Color textColor = text.color;
        textColor.a = 0f;
        text.color = textColor;

        text.DOFade(1f, 2f);

        StartCoroutine(ChangeScene(3f));
    }

    IEnumerator ChangeScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }
}

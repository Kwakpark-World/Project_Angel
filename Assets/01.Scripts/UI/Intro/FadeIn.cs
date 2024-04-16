using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{
    public float fadeInTime = 1f; // 페이드 인에 걸리는 시간

    private Image image; // 이미지 컴포넌트
    private float timer; // 타이머

    void Start()
    {
        image = GetComponent<Image>(); // 이미지 컴포넌트 가져오기
        timer = 0; // 타이머 초기화 (Fade In에서는 0부터 시작)
    }

    public void OnFadeIn()
    {
        gameObject.SetActive(true);
    }

    public void Update()
    {
        // 타이머 증가
        timer += Time.deltaTime;

        Color color = image.color;
        color.a = timer / fadeInTime; // 타이머가 증가함에 따라 알파값을 증가
        image.color = color;

        // 타이머가 fadeInTime 보다 커지면 스크립트 비활성화하여 더 이상 업데이트하지 않음
        if (timer >= fadeInTime)
        {
            
            SceneManager.LoadScene(2);
            enabled = false;

        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    public float fadeOutTime = 1f; // 페이드 아웃에 걸리는 시간

    private Image image; // 이미지 컴포넌트
    private float timer; // 타이머

    void Start()
    {
        image = GetComponent<Image>(); // 이미지 컴포넌트 가져오기
        timer = fadeOutTime; // 타이머 초기화
    }

    public void Update()
    {
        // 타이머 감소
        timer -= Time.deltaTime;

        // 이미지 알파 값을 감소시켜서 페이드 아웃 효과 생성
        Color color = image.color;
        color.a = timer / fadeOutTime; // 타이머가 감소함에 따라 알파값을 줄임
        image.color = color;

        // 타이머가 0보다 작아지면 오브젝트를 비활성화하여 완전히 사라지게 함
        if (timer <= 0)
        {
            gameObject.SetActive(false);
            
        }
    }
}

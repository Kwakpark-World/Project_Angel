using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{
    public float fadeInTime = 1f; // ���̵� �ο� �ɸ��� �ð�

    private Image image; // �̹��� ������Ʈ
    private float timer; // Ÿ�̸�

    void Start()
    {
        image = GetComponent<Image>(); // �̹��� ������Ʈ ��������
        timer = 0; // Ÿ�̸� �ʱ�ȭ (Fade In������ 0���� ����)
    }

    public void OnFadeIn()
    {
        gameObject.SetActive(true);
    }

    public void Update()
    {
        // Ÿ�̸� ����
        timer += Time.deltaTime;

        Color color = image.color;
        color.a = timer / fadeInTime; // Ÿ�̸Ӱ� �����Կ� ���� ���İ��� ����
        image.color = color;

        // Ÿ�̸Ӱ� fadeInTime ���� Ŀ���� ��ũ��Ʈ ��Ȱ��ȭ�Ͽ� �� �̻� ������Ʈ���� ����
        if (timer >= fadeInTime)
        {
            
            SceneManager.LoadScene(2);
            enabled = false;

        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    public float fadeOutTime = 1f; // ���̵� �ƿ��� �ɸ��� �ð�

    private Image image; // �̹��� ������Ʈ
    private float timer; // Ÿ�̸�

    void Start()
    {
        image = GetComponent<Image>(); // �̹��� ������Ʈ ��������
        timer = fadeOutTime; // Ÿ�̸� �ʱ�ȭ
    }

    public void Update()
    {
        // Ÿ�̸� ����
        timer -= Time.deltaTime;

        // �̹��� ���� ���� ���ҽ��Ѽ� ���̵� �ƿ� ȿ�� ����
        Color color = image.color;
        color.a = timer / fadeOutTime; // Ÿ�̸Ӱ� �����Կ� ���� ���İ��� ����
        image.color = color;

        // Ÿ�̸Ӱ� 0���� �۾����� ������Ʈ�� ��Ȱ��ȭ�Ͽ� ������ ������� ��
        if (timer <= 0)
        {
            gameObject.SetActive(false);
            
        }
    }
}

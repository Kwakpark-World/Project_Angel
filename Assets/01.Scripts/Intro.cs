using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
=======
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class Intro : MonoBehaviour
{
    public TimelineAsset timeline;
    private PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        // ����ڰ� Space Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTimeline();
        }
    }

    public void PlayTimeline()
    {
        // Ÿ�Ӷ����� �����Ǿ� ���� ������ �������� ����
        if (timeline == null)
        {
            Debug.LogWarning("Timeline is not assigned!");
            return;
        }

        // PlayableDirector�� Ÿ�Ӷ��� ����
        director.playableAsset = timeline;

        // Ÿ�Ӷ��� ���
        director.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // �÷��̾�� �浹���� �� ���� �ִ� ������ ���� ��(�ε��� 2)���� �̵�
            SceneManager.LoadScene(2);
        }
>>>>>>> Stashed changes
    }
}

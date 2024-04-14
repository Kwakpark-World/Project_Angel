using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // 사용자가 Space 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTimeline();
        }
    }

    public void PlayTimeline()
    {
        // 타임라인이 설정되어 있지 않으면 실행하지 않음
        if (timeline == null)
        {
            Debug.LogWarning("Timeline is not assigned!");
            return;
        }

        // PlayableDirector에 타임라인 설정
        director.playableAsset = timeline;

        // 타임라인 재생
        director.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 플레이어와 충돌했을 때 원래 있던 씬에서 다음 씬(인덱스 2)으로 이동
            SceneManager.LoadScene(2);
        }
    }
}

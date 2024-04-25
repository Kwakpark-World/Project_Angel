using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ESCPage : MonoBehaviour
{
    public GameObject _escPanel;

    public GameObject _controlPanel;

    private bool isPanel = false;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isPanel == false)
        {
                isPanel = true;
                _escPanel.transform.DOMoveY(550, 1);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPanel == true)
        {

            _controlPanel.SetActive(false);
            UIManager.Instance._volumeSetting.SetActive(false);
            AnmationPanel();
            isPanel = false;
        }
    }

    public void ContinueGame()
    {
        AnmationPanel();
    }

    public void ReturnGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void Control()
    {
        _controlPanel.SetActive(true);
    }

    public void AnmationPanel()
    {
            _escPanel.transform.DOMoveY(380, 0.8f).OnComplete(() =>
            {
                _escPanel.transform.DOMoveY(1500, 1);
            });
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCPage : MonoBehaviour
{
    public GameObject _escPanel;

    public GameObject _controlPanel;

    public GameObject _settingPanel;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _escPanel.SetActive(false);
            _controlPanel.SetActive(false);
            _settingPanel.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        _escPanel.SetActive(false);
    }

    public void ReturnGame()
    {
        _escPanel.SetActive(false);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void Control()
    {
        _escPanel.SetActive(false);
        _controlPanel.SetActive(true);
    }

    public void Setting()
    {
        _escPanel.SetActive(false);
        _settingPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private Player _player;
    public Player PlayerInstance
    {
        get
        {
            if (!_player)
            {
                if (HasPlayer)
                {
                    Debug.LogError("Player instance doesn't exist.");
                }
            }

            return _player;
        }
    }
    public bool HasPlayer
    {
        get
        {
            return _player = FindObjectOfType<Player>();
        }

        private set
        {

        }
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (HasPlayer)
        {

        }
    }
}

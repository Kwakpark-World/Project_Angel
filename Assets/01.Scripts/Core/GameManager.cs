using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private Player _player;
    public Player PlayerInstance
    {
        get
        {
            if (!_player)
            {
                _player = FindObjectOfType<Player>();

                if (!_player)
                {
                    Debug.LogError("Player instance doesn't exist.");
                }
            }

            return _player;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

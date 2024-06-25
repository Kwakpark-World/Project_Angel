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

    private void Start()
    {
        SoundManager.Instance.PlayEnv(ENVType.Wind);
    }

    // 커서 키면 보이고 락풀리고, 커서 끄면 안보이고 락걸리고.
    public void SetCursor(bool isOnCursor)
    {
        Cursor.visible = isOnCursor;
        Cursor.lockState = isOnCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}

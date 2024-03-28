using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            if (_instance == null)
            {
                Debug.LogError("GameManager Component is null");
            }
            else if (!_instance.player)
            {
                _instance.player = FindObjectOfType<Player>();

                if (_instance.player)
                {
                    _instance.playerTransform = _instance.player.transform;
                }
            }

            return _instance;
        }
    }

    [HideInInspector]
    public Player player;
    [HideInInspector]
    public Transform playerTransform;

}

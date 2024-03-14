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

    public Camera MainCam { get; private set; }

    private void Start()
    {
        MainCam = Camera.main;

        if (MainCam == null)
        {
            Debug.LogError("There is no camera in this scene. Check if the main camera tag exists or if the camera exists in the scene.\r\n");
        }
    }
}

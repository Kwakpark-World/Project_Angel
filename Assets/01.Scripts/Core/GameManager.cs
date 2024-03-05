using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public Player player;
    public PoolManager pool;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            if (_instance == null)
                Debug.LogError("CameraManager Component is null");

            return _instance;
        }
    }

    public Camera MainCam { get; private set; }

    private void Start()
    {
        MainCam = Camera.main;
        if (MainCam == null)
            Debug.LogError("There is no camera in this scene. Check if the main camera tag exists or if the camera exists in the scene.\r\n");
    }
}

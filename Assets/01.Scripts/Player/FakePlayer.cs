using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    [Header("Awaken Parameters")]
    [SerializeField] protected GameObject _defaultVisual;
    [SerializeField] protected GameObject _awakenVisual;

    private void Start()
    {
        SetPlayerModelAndAnim();
    }

    public void SetPlayerModelAndAnim()
    {
        _defaultVisual.SetActive(!GameManager.Instance.player.IsAwakening);
        _awakenVisual.SetActive(GameManager.Instance.player.IsAwakening);
    }
}

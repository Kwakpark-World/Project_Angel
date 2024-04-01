using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    [Header("Awaken Parameters")]
    [SerializeField] protected GameObject _defaultVisual;
    [SerializeField] protected GameObject _awakenVisual;

    private void Update()
    {
        SetPlayerModelAndAnim();
    }

    public void SetPlayerModelAndAnim()
    {
        if(GameManager.Instance.player.IsAwakening == false)
        {
            _defaultVisual.SetActive(true);
            _awakenVisual.SetActive(false);
        }
        else
        {
            _defaultVisual.SetActive(false);
            _awakenVisual.SetActive(true);
        }
        
    }
}

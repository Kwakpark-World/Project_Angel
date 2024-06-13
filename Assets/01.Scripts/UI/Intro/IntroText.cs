using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class IntroText : MonoBehaviour
{
    public TextMeshProUGUI introText;

    public void Start()
    {
        introText.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);    
    }
}

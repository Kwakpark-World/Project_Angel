using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class WaveFont : MonoBehaviour
{
    public TextMeshProUGUI _waveFont;
    public Image _image;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    public void WavePrint()
    {
        originalPosition = transform.position;
        MoveToNewPosition("wave " + GameManager.Instance.SpawnWave);
    }

    void MoveToNewPosition(string waveText)
    {
        _waveFont.text = waveText;

        transform.DOMove(new Vector3(950, 800, 1), 1.5f).OnComplete(MoveBackToOriginalPosition);

        _image.rectTransform.DOMove(new Vector3(950, 800, 1), 1.5f);
    }

    void MoveBackToOriginalPosition()
    {
        transform.DOMove(originalPosition, 1.5f).SetDelay(1);

        _image.rectTransform.DOMove(originalPosition, 1.5f).SetDelay(1);
    }
}

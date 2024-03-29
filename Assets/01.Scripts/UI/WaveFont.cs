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
    public EnemySpawn enemySpawn;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    // Start is called before the first frame update
    public void WavePrint()
    {
        MoveToNewPosition("wave " + enemySpawn.SpawnWave);
    }

    void MoveToNewPosition(string waveText)
    {
        _waveFont.text = waveText;

        _image.rectTransform.DOMove(new Vector3(950, 800, 1), 1.5f).OnComplete(MoveBackToOriginalPosition);
    }

    void MoveBackToOriginalPosition()
    {
        _image.rectTransform.DOMove(originalPosition, 1.5f).SetDelay(1);
    }
}

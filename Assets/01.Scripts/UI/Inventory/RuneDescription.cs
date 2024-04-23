using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuneDescription : MonoBehaviour
{
    public RuneInventory runeInventory;

    public List<Image> _displayImage;
    public List<Image> _runeLine;
    public Image _targetImage;

    public GameObject _Inventory;
    public RuneDataSO runeDataSO;

    private Dictionary<BuffType, Sprite> itemSprites;
    //private Dictionary<RuneDataSO, string> targetSprites;

    public TextMeshProUGUI _runeName;

    void Start()
    {
        itemSprites = runeInventory.itemSprites;
        _Inventory.SetActive(false);
       // _targetImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateDisplay();
        InventoryStat();
        //SynergisticActivity();
    }

    void InventoryStat()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _Inventory.SetActive(!_Inventory.activeSelf); 
        }
    }

    void UpdateDisplay()
    {
        foreach (var image in _displayImage)
        {
            image.gameObject.SetActive(false);
        }

        int index = 0;
        foreach (var kvp in itemSprites)
        {
            _displayImage[index].sprite = kvp.Value;
            // 이미지를 할당한 후 displayImage를 활성화
            _displayImage[index].gameObject.SetActive(true);
            index++;
        }

        // 나머지 displayImage에 null을 할당하고 활성화
        for (int i = index; i < _displayImage.Count; i++)
        {
            _displayImage[i].sprite = null;
        }
    }

    /*void SynergisticActivity()
    {
        foreach (var kvp in itemSprites)
        {
            runeTypeCounts[kvp.Key] = 0;
        }

        // 현재 보유한 룬 개수를 세기
        foreach (var kvp in itemSprites)
        {
            runeTypeCounts[kvp.Key]++;
        }

        // 시너지가 충족된 룬 타입을 찾아 이미지 색상 변경
        foreach (var kvp in runeTypeCounts)
        {
            if (kvp.Value >= 3)
            {
                ChangeImageColorByRuneType(kvp.Key);
            }
        }
    }*/

    void ChangeImageColorByRuneType(RuneType runeType)
    {
        // 변경할 이미지 색상 설정
        Color newColor = Color.red; // 예시로 빨간색 사용, 원하는 색상으로 변경 가능

        // 이미지 색상 변경
        foreach (var image in _runeLine)
        {
            if (image.sprite != null) // 이미지의 RuneType이 시너지를 만족하는 경우에만 색상 변경
            {
                image.color = newColor;
            }
        }
    }

    public void CopyImage(int index)
    {
        if (index >= 0 && index < _displayImage.Count)
        {
            Image sourceImage = _displayImage[index];
            _targetImage.gameObject.SetActive(true);

            if (sourceImage != null && _targetImage != null)
            {
                _targetImage.sprite = sourceImage.sprite;
                _runeName.text = runeDataSO.runeName;
            }
        }
    }
}

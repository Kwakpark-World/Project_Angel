using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventroyUI : MonoBehaviour
{
    public RuneInventory runeInventory;
    public Image displayImage;

    private Dictionary<RuneType, Sprite> itemSprites;

    void Start()
    {
        itemSprites = runeInventory.itemSprites;
        
    }

    private void Update()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        Debug.Log("2");
        // 현재 UI Image에 아이템 이미지 설정하기
        foreach (var kvp in itemSprites)
        {
            displayImage.sprite = kvp.Value;
            Debug.Log(kvp.Value);
        }
    }
}

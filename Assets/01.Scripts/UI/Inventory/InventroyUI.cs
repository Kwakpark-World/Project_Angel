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
        Debug.Log("5");
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // ���� UI Image�� ������ �̹��� �����ϱ�
        foreach (var kvp in itemSprites)
        {
            displayImage.sprite = kvp.Value;
            Debug.Log(kvp.Value);
        }
    }
}

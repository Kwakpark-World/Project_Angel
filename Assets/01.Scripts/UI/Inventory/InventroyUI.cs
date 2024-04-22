using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventroyUI : MonoBehaviour
{
    public RuneInventory runeInventory;
    public Image displayImage;
    public GameObject _Inventory;

    private Dictionary<RuneType, Sprite> itemSprites;

    void Start()
    {
        itemSprites = runeInventory.itemSprites;
        _Inventory.SetActive(false);
    }

    private void Update()
    {
        UpdateDisplay();
        InventoryStat();
    }

    void InventoryStat()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _Inventory.SetActive(!_Inventory.activeSelf); // Toggle the Inventory GameObject's active state
        }
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

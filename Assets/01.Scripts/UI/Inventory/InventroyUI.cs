using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventroyUI : MonoBehaviour
{
    public RuneInventory runeInventory;

    public List<Image> _displayImage;
    public List<Image> _runeLine;
    public Image _targetImage;

    public GameObject _Inventory;
    

    private Dictionary<RuneType, Sprite> itemSprites;
    private Dictionary<RuneType, int> runeTypeCounts;

    void Start()
    {
        itemSprites = runeInventory.itemSprites;
        _Inventory.SetActive(false);
        _targetImage.gameObject.SetActive(false);
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
            // �̹����� �Ҵ��� �� displayImage�� Ȱ��ȭ
            _displayImage[index].gameObject.SetActive(true);
            index++;
        }

        // ������ displayImage�� null�� �Ҵ��ϰ� Ȱ��ȭ
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

        // ���� ������ �� ������ ����
        foreach (var kvp in itemSprites)
        {
            runeTypeCounts[kvp.Key]++;
        }

        // �ó����� ������ �� Ÿ���� ã�� �̹��� ���� ����
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
        // ������ �̹��� ���� ����
        Color newColor = Color.red; // ���÷� ������ ���, ���ϴ� �������� ���� ����

        // �̹��� ���� ����
        foreach (var image in _runeLine)
        {
            if (image.sprite != null) // �̹����� RuneType�� �ó����� �����ϴ� ��쿡�� ���� ����
            {
                image.color = newColor;
            }
        }
    }

    public void CopyImage(int index)
    {

        // index�� ��ȿ�� �ε������� Ȯ��
        if (index >= 0 && index < _displayImage.Count)
        {
            Image sourceImage = _displayImage[index];
            _targetImage.gameObject.SetActive(true);

            if (sourceImage != null && _targetImage != null)
            {
                _targetImage.sprite = sourceImage.sprite;
            }
        }


    }
}

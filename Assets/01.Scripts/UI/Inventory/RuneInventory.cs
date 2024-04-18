using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneInventory : MonoSingleton<RuneInventory>
{
    public Dictionary<RuneType, Sprite> itemSprites; // ������ ��������Ʈ ��ųʸ�

    protected override void Awake()
    {
        base.Awake();
        itemSprites = new Dictionary<RuneType, Sprite>();
    }

    // �������� �߰��ϴ� �޼ҵ�
    public void AddItem(RuneType runeType, Sprite itemSprite)
    {
        if (!itemSprites.ContainsKey(runeType))
        {
            itemSprites.Add(runeType, itemSprite);
            
        }
    }

    // �������� �����ϴ� �޼ҵ�
    public void RemoveItem(RuneType runeType)
    {
        if (itemSprites.ContainsKey(runeType))
        {
            itemSprites.Remove(runeType);
        }
    }
}

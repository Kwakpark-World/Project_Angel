using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneInventory : MonoSingleton<RuneInventory>
{
    public Dictionary<BuffType, Sprite> itemSprites; // ������ ��������Ʈ ��ųʸ�


    protected override void Awake()
    {
        base.Awake();

        itemSprites = new Dictionary<BuffType, Sprite>();
    }

    // �������� �߰��ϴ� �޼ҵ�
    public void AddItem(BuffType runeType, Sprite itemSprite)
    {
        if (!itemSprites.ContainsKey(runeType))
        {
            itemSprites.Add(runeType, itemSprite);
        }
    }

    // �������� �����ϴ� �޼ҵ�
    public void RemoveItem(BuffType runeType)
    {
        if (itemSprites.ContainsKey(runeType))
        {
            itemSprites.Remove(runeType);
        }
    }
}

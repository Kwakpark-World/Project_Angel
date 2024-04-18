using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneInventory : MonoSingleton<RuneInventory>
{
    public Dictionary<RuneType, Sprite> itemSprites; // 아이템 스프라이트 딕셔너리

    protected override void Awake()
    {
        base.Awake();
        itemSprites = new Dictionary<RuneType, Sprite>();
    }

    // 아이템을 추가하는 메소드
    public void AddItem(RuneType runeType, Sprite itemSprite)
    {
        if (!itemSprites.ContainsKey(runeType))
        {
            itemSprites.Add(runeType, itemSprite);
            
        }
    }

    // 아이템을 제거하는 메소드
    public void RemoveItem(RuneType runeType)
    {
        if (itemSprites.ContainsKey(runeType))
        {
            itemSprites.Remove(runeType);
        }
    }
}

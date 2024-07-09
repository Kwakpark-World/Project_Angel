using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerESkillEffect : PoolableMonoEffect
{
    float time;
    bool isDownEffect;

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        isDownEffect = false;

        time = 0;
        transform.localScale = Vector3.zero;

        StartCoroutine(DoScale());
    }
    
    protected override void Update()
    {
        base.Update();
        time += Time.deltaTime;
        if (time > 2.3f && !isDownEffect)
        {
            isDownEffect = true;

            PoolManager.Instance.Push(this);

            //Vector3 pos = transform.position;
            //EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAwakened, pos);

            GameManager.Instance.PlayerInstance.IsAwakened = true;
        }
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }

    private IEnumerator DoScale()
    {
        while (transform.localScale.y < 1)
        {
            transform.localScale += Vector3.one * 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
}

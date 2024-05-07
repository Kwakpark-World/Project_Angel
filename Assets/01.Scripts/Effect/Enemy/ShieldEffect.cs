using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ShieldEffect : PoolableMonoEffect
{
    float time;
    bool isDownEffect;

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        PoolManager.Instance.Push(this, duration, true);
/*        EnemyBrain enemy = new EnemyBrain();
        Vector3 pos = enemy.transform.position;
        EffectManager.Instance.PlayEffect(PoolingType.Effect_Shield, pos);*/

        StartCoroutine(DoScale());
    }

    protected override void Update()
    {
        
        base.Update();
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }

    private Transform _shieldTransform;

/*    IEnumerator Shield(float duration)
    {
        EnemyBrain enemyBrain = new EnemyBrain();
        Vector3 enemyPos = enemyBrain.transform.position;

        Vector3 dirction = enemyPos - _shieldTransform.position;
        dirction.Normalize();
        yield return new WaitForSeconds(duration);
    }*/

    private IEnumerator DoScale()
    {
        //StartCoroutine(Shield(3f));
        while (transform.localScale.y < 1)
        {
            transform.localScale += Vector3.one * 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
}

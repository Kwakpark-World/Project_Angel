using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderSkillAttack : EnemyAttack
{
    private bool _canSkillPlay;
    private bool _isSkillPlaying;

    private Transform m_transform;
    public LayerMask enemyLayer;
    private int maxEnemiesToShield = 5;

    public override void OnStart()
    {
        _canSkillPlay = true;
    }

    // Start is called before the first frame update
    void Update()
    {
        ApplyShield();
    }

    void ApplyShield()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.forward, 5, enemyLayer);

        int enemiesShielded = 0;
        foreach (RaycastHit hit in hits)
        {
            OwnerNode.brain.AnimatorCompo.SetAnimationState("SkillAttack");
            if (enemiesShielded >= maxEnemiesToShield)
                break;

            GameObject enemy = hit.collider.gameObject;
            Debug.Log(enemy);
            
        }
        OwnerNode.brain.AnimatorCompo.SetAnimationState("Idle");
    }


    public override void OnStop()
    {
        OwnerNode.brain.AnimatorCompo.OnAnimationEnd("");
    }

    public override Node.State OnUpdate()
    {
        return Node.State.Success;
    }
}

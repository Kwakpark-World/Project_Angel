using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;

public class PlayerChargeAttackEffect : PoolableMonoEffect
{
    float time;
    ParticleSystem particle;
    BoxCollider boxCol;

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        if (GameManager.Instance.player.IsAwakening)
        {
            PoolManager.Instance.Push(this);
            return;
        }
            

        Quaternion rot = GameManager.Instance.player.transform.rotation;
        rot.x = 0;
        rot.z = 0;

        transform.rotation = rot;

        particle = GetComponentInChildren<ParticleSystem>();
        boxCol = GetComponent<BoxCollider>();

        boxCol.enabled = false;

        time = 0;
        transform.localScale = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();
        

        if (GameManager.Instance.player.PlayerInput.isCharge)
        {
            if (time > 0.3f)
                particle.Pause();
            else
                time += Time.deltaTime;

            if (GameManager.Instance.player.ChargingGage > 0.5f)
            {
                transform.localScale = Vector3.one * GameManager.Instance.player.ChargingGage * 0.5f;
            }
        }
        else
        {
            if (GameManager.Instance.player.ChargingGage < 0.5f)
            {
                PoolManager.Instance.Push(this);
            }

            if (time < 0.3f) return;
            time = 0;
            
            particle.Play();
            boxCol.enabled = true;

            PoolManager.Instance.Push(this, 3);
        }

        if (GameManager.Instance.player.StateMachine.CurrentState == GameManager.Instance.player.StateMachine.GetState(PlayerStateEnum.Dash))
        {
            PoolManager.Instance.Push(this);
        }
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            if(other.gameObject.TryGetComponent<Brain>(out Brain brain))
            {
                brain.OnHit(GameManager.Instance.player.attackPower);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public ParticleSystem _hitParticle;

    public void RotatonEffect()
    {
        _hitParticle.transform.rotation = GameManager.Instance.PlayerInstance.weapon.transform.rotation;
    }
}

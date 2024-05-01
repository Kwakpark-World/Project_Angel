using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerAttackRangeEditor : MonoBehaviour
{
    public float value;
    public Vector3 size;
    public Vector3 offset;

    #region Toggle Boolean
    public bool MeleeAttack_Normal = false;
    public bool ChargeAttack_Normal = false;
    public bool ChargeAttack_Stab_Normal = false;
    #endregion

    private Player Player;
    private Transform WeaponRT;
    private Transform WeaponRB;
    private Transform WeaponLT;
    private Transform WeaponLB;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Player == null) Player = GetComponent<Player>();
        if (Player._weapon == null) Player._weapon = GameObject.FindGameObjectWithTag("Weapon");
        if (WeaponRT == null) WeaponRT = Player._weapon.transform.Find("RightPointTop");
        if (WeaponRB == null) WeaponRB = Player._weapon.transform.Find("RightPointBottom");
        if (WeaponLT == null) WeaponLT = Player._weapon.transform.Find("LeftPointTop");
        if (WeaponLB == null) WeaponLB = Player._weapon.transform.Find("LeftPointBottom");

        Gizmos.color = Color.red;

        if (MeleeAttack_Normal)
            MeleeAttack();

        if (ChargeAttack_Normal)
            ChargeAttackNormal();
        if (ChargeAttack_Stab_Normal)
            ChargeAttackStabNormal();
    }
#endif
    private void MeleeAttack()
    {
        Vector3 dir = (WeaponRT.position - WeaponRB.position).normalized;

        Vector3 weaponRPos = WeaponRB.position;
        Vector3 weaponLPos = WeaponLB.position;

        Gizmos.DrawRay(weaponRPos, dir * value);
        Gizmos.DrawRay(weaponLPos, dir * value);
    }

    private void ChargeAttackNormal()
    {
        Gizmos.DrawCube(WeaponRT.transform.position + offset, size);
    }

    private void ChargeAttackStabNormal()
    {
        Gizmos.DrawCube(WeaponRT.transform.position + offset, size);
    }
}

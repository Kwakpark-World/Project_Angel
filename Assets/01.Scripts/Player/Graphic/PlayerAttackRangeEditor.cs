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
    public bool AttackRange_Weapon = false;
    public bool AttackRange_Player = false;
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

        if (AttackRange_Weapon)
            AttackRangeWeapon();
        if (AttackRange_Player)
            AttackRangePlayer();
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

    private void AttackRangeWeapon()
    {
        Gizmos.DrawRay(Player._weapon.transform.position, Player._weapon.transform.up * value);

        Gizmos.matrix = Matrix4x4.TRS(Player._weapon.transform.TransformPoint(Player._weapon.transform.position + offset), Player._weapon.transform.rotation, Player._weapon.transform.lossyScale);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(-Player._weapon.transform.position + offset, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(-Player._weapon.transform.position + offset, size);
    }

    private void AttackRangePlayer()
    {
        Gizmos.matrix = Matrix4x4.TRS(Player.transform.TransformPoint(Player.transform.position + offset), Player.transform.rotation, Player.transform.lossyScale);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(-Player.transform.position + offset, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(-Player.transform.position + offset, size);

        //Gizmos.DrawCube(Player.transform.position + offset, size);
    }
}

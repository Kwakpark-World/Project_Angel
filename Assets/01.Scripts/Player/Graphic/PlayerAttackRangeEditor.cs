using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class PlayerAttackRangeEditor : MonoBehaviour
{
    public float value;
    public Vector3 size;
    public Vector3 offset;

    [Header("Rot Params")]
    public Vector3 pos;
    public Vector3 rotation;

    #region Toggle Boolean
    public bool MeleeAttack_Normal = false;
    public bool AttackRange_Weapon = false;
    public bool AttackRange_Player = false;
    public bool AttackRange_Rotation = false;
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
        if (Player.weapon == null) Player.weapon = GameObject.FindGameObjectWithTag("Weapon");
        if (WeaponRT == null) WeaponRT = Player.weapon.transform.Find("RightPointTop");
        if (WeaponRB == null) WeaponRB = Player.weapon.transform.Find("RightPointBottom");
        if (WeaponLT == null) WeaponLT = Player.weapon.transform.Find("LeftPointTop");
        if (WeaponLB == null) WeaponLB = Player.weapon.transform.Find("LeftPointBottom");

        Vector3 weaponLBPos = WeaponLB.position;
        Vector3 weaponRTPos = WeaponRT.position;
        Vector3 averageWeaponVector = weaponLBPos - weaponRTPos;

        float averageWeaponAngle = Mathf.Atan2(averageWeaponVector.y, averageWeaponVector.x) * Mathf.Rad2Deg;

 
        GameManager.Instance.PlayerInstance.WeaponAngel = Quaternion.Euler(0, 0, averageWeaponAngle);

        Gizmos.color = Color.red;

        if (MeleeAttack_Normal)
            MeleeAttack();

        if (AttackRange_Weapon)
            AttackRangeWeapon();
        if (AttackRange_Player)
            AttackRangePlayer();
        if (AttackRange_Rotation)
            AttackRangeRotation();
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
        Gizmos.DrawRay(Player.weapon.transform.position, Player.weapon.transform.up * value);

        Gizmos.matrix = Matrix4x4.TRS(Player.weapon.transform.TransformPoint(Player.weapon.transform.position + offset), Player.weapon.transform.rotation, Player.weapon.transform.lossyScale);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(-Player.weapon.transform.position + offset, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(-Player.weapon.transform.position + offset, size);
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

    private void AttackRangeRotation()
    {
        Gizmos.matrix = Matrix4x4.TRS(pos + offset, Quaternion.Euler(rotation), size);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(pos + offset, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos + offset, size);
    }
}

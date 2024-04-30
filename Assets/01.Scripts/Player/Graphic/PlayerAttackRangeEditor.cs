using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerAttackRangeEditor : MonoBehaviour
{
    public Vector3 size;
    public Vector3 offset;

    #region Toggle Boolean
    public bool ChargeAttack_Normal = false;
    public bool ChargeAttack_Stab_Normal = false;
    #endregion

    private Player Player;
    private Transform WeaponRT;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Player == null) Player = GetComponent<Player>();
        if (Player._weapon == null) Player._weapon = GameObject.FindGameObjectWithTag("Weapon");
        if (WeaponRT == null) WeaponRT = Player._weapon.transform.Find("RightPointTop");

        Gizmos.color = Color.red;


        if (ChargeAttack_Normal)
            ChargeAttackNormal();
        if (ChargeAttack_Stab_Normal)
            ChargeAttackStabNormal();
    }
#endif

    private void ChargeAttackNormal()
    {
        Gizmos.DrawCube(WeaponRT.transform.position + offset, size);
    }

    private void ChargeAttackStabNormal()
    {
        Gizmos.DrawCube(WeaponRT.transform.position + offset, size);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/RuneEffect/UpgradeSkill")]
public class UpgradeSkillEffectSO : RuneEffectSO
{
    public PlayerStateEnum targetSkill;

    [HideInInspector] public int selectedMethodIndex;
    [HideInInspector] public int selectedKillMethodIndex;

    [HideInInspector] public string paramStr;

    public List<MethodInfo> methodList = new List<MethodInfo>();
    public List<MethodInfo> killMethodList = new List<MethodInfo>();

    public object[] paramArr;

    public override void UseEffect()
    {
        MethodInfo m = killMethodList[selectedKillMethodIndex];
        PlayerState playerState = GameManager.Instance.player.StateMachine.GetState(targetSkill);
        m.Invoke(playerState, null);
    }

    public override void KillEffect()
    {
        MethodInfo m = methodList[selectedMethodIndex];
        PlayerState playerState = GameManager.Instance.player.StateMachine.GetState(targetSkill);
        m.Invoke(playerState, paramArr);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (targetSkill != 0)
        {
            Type t = Type.GetType($"Player{targetSkill}State");

            MethodInfo[] methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            methodList.Clear();
            killMethodList.Clear();

            paramArr = paramStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => GetData(x))
                .ToArray();

            foreach (MethodInfo m in methods)
            {
                if (m.Name.StartsWith("Upgrade"))
                {
                    methodList.Add(m);
                }
                else if(m.Name.StartsWith("Kill"))
                {
                    killMethodList.Add(m);
                }
            }
        }
    }

    private object GetData(string strInput)
    {
        object data;
        if (strInput.StartsWith("\""))
            data = strInput.Trim('\"');
        else if (bool.TryParse(strInput, out bool bTemp))
            data = bTemp;
        else if (int.TryParse(strInput, out int iTemp))
            data = iTemp;
        else if (double.TryParse(strInput, out double dTemp))
            data = dTemp;
        else
            data = strInput;

        return data;
    }


#endif
}

using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/Rune/StatUpgrade")]
public class StatUpgradeSO : RuneDataSO
{
    public PlayerStatType increaseStat;
    public int increaseValue;

    private Stat _stat;

    public override void UseEffect()
    {
        _stat = GameManager.Instance.PlayerInstance.PlayerStatData.GetStatByType(increaseStat);
        if(_stat == null)
        {
            Debug.LogError($"PlayerStat Dosen't have {increaseStat.ToString()} Stat.");
            return;
        }
        _stat.AddModifier(increaseValue);
    }

    public override void KillEffect()
    {
        if (_stat == null)
        {
            Debug.LogError($"PlayerStat Dosen't have {increaseStat.ToString()} Stat.");
            return;
        }
        _stat.RemoveModifier(increaseValue);
    }
}
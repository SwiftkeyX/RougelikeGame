using UnityEngine;

public enum ENEMYATTACKTYPE
{
    LIGHT = 0,
    HEAVY = 1,
    SPECIAL = 2,
    GAPCLOSER = 3,
    RANGE = 4
}

[System.Serializable]
public class EnemyAttackType
{
    public float damage;
    public float maxCooldown;
    public float range;
    public AttackData attackData;
}

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "SO/Enemy/EnemyAttackData")]
public class EnemyAttackData : ScriptableObject
{
    [SerializeField] private EnemyAttackType Light;
    [SerializeField] private EnemyAttackType Heavy;
    [SerializeField] private EnemyAttackType Special;
    [SerializeField] private EnemyAttackType GapCloser;
    [SerializeField] private EnemyAttackType Range;

    public EnemyAttackType Get(ENEMYATTACKTYPE type)
    {
        if (type == ENEMYATTACKTYPE.LIGHT)
        {
            return Light;
        }
        else if (type == ENEMYATTACKTYPE.HEAVY)
        {
            return Heavy;
        }
        else if (type == ENEMYATTACKTYPE.SPECIAL)
        {
            return Special;
        }
        else if (type == ENEMYATTACKTYPE.GAPCLOSER)
        {
            return GapCloser;
        }
        else if (type == ENEMYATTACKTYPE.RANGE)
        {
            return Range;
        }

        return null;
    }
}
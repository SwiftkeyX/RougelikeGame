
/// <summary>
/// weight is EnemyAttackWeight 
/// it's use in logic to random enemy's attack when enemy decide to attack
/// this is too add unpredictable to enemy's AI
/// </summary>
public class Weight
{
    public float LIGHT;
    public float HEAVY;
    public float SPECIAL;
    public float GAPCLOSER;
    public float RANGE;

    public Weight(
        float light,
        float heavy,
        float special,
        float gapcloser,
        float range)
    {
        LIGHT = light;
        HEAVY = heavy;
        SPECIAL = special;
        GAPCLOSER = gapcloser;
        RANGE = range;
    }

    public float[] ToArray()
    {
        return new float[5]
        {
            LIGHT,
            HEAVY,
            SPECIAL,
            GAPCLOSER,
            RANGE
        };
    }
}

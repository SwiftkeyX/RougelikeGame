using UnityEngine;

[CreateAssetMenu(fileName = "AttackComboBuffer", menuName = "SO/Weapon/AttackComboBuffer")]
public class WeaponAttackData : ScriptableObject
{
    [SerializeField] private int animationName; // identify the animation clip to use (we have different attack base on the weapon ID)
    [SerializeField] private AttackData[] attackData;

    // getter and setter
    public AttackData[] AttackDatas { get { return attackData; } }
}

[System.Serializable]
public class AttackData
{
    // put 0 if no chain 
    public float chainStart;
    public float chainEnd;
    // hitbox window
    public float hitboxStart;
    public float hitboxEnd;
    // collider id
}
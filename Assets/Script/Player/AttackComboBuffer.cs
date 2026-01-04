using UnityEngine;

[CreateAssetMenu(fileName = "AttackComboBuffer", menuName = "SO/Weapon/AttackComboBuffer")]
public class AttackComboBuffer : ScriptableObject
{
    [SerializeField] private int animationName; // identify the animation clip to use (we have different attack base on the weapon ID)
    [SerializeField] private AttackWindow[] attackWindows;

    // getter and setter
    public AttackWindow[] AttackWindows { get { return attackWindows; } }
}

[System.Serializable]
public struct AttackWindow
{
    public float start;
    public float end;
}
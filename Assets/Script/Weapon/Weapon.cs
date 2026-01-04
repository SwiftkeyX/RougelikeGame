using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;

    [Header("Weapon Holder")]
    [SerializeField] private Transform _weaponSocket;
    [SerializeField] private Quaternion offset;

    [Header("Weapon Stat")]
    [SerializeField] private float _attackDamage;
    [SerializeField] private AttackComboBuffer _buffer;

    private GameObject _currentWeapon;

    // getter and setter
    public float AttackDamge { get { return _attackDamage; } }
    public AttackComboBuffer Buffer { get { return _buffer; } }

    void Start()
    {
        EquipWeapon();
    }

    private void EquipWeapon()
    {
        // spawn weapon attach to weaponSocket
        _currentWeapon = Instantiate(_weapon, _weaponSocket);

        // scale the weapon's size
        _currentWeapon.transform.localScale = new Vector3(1, 1, 1);

        // move weapon to local 0,0,0
        _currentWeapon.transform.localPosition = new Vector3(0, 0, 0);

        // rotate the weapon 
        _currentWeapon.transform.localRotation = offset;
    }

}
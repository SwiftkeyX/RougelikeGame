using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;
    [SerializeField] private GameObject _currentWeapon;

    [Header("Weapon Holder")]
    [SerializeField] private Transform _weaponSocket;
    [SerializeField] private Quaternion offset;

    [Header("Weapon Stat")]
    [SerializeField] private float _attackDamage;
    [SerializeField] private WeaponAttackData _data;    // put "data" only when the Weapon is on the Player

    [Header("Weapon Hitbox")]
    private Hitbox _hitbox;

    // getter and setter
    public float AttackDamge { get { return _attackDamage; } }
    public WeaponAttackData Data { get { return _data; } }
    public GameObject WeaponObject { get { return _weapon; } }
    public Hitbox Hitbox { get { return _hitbox; } }

    void Awake()
    {
        if (_currentWeapon == null) EquipWeapon();

        // initial dependency
        _hitbox = _currentWeapon.GetComponent<Hitbox>();   // get hitbox from the weapon gameobject
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
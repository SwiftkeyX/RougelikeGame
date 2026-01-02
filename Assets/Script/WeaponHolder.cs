using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    // place weapon.R here
    [SerializeField] private Transform _weaponSocket;
    [SerializeField] private GameObject _weapon;
    private GameObject _currentWeapon;
    public Quaternion offset;

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
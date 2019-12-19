using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItems : MonoBehaviour
{
    public Weapon weapon;

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }
}

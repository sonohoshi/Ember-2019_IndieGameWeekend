using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseDown
{
    void MouseDown(Player player);
}

public interface IMouseStay
{
    void MouseStay(Player player);
}
public interface IMouseUp
{
    void MouseUp(Player player);
}
public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float attackDelay;
    public abstract void Attack(Player player);

    public bool IsAttack(float attackTimer)
    {
        if (attackTimer > attackDelay)
        {
            return true;
        }

        return false;
    }
    public enum Weapons{ Torch = 0, Axe = 1, Sword = 2, Shovel = 3, Spear = 4}
}

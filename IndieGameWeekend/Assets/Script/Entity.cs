using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    public float movement;

    public virtual void Hurt(int damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);
        if (health == 0)
        {
            Dead();
        }

    }
    public virtual void Dead()
    {

    }


}

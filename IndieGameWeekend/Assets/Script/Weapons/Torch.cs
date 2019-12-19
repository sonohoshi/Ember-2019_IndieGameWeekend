using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Torch : Weapon, IMouseDown
{
    public Collider2D collider;
    public Effect attackTail;
    private bool isUp;
    private SpriteMask spriteMask;
    private void Awake()
    {
        spriteMask = GetComponentInChildren<SpriteMask>();
    }
    private void Update()
    {
        spriteMask.alphaCutoff= Mathf.PingPong(Time.time*0.1f, 0.2f) + 0.1f;
    }

    public void MouseDown(Player player)
    {
        List<Collider2D> colliders = new List<Collider2D>();
        SoundManager.Instance.PlayEffect("Swing");

        isUp = !isUp;



        if (isUp)
        {
            var c = Instantiate(attackTail, transform.right * 0.125f + transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z));
            var scale = c.transform.localScale;
            scale.y = -2 * player.transform.localScale.x;
            c.transform.localScale = scale;

            transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, 89), 0.1f);

        }
        else
        {
            var c = Instantiate(attackTail, transform.right * 0.125f + transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z));
            var scale = c.transform.localScale;
            scale.y = 2 * player.transform.localScale.x;
            c.transform.localScale = scale;
            transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, -89), 0.1f);
        }
        if (collider.OverlapCollider(new ContactFilter2D(), colliders) > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    var vel = collider.transform.position + (collider.transform.position - transform.position).normalized * 0.2f;
                    collider.transform.DOMove(vel, 0.1f);
                    collider.GetComponent<Entity>().Hurt(damage);
                }
            }
        }
    }

    public override void Attack(Player player)
    {
    }
}

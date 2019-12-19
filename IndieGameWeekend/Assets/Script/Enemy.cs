using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public int damage;
    public Transform target;
    protected bool isDead = false;
    protected bool isHurt = false;
    protected float dropHeart;
    public GameObject heart;
    public Animator anim;
    protected virtual void Update()
    {
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<Entity>().Hurt(damage);
        }
    }
    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        DamageTextManager.Instance.ShowDamage(damage, transform.position);
        anim.SetTrigger("isHurt");
        //StartCoroutine(IHurt());
    }
    public override void Dead()
    {
        isDead = true;
    }
    IEnumerator IHurt()
    {
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime*10;
            GetComponent<SpriteRenderer>().material.SetFloat("_Value", t);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        while (t >0)
        {
            t -= Time.deltaTime * 10;
            GetComponent<SpriteRenderer>().material.SetFloat("_Value", t);
            yield return null;
        }
        GetComponent<SpriteRenderer>().material.SetFloat("_Value", 0);
    }
}

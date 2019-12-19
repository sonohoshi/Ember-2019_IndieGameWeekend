using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon,IMouseDown
{
    public Collider2D collider;
    public Effect attackTail;
    private void Awake()
    {
    }
    private void Update()
    {
    }

    public void MouseDown(Player player)
    {
        Debug.Log("down");
        Sequence sequence = DOTween.Sequence();
        SoundManager.Instance.PlayEffect("Swing");
        sequence.Append(transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, 140), 0.2f).SetEase(Ease.Linear));
        sequence.Insert(0.8f, transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, 0), 0.07f).SetEase(Ease.InQuart));
        sequence.AppendCallback(new TweenCallback(()=> { Attacks(); }));
        sequence.Play();
    }

    private void Attacks()
    {
        CameraManager.CameraShake(0.3f, 0.4f);
        SoundManager.Instance.PlayEffect("Axe");
        List<Collider2D> colliders = new List<Collider2D>();
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
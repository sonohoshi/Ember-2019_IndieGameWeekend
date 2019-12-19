using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBreak : MonoBehaviour
{
    private Animator animator;
    public Collider2D collider;
    public bool isBoom;
    private IEnumerator Boom()
    {
        yield return new WaitForSeconds(3);
        animator.SetTrigger("Boom");
        isBoom = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(1);
        if(isBoom&&collision.CompareTag("Player"))
        {
            collision.GetComponent<Entity>().Hurt(10);
        }
    }
    private void Start()
    {
        Destroy(gameObject,4.2f);
        animator = GetComponent<Animator>();
        StartCoroutine(Boom());
    }
   
}

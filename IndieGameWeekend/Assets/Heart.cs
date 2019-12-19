using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            collider.GetComponent<Player>().health = Mathf.Clamp(collider.GetComponent<Player>().health + 5, 0, 100);
            Destroy(gameObject);
        }
    }
}

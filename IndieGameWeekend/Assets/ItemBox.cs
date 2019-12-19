using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemBox : MonoBehaviour
{
    private bool isOpen;
    public Sprite openBox;
    public WeaponItems[] weaponItems;
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);
            foreach(var collider in colliders)
            {
                if(collider.CompareTag("Player")&&!isOpen)
                {
                    var item= Instantiate(weaponItems[Random.Range(0, weaponItems.Length)],transform.position,Quaternion.identity);
                    item.transform.DOJump(transform.position, 0.3f, 1, 1);
                    GetComponent<SpriteRenderer>().sprite = openBox;
                    isOpen = true;
                }
            }
        }
    }
}

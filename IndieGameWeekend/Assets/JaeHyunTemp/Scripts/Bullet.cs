using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public Vector2 speedVector;
    public Transform axis;

    void Update()
    {
        transform.Translate(speedVector * Time.deltaTime);
        OutOfScreen();
    }

    void OutOfScreen()
    {
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPos.x > 1 || viewportPos.x < 0 || viewportPos.y > 1 || viewportPos.y < 0)
        {
            gameObject.SetActive(false);
            transform.position = axis.transform.position;
            BulletPool.Instance.PushBullet(this.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("총알 데미지");
            gameObject.SetActive(false);
            transform.position = axis.transform.position;

            BulletPool.Instance.PushBullet(this.gameObject);
                        //데미지
        }
    }
}

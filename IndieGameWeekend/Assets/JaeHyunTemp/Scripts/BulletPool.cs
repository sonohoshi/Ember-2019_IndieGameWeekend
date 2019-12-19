using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletPool : MonoBehaviour
{

    public static BulletPool Instance = null;

    public GameObject bulletPref;
    public int bulletNum;

    private Stack<GameObject> bulletStack = new Stack<GameObject>();

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;

        InitPool();
    }

    void InitPool()
    {
        for (int i = 0; i < bulletNum; i++)
        {
            PushBullet(CreateBullet());
        }
    }

    GameObject CreateBullet()
    {
        var temp_Obj = Instantiate(bulletPref, transform);
        temp_Obj.SetActive(false);

        return temp_Obj;
    }

    public GameObject PopBullet()
    {
        if (bulletStack.Count == 0)
            PushBullet(CreateBullet());

        var temp_Obj = bulletStack.Pop();
        temp_Obj.SetActive(true);
        return temp_Obj;
    }

    public void PushBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletStack.Push(bullet);
    }
}

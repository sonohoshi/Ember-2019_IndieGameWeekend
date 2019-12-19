using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    public cN_WayBullet m_cN_WayBullet;
    public float m_time;

    WaitForSeconds m_waitForSeconds;

    public void Update()
    {

    }
    IEnumerator Start()
    {
        Debug.Log("왜 안들어와");
        m_waitForSeconds = new WaitForSeconds(m_time);
        while (true)
        {
            yield return null;
            for (int i = 0; i < m_cN_WayBullet.bulletsSpeedVector.Length; i++)
            {
                var temp_Obj = BulletPool.Instance.PopBullet();
                var bullet = temp_Obj.GetComponent<Bullet>();
                bullet.speedVector = m_cN_WayBullet.bulletsSpeedVector[i];
                bullet.axis = transform;
            }

            yield return m_waitForSeconds;
        }
    }

}

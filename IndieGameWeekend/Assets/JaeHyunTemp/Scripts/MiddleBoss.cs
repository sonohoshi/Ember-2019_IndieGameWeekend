using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : Entity
{
    GameObject m_Player;
    [SerializeField] float m_Speed = 10f;
    [SerializeField] bool IsShockwave = false;

    [SerializeField] int m_Seed;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Seed = Random.Range(0, 2);
        StartCoroutine(Turn(m_Seed));
    }
    IEnumerator Desh(int WaitTime, float speed)// 돌진
    {
        Debug.Log("돌진");

        float t = 0;
        var heading = gameObject.transform.position - m_Player.transform.position;
        var dis = heading.magnitude;
        var dir = heading / dis;
        Vector3 Playertemp = m_Player.transform.position;

        if (Playertemp.x >= gameObject.transform.position.x)
        {
            while (t < 1)
            {
                if (Playertemp.x <= gameObject.transform.position.x)
                {
                    m_Seed = Random.Range(0, 2);
                    StartCoroutine(Turn(m_Seed));
                    break;
                }

                //Debug.Log("EnemyPos:" + gameObject.transform.position + "PlayerPos:" + Playertemp);
                gameObject.transform.Translate(-dir * speed * Time.deltaTime);
                yield return null;

            }
        }
        else if (Playertemp.x <= gameObject.transform.position.x)
        {
            while (t < 1)
            {
                if (Playertemp.x >= gameObject.transform.position.x)
                {
                    m_Seed = Random.Range(0, 2);
                    StartCoroutine(Turn(m_Seed));
                    
                    break;

                }

                //Debug.Log("EnemyPos:" + gameObject.transform.position + "PlayerPos:" + Playertemp);
                gameObject.transform.Translate(-dir * speed * Time.deltaTime);
                yield return null;
            }
        }
    }
    IEnumerator Shockwave(int WaitTime) //충격파
    {
        Debug.Log("충격파");
        IsShockwave = true;

        yield return new WaitForSeconds(WaitTime);

        IsShockwave = false;
        m_Seed = Random.Range(0, 2);
        StartCoroutine(Turn(m_Seed));
    }
    IEnumerator Turn(int seed)
    {
        yield return new WaitForSeconds(1);

        switch(seed)
        {
            case 0:
                StartCoroutine(Desh(2, 20));
                break;
            case 1:
                StartCoroutine(Shockwave(1));
                break;
            default:
                Debug.Log("!!!!!!");
                break;

        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsShockwave)
        {
            if (other.tag == "Player")
            {
                Debug.Log("데미지 줌");
            }
        }

    }
    /*
중간보스 패턴

   땅에서 다니는 친구였음

제자리에서 n초 대기, 플레이어 방향 돌진 // 쿠파주니어



제자리에서 n초 대기, 보스 주변에 충격파로 주변에 플레이어 있으면 데미지 입음

*/
}

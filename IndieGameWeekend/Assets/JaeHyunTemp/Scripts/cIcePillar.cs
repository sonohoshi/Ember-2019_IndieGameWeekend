using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cIcePillar : MonoBehaviour
{
    [SerializeField]
    int m_Count;

    [SerializeField]
    GameObject IcePillar;
    [SerializeField]
    Vector2 temp;

    [SerializeField]
    float m_Time = 0.2f;
    [SerializeField]
    int m_WaitTime;

    [SerializeField]
    GameObject m_Player;


    Vector3 dir;
    Vector3 dirToTarget;

    [SerializeField]
    float m_Interval;


    private void Start()
    {
        //StartCoroutine(MakePillar(m_Time, 2));
        m_Player = GameObject.FindGameObjectWithTag("Player");

       // this.transform.LookAt(this.m_Player.transform);

    }
    private void Update()
    {
        //this.transform.LookAt(this.m_Player.transform);

    }
    IEnumerator MakePillar(float waitTime, float interval)
    {

        temp = new Vector2(transform.position.x- (interval*2), transform.position.y);
        for (int i = 0; i < 5; i++)
        {
            temp.x += interval;
            Instantiate(IcePillar, temp, Quaternion.identity);
            Destroy(GameObject.Find("Ice(Clone)"), 1f);
            yield return new WaitForSeconds(waitTime);

        }
    }
}

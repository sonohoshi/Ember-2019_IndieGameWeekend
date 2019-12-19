using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cN_WayBullet : MonoBehaviour
{
    //중심 탄환 속도 벡터
    [SerializeField]
    private Vector2 axisSpeedVector;

    //탄환과 탄환 사이의 각도
    [SerializeField]
    private float intervalTheta;

    //탄환의 수
    [SerializeField]
    private int bulletNum;

    //탄환들의 속도 벡터
    public Vector2[] bulletsSpeedVector;

    void Awake()
    {
        bulletsSpeedVector = new Vector2[bulletNum];
        float edgeDegree;

        if (bulletNum % 2 == 0)
        {
            edgeDegree = (-bulletNum / 2 + 0.5f) * intervalTheta;
        }
        else
        {
            edgeDegree = -(bulletNum - 1) / 2 * intervalTheta;
        }


        for (int i = 0; i < bulletNum; i++)
        {
            bulletsSpeedVector[i] = axisSpeedVector.Rotate(edgeDegree);
            edgeDegree += intervalTheta;
        }
    }
}

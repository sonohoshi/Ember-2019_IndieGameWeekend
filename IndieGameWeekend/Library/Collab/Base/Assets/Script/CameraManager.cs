using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform Target;
    private void Update()
    {
        Vector2 pos = transform.position;
        pos = Vector2.Lerp(transform.position, Target.position, Time.deltaTime);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
    public void CameraShake()
    {
        GetComponent<Camera>().DOShakePosition(0.4f, 0.5f);

    }
}

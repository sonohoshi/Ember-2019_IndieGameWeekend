using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<CameraManager>();
            }
            return instance;
        }
    }
    public Transform Target;
    public SpriteRenderer lightBlackGround;
    public void SetLightBlackGround(bool isShow)
    {
        lightBlackGround.maskInteraction = (isShow) ? SpriteMaskInteraction.VisibleOutsideMask : SpriteMaskInteraction.None;
    }
    private void LateUpdate()
    {
        Vector2 pos = transform.position;
        pos = Vector2.LerpUnclamped(transform.position, Target.position, 5*Time.deltaTime);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
    public static void CameraShake(float d= 0.5f,float p=0.3f)
    {
        Camera.main.DOShakePosition(d, p);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageTextManager : MonoBehaviour
{
    public DamageText damageText;
    public TextMesh[] healthText;
    private static DamageTextManager instance = null;
    public static DamageTextManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = GameObject.FindObjectOfType<DamageTextManager>();
            }
            return instance;
        }
    }
    public void ShowHealthShow(int number)
    {
        var textmesh =Instantiate<TextMesh>(healthText[number], new Vector3(0, -0.7f), Quaternion.identity, Camera.main.transform);
        textmesh.transform.localPosition = new Vector3(0, -1,10);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(textmesh.transform.DOLocalMoveY(-0.7f, 0.2f));

        sequence.Insert(3,textmesh.transform.DOLocalMoveY(-1, 1));
    }
    public void ShowDamage(int damage,Vector2 pos)
    {
        var damageText = Instantiate(this.damageText, pos, Quaternion.identity);
        damageText.SetDamage(damage);
    }
}

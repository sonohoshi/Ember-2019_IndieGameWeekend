using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float t;
    Vector3 pos;
    public float power_1;
    public float power_2;
    public TextMesh textMesh;
    
    void Start()
    {
        t = 0;
        pos = transform.position;
    }

    public void SetDamage(int damage)
    {
        textMesh.text = damage.ToString();
    }
    void Update()
    {
        t += Time.deltaTime;
        transform.position = pos + new Vector3(t, -power_1 * Mathf.Pow((t - power_2),2)  + power_1 * Mathf.Pow(power_2, 2));
        Color color= textMesh.color;
        color.a = 1-t / 1.50f;
        textMesh.color = color;
        if(t> 1.50f)
        {
            Destroy(gameObject); 
        }
    }
}

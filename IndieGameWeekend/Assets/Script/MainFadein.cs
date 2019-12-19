using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainFadein : MonoBehaviour
{
    private Image blackBack;
    private bool isPlaying = false;
    void Awake()
    {
        blackBack = GetComponent<Image>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
            StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        if (isPlaying)
            yield return null;
        Color fade;
        fade = blackBack.color;
        isPlaying = true;
        if (fade.a > 0f)
        {
            fade.a -= 0.01f;
            blackBack.color = fade;
            yield return new WaitForSeconds(0.01f);
        }
        else
        {
            gameObject.SetActive(false);
        }
        isPlaying = false;
    }
}

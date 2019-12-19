using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Stats")]
    public Image healthBar;
    public Image energyhBar;

    [Header("Icon")]
    public Image weaponIcon;
    public void SetHealthBar(int health,int maxHealth)
    {
        float amount = health / (float)maxHealth;
        healthBar.fillAmount = amount;
    }
    public void SetEnergyBar(float energy, float maxenergy)
    {
        float amount = energy / (float)maxenergy;
        energyhBar.fillAmount = amount;
    }
    public void GoTitle()
    {
        SceneManager.LoadScene(0);
    }
}

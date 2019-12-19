using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Weapon basicWeapon;
    private Weapon[] weapons;
    private Player player;

    public CanvasManager canvasManager;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        weapons = new Weapon[15];
    }
    private void Update()
    {
        canvasManager.SetHealthBar(player.health, 100);
        canvasManager.SetEnergyBar(player.energy, 100);
    }

}

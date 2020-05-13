using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    private GameControllerScript gameController;
    private GameObject worm;
    void Start()
    {
        gameController = GetComponent<GameControllerScript>();
        worm = gameController.activeWorm;
    }

    void Update()
    {
        
    }

    public void ChangeWeapon(string newWeapon)
    {
        worm.GetComponent<WormWeaponsScript>().ChangeWeaponState(newWeapon);
    }
    public void Surrender()
    {
        int surrenderTeam = gameController.activeWorm.GetComponent<WormHealthScript>().teamNumber;
        if(surrenderTeam == 1) gameController.TeamWins(2);
        else gameController.TeamWins(1);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<WormHealthScript>().teamNumber == surrenderTeam) player.GetComponent<WormAnimationsScript>().Surrender();
        }
    }
}

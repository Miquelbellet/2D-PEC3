using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public Camera mainCam;
    [Range(1f, 15f)] public float cameraDistance;
    public GameObject allWormsObj;
    [HideInInspector] public GameObject activeWorm;

    private int mapNumber;
    private GameObject[] wormsList;
    private int numActiveWorm;
    private void Awake()
    {
        initParams();
    }
    void Start()
    {
        
    }

    void Update()
    {
        mainCam.transform.position = new Vector3(activeWorm.transform.position.x, activeWorm.transform.position.y, -cameraDistance);
    }
    void initParams()
    {
        mapNumber = PlayerPrefs.GetInt("mapNumber", 1);
        allWormsObj.transform.GetChild(mapNumber - 1).gameObject.SetActive(true);
        wormsList = new GameObject[4];
        for (int i = 0; i < allWormsObj.transform.GetChild(mapNumber - 1).childCount; i++)
        {
            wormsList[i] = allWormsObj.transform.GetChild(mapNumber - 1).GetChild(i).gameObject;
        }
        activeWorm = wormsList[numActiveWorm];
    }
    public void NextPlayer()
    {
        if (numActiveWorm < wormsList.Length-1) numActiveWorm++;
        else numActiveWorm = 0;
        activeWorm = wormsList[numActiveWorm];
    }
    void CheckingWinner()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        bool blueSurvivors = false;
        bool redSurvivors = false;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<WormHealthScript>().teamNumber == 1) blueSurvivors = true;
            else if (player.GetComponent<WormHealthScript>().teamNumber == 2) redSurvivors = true;
        }
        if (!blueSurvivors) TeamWins(2);
        else if (!redSurvivors) TeamWins(1);
    }
    public void CheckForWinner()
    {
        Invoke("CheckingWinner", 1f);
    }
    public void FindExplosionHit(Vector2 explosionPos, float explosionDamage, float explosionRadious)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(explosionPos, player.transform.position);
            if (dist < explosionRadious)
            {
                player.GetComponent<WormHealthScript>().ReduceHealth(explosionDamage);
            }
        }
    }
    public void TeamWins(int winnerTeam)
    {
        Debug.Log("The Winner Team is: "+ winnerTeam);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<WormHealthScript>().teamNumber == winnerTeam) player.GetComponent<WormAnimationsScript>().Winner();
        }
    }
}

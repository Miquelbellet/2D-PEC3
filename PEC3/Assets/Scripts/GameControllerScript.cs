using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    [Range(1f, 15f)] public float cameraDistance;
    public float cameraTransitionTime;
    public GameObject allWormsObj, allDecorationObj;
    [HideInInspector] public GameObject activeWorm;
    [HideInInspector] public bool playingWithAI;

    private SoundEffectsScript soundsScript;
    private int mapNumber, numActiveWorm;
    private GameObject[] wormsList;
    private bool isChangingPlayer;
    private List<int> numberDeads = new List<int>();
    private void Awake()
    {
        initParams();
    }
    void Start()
    {

    }

    void Update()
    {
        if(activeWorm.gameObject) Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(activeWorm.transform.position.x, activeWorm.transform.position.y, Camera.main.transform.position.z), cameraTransitionTime * Time.deltaTime);
    }
    void initParams()
    {
        soundsScript = GetComponent<SoundEffectsScript>();
        mapNumber = PlayerPrefs.GetInt("mapNumber", 1);
        if (PlayerPrefs.GetString("playAi", "true") == "true") playingWithAI = true;
        else playingWithAI = false;
        allWormsObj.transform.GetChild(mapNumber - 1).gameObject.SetActive(true);
        allDecorationObj.transform.GetChild(mapNumber - 1).gameObject.SetActive(true);
        wormsList = new GameObject[mapNumber*2];
        for (int i = 0; i < allWormsObj.transform.GetChild(mapNumber - 1).childCount; i++)
        {
            wormsList[i] = allWormsObj.transform.GetChild(mapNumber - 1).GetChild(i).gameObject;
        }
        activeWorm = wormsList[numActiveWorm];
    }
    public void NextPlayer()
    {
        GetComponent<UIScript>().hasWormAttacked = true;
        Invoke("ChangeFocusWorm", 4f);
        isChangingPlayer = true;
    }
    void ChangeFocusWorm()
    {
        if (isChangingPlayer)
        {
            isChangingPlayer = false;
            if (numActiveWorm < wormsList.Length - 1) numActiveWorm++;
            else numActiveWorm = 0;

            if (!numberDeads.Contains(numActiveWorm))
            {
                soundsScript.SelectWormClip();
                activeWorm = wormsList[numActiveWorm];
                if (playingWithAI && activeWorm.GetComponent<WormHealthScript>().teamNumber == 2)
                {
                    activeWorm.GetComponent<WormAIEnemieScript>().BeginAI();
                }
                else GetComponent<UIScript>().hasWormAttacked = false;
            }
        }
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
    public void WormDied(GameObject deadWorm)
    {
        for(int i = 0; i < wormsList.Length - 1; i++)
        {
            if (wormsList[i] == deadWorm) numberDeads.Add(i);
        }
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
        GetComponent<UIScript>().SetTextWinner(winnerTeam);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<WormHealthScript>().teamNumber == winnerTeam) player.GetComponent<WormAnimationsScript>().Winner();
        }
        soundsScript.WinningClip();
        Invoke("GoToMenuPage", 4f);
    }
    void GoToMenuPage()
    {
        SceneManager.LoadScene("MenuScene");
    }
}

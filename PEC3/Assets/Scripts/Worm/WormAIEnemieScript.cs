using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAIEnemieScript : MonoBehaviour
{
    public float walkingForce;
    public float walkingTime;

    public float airAttackRange;
    public float bazookaRange;
    public float uziRange;
    public float axHitRange;

    [HideInInspector] public bool scriptActivated;

    enum enemieStates { None, Positioning, Aiming, Shooting };
    enemieStates currentState;

    private List<GameObject> players = new List<GameObject>();
    private GameObject focusedPlayer;
    private bool isDebuging, shooted;
    private float time;
    private int intTime;
    private string currentWeapon;
    void Start()
    {

    }

    void Update()
    {
        if(scriptActivated) EnemieStateMachine();
    }
    void SearchPlayers()
    {
        var allPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in allPlayers)
            if (player.GetComponent<WormHealthScript>().teamNumber == 1) players.Add(player);

        var shotestDist = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < shotestDist)
            {
                shotestDist = dist;
                focusedPlayer = player;
            }
           
        }
    }
    void EnemieStateMachine()
    {
        switch (currentState)
        {
            case enemieStates.Positioning:
                if (isDebuging) Debug.Log("Positioning");
                PositioningState();
                break;
            case enemieStates.Aiming:
                if (isDebuging) Debug.Log("Aiming");
                AimingState();
                break;
            case enemieStates.Shooting:
                if (isDebuging) Debug.Log("Shooting");
                ShootingState();
                break;
            default:
                currentState = enemieStates.None;
                break;
        }
    }
    void ChangeAIState(enemieStates newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }
    void PositioningState()
    {
        time += Time.deltaTime;
        float dist = Vector2.Distance(transform.position, focusedPlayer.transform.position);
        if (time < walkingTime)
        {
            if (transform.position.x > focusedPlayer.transform.position.x) GetComponent<WormMovementScript>().MovmentForAI("Left", false);
            else GetComponent<WormMovementScript>().MovmentForAI("Right", false);

            if(intTime != (int)time)
            {
                intTime = (int)time;
                if (transform.position.x > focusedPlayer.transform.position.x) GetComponent<WormMovementScript>().MovmentForAI("Left", true);
                else GetComponent<WormMovementScript>().MovmentForAI("Right", true);
            }

            if (dist > airAttackRange)
            {
                currentWeapon = "AirAttack";
            }
            else if (dist < airAttackRange && dist > bazookaRange)
            {
                currentWeapon = "Bazooka";
            }
            else if (dist < bazookaRange && dist > uziRange)
            {
                currentWeapon = "Uzi";
            }
            else if (dist < uziRange && dist > axHitRange)
            {
                currentWeapon = "AxHit";
            }
            else
            {
                currentWeapon = "AxHit";
                ChangeAIState(enemieStates.Aiming);
            }
        }
        else ChangeAIState(enemieStates.Aiming);
    }
    void AimingState()
    {
        GetComponent<WormMovementScript>().MovmentForAI("None", false);
        GetComponent<WormWeaponsScript>().ChangeWeaponState(currentWeapon);
        ChangeAIState(enemieStates.Shooting);
    }
    void ShootingState()
    {
        Invoke("ShotPlayer", 2f);
    }
    void ShotPlayer()
    {
        if (!shooted)
        {
            shooted = true;
            GetComponent<WormWeaponsScript>().AIShootWeapon(focusedPlayer.transform.position);
            EndAI();
        }
    }
    public void BeginAI()
    {
        if(GetComponent<WormHealthScript>().currentHeath > 0)
        {
            currentState = enemieStates.Positioning;
            SearchPlayers();
            scriptActivated = true;
            shooted = false;
            time = 0;
        }
    }
    public void EndAI()
    {
        currentState = enemieStates.None;
        scriptActivated = false;
    }
}

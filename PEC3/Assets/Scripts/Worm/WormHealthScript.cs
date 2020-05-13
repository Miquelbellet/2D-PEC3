using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WormHealthScript : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject explosionPrefab;
    public GameObject graveBlueTeam;
    public GameObject graveRedTeam;
    public TextMeshProUGUI healthTxt;
    public Slider sliderBar;
    public Image fillImageHealth;

    [Header("Parameters")]
    [Range(1, 2)]
    public int teamNumber;
    public float startingHealth;
    public float deadExplosionDamage;
    public float deadExplosionRadius;

    [HideInInspector] public bool isDead;

    private WormAnimationsScript animScript;
    private float currentHeath;
    void Start()
    {
        animScript = GetComponent<WormAnimationsScript>();
        currentHeath = startingHealth;
        SetHealthBar();
    }

    void Update()
    {

    }
    public void ReduceHealth(float reducedHealth)
    {
        currentHeath -= reducedHealth;
        SetHealthBar();
    }
    public void WormAnimationDied()
    {
        GameControllerScript gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(new Vector2(0, 0)));
        gameController.FindExplosionHit(explosion.transform.position, deadExplosionDamage, deadExplosionRadius);
        gameController.CheckForWinner();
        if (teamNumber == 1) Instantiate(graveBlueTeam, transform.position, Quaternion.Euler(new Vector2(0, 0)));
        else if (teamNumber == 2) Instantiate(graveRedTeam, transform.position, Quaternion.Euler(new Vector2(0, 0)));
        Destroy(explosion, 5f);
        Destroy(gameObject);
    }
    private void SetHealthBar()
    {
        sliderBar.value = currentHeath;
        if(currentHeath <= 0)
        {
            currentHeath = 0;
            isDead = true;
            animScript.Died();
        }
        fillImageHealth.color = Color.Lerp(Color.red, Color.green, currentHeath / startingHealth);
        healthTxt.text = currentHeath.ToString();
        if (teamNumber == 1) healthTxt.color = Color.blue;
        else healthTxt.color = Color.red;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour
{
    public GameObject panelWeapons;
    public TextMeshProUGUI winnerTxt;
    public float panelWeaponsShowTime;
    [HideInInspector] public bool hasWormAttacked;

    private GameControllerScript gameController;
    private SoundEffectsScript soundScript;
    private GameObject worm;
    private bool pointingPanel;
    private Vector3 panelWeaponsPosition;
    void Start()
    {
        gameController = GetComponent<GameControllerScript>();
        soundScript = GetComponent<SoundEffectsScript>();
        worm = gameController.activeWorm;
        winnerTxt.gameObject.SetActive(false);
        panelWeaponsPosition = panelWeapons.transform.localPosition;
    }

    void Update()
    {
        ShowHidePanelWeapons();
    }
    void ShowHidePanelWeapons()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        pointingPanel = false;
        foreach (RaycastResult result in results) if (result.gameObject.tag == "Panel") pointingPanel = true;

        if (pointingPanel && !hasWormAttacked) panelWeapons.transform.localPosition = Vector3.Lerp(panelWeapons.transform.localPosition, new Vector3(panelWeaponsPosition.x, panelWeaponsPosition.y, panelWeaponsPosition.z), panelWeaponsShowTime * Time.deltaTime);
        else panelWeapons.transform.localPosition = Vector3.Lerp(panelWeapons.transform.localPosition, new Vector3(panelWeaponsPosition.x+85, panelWeaponsPosition.y, panelWeaponsPosition.z), panelWeaponsShowTime * Time.deltaTime);
    }
    public void ChangeWeapon(string newWeapon)
    {
        soundScript.ButtonSelectClip();
        worm = gameController.activeWorm;
        var currentWeapon = worm.GetComponent<WormWeaponsScript>().currentWeapon;
        if (newWeapon == currentWeapon.ToString()) newWeapon = "None";
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
    public void SetTextWinner(int winnerTeam)
    {
        winnerTxt.gameObject.SetActive(true);
        if (winnerTeam == 1) winnerTxt.text = "Blue Team Wins!";
        else winnerTxt.text = "Red Team Wins!";
    }
}

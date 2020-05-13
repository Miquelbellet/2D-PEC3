using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControllerScript : MonoBehaviour
{
    public int maxNumberMaps;
    public Image imgContainer;
    public Sprite[] mapsImgs;

    private int mapNumber = 1;
    void Start()
    {
        imgContainer.GetComponent<Image>().sprite = mapsImgs[mapNumber-1];
    }

    void Update()
    {
        
    }
    public void sumaNum()
    {
        if (mapNumber < maxNumberMaps)
        {
            mapNumber++;
            imgContainer.GetComponent<Image>().sprite = mapsImgs[mapNumber-1];
        }
    }
    public void restaNum()
    {
        if (mapNumber > 1)
        {
            mapNumber--;
            imgContainer.GetComponent<Image>().sprite = mapsImgs[mapNumber-1];
        } 
    }
    public void StartGame()
    {
        PlayerPrefs.SetInt("mapNumber", mapNumber);
        SceneManager.LoadScene("GameScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

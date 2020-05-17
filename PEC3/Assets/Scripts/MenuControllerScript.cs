using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControllerScript : MonoBehaviour
{
    public int maxNumberMaps;
    public Image imgContainer;
    public AudioClip selectClip;
    public Sprite[] mapsImgs;

    private AudioSource audioSource;
    private bool playAi = true;
    private int mapNumber = 1;
    void Start()
    {
        PlayerPrefs.SetString("playAi", "true");
        imgContainer.GetComponent<Image>().sprite = mapsImgs[mapNumber-1];
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
    public void playAiToggle()
    {
        playAi = !playAi;
        if(playAi) PlayerPrefs.SetString("playAi", "true");
        else PlayerPrefs.SetString("playAi", "false");
        audioSource.PlayOneShot(selectClip);
    }
    public void sumaNum()
    {
        if (mapNumber < maxNumberMaps)
        {
            mapNumber++;
            imgContainer.GetComponent<Image>().sprite = mapsImgs[mapNumber-1];
            audioSource.PlayOneShot(selectClip);
        }
    }
    public void restaNum()
    {
        if (mapNumber > 1)
        {
            mapNumber--;
            imgContainer.GetComponent<Image>().sprite = mapsImgs[mapNumber-1];
            audioSource.PlayOneShot(selectClip);
        } 
    }
    public void StartGame()
    {
        audioSource.PlayOneShot(selectClip);
        PlayerPrefs.SetInt("mapNumber", mapNumber);
        SceneManager.LoadScene("GameScene");
    }
    public void ExitGame()
    {
        audioSource.PlayOneShot(selectClip);
        Application.Quit();
    }
}

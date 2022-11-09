using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIController : MonoBehaviour
{
    bool shallWePlay = false;
    float speed = 7;
    public GameObject title;
    public GameObject[] lights;
    public GameObject [] Buttons= new GameObject [2];
    float timer = 0;
    int l;
    private void Start()
    {
        shallWePlay = false;
        timer = 0;
        l = 2;
    }
    private void FixedUpdate()
    {
        if (shallWePlay)
        {
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 25, Time.deltaTime * speed);
            timer += Time.deltaTime;
            if ( timer>=0.5f && l < 6)
            {
                print(timer + "-" + (Mathf.Round(timer % 0.3f * 10) * 0.1f) + "-" + l);
                lights[l].SetActive(true);
                l++;
                timer = 0;
            }

        }
        if(Camera.main.fieldOfView >= 25)
        {
            print(timer);
            GameManager.instance.gameState = 1;
            print("Loading");
            SceneManager.LoadScene("Main Game");
        }
        
    }
    public void StartGame()
    {
        shallWePlay = true;
        lights[0].SetActive(false);
        lights[1].SetActive(false);
        title.SetActive(false);
        for(int x=0; x<2;x++)
            Buttons[x].SetActive(false);
        
    }

    public void SaveStuff()
    {
        GameManager.instance.SaveGame();
    }
    public void LoadStuff()
    {
        GameManager.instance.LoadGame();
    }
    public GameObject Settings;
    bool settings = false;
    public void SettingsScreen()
    {
        Settings.SetActive(!settings);
        for (int x = 0; x < 2; x++)
            Buttons[x].SetActive(settings);
        settings = !settings;
    }
    public GameObject Skins;
    bool skins = false;
    public void SkinsScreen()
    {
        Skins.SetActive(!skins);
        skins = !skins;
    }
    public void ThermalExhaustPort()
    {
        GameManager.instance.ImplodeDeathStar();
    }
}

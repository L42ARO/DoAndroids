using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverUI : MonoBehaviour
{
    public GameObject GameOver;
    public GameObject pauseButton;
    public Text highScoreLabel;
    public Text highestLevelLabel;
    // Update is called once per frame
    public void CheckGameOver(float highScore, int highestLevel=0 )
    {
        pauseButton.SetActive(false);
        GameOver.SetActive(true);
        highScoreLabel.text = ""+highScore;
        highestLevelLabel.text = (highestLevel>9?"":"0") + highestLevel;
    }
    public void ResetGame()
    {

        //GameManager.playerAlive = true;
        SceneManager.LoadScene("Main Game");
    }
    public void HomeSweetHome()
    {
        SceneManager.LoadScene("Home");
    }
}

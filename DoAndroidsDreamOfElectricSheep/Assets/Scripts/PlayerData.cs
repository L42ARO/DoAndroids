using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int highestLevel;
    public int highScore;
    public int coins;

    public PlayerData(GameManager GM)
    {
        highestLevel = GM.highestLevel;
        highScore = GM.highScore;
        coins = GM.coins;
    }
}

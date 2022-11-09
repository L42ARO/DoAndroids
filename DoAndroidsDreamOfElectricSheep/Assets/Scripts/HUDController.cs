using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Text level, score, coins, newCryptos;//,cryptosOnUSB;
    private void Start()
    {
        ResetHUD();
        newCryptos.gameObject.SetActive(false);
    }
    public void ResetHUD()
    {
        float levelNum = GameManager.level;
        int enemyNum= GameManager.EnemyCount; ;
        int coinNum = GameManager.instance.coins;
        //enemies.text = (enemyNum < 10 ? "0" : "")+enemyNum;
        coins.text = (coinNum < 10 ? "0" : "") + coinNum;
        level.text = (levelNum<10?"0":"")+levelNum;
    }
    public void ResetScore(float u)
    {
        score.text = ((u<100)?"00":"0")+u;
    }
    public void NotifyNewCryptos(int quant)
    {
        newCryptos.gameObject.SetActive(true);
        newCryptos.text = "+" + quant + ".00";
        StartCoroutine(FadeTextToZeroAlpha(2.0f, newCryptos));
        //StartCoroutine(FadeTextToZeroAlpha(2.0f, cryptosOnUSB));
    }
    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    float originalY;
    float speed=10;
    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y - originalY <= 31) transform.position += transform.up * Time.deltaTime * speed;
        else
        {
            //AttackPlayer.reset = true;
            GameManager.instance.win = false;
            GameManager.instance.hud.ResetHUD();
            GameManager.stopPlatform = false;
            GameManager.instance.changeHud();//we change the HUD layout after moving the player to the next level
            Destroy(gameObject,0.25f);
        }
    }
}

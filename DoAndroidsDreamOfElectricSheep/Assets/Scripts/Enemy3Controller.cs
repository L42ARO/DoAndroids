using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    float initialY;
    float speed = 1;
    public GameObject Emitter;//this has to be manyally done in the unity editor since if we add a GameObject.Find, when there are more than 1 the script will get confused
    GameObject Player;//there is only 1 player so there wont be any confusion with GameObject.Find
    [SerializeField] private LineRenderer CaptureLaser;//this also has to be done manually
    void Start()
    {
        initialY = transform.position.y;//we record the intial Y since that is the point around it will hover
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.playerAlive)//we must check if the player is alive in order to avoid errros when the player dies
            if ((Mathf.Abs(Player.transform.position.y - transform.position.y)) < 2)//to avoid lasers before the player arrives we add this funciton that checks the distance to the player
                LaserStreams();
        Hovering();//the hovering doesn't depend on the player so it can stay out of the conditional
    }
    void Hovering()//funciton that makes the inhibitors hover around a set point in the Y axis
    {
        transform.position += Vector3.up * Time.deltaTime * speed;
        if (Mathf.Abs(initialY - transform.position.y) >= 0.5f) 
            speed *= -1;
    }
    void LaserStreams()//funciton that makes the lasers go from the tip of the inhibitors to the player
    {
        CaptureLaser.SetPosition(0, Emitter.transform.position);
        CaptureLaser.SetPosition(1, Player.transform.position);
    }
    bool redDead = false;//function that avoids confusion of inhibitors death, since bafore it seemed to report dying twice from 2 bullets that were closely shot
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag=="Player" || other.gameObject.tag=="Proyectile1") //avoid collision with enemies and player since it's objetive is to hover and inhibit the player
        {
            Physics.IgnoreCollision(other.collider, transform.GetComponent<Collider>());
        }
        else if (other.gameObject.tag == "Proyectile" && !redDead)//it shouldn't die twice
        {
            redDead = true;//he's dead he can't die again so we check that to avoid reducing the enmy count more than it already is
            GameManager.Inhibitors -= 1;//this reduces the inhibitor count therefore restoring the mobility of the player
            GameManager.EnemyCount -= 1;
            //GameManager.instance.changeHud();
            Destroy(gameObject,0.2f);//we wait the 0.2 seconds so that it coordintates witht he bullet dispearing
            
        }
    }
}

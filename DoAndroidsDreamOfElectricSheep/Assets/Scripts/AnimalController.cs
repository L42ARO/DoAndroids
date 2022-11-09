using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject finalPlace;
    GameObject Player;
    float speed = 1.5f;
    Vector3 direction;
    Vector3 distToPlayer;
    public bool free = true;
    public bool safe = true;
    void Start()
    {
        finalPlace = GameObject.Find("SheepPosition");
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.playerAlive||(!free && !safe))
        {
            
            distToPlayer = Player.transform.position - transform.position;
            if ((free && distToPlayer.magnitude<1.5f) || GameManager.instance.win)
            {
                safe = true;
                transform.position = finalPlace.transform.position;
                transform.rotation = finalPlace.transform.rotation;
            }else if (free)
            {
                safe = false;
                GoodBoy();
            }

        }
        /*else if (free && safe)
        {
            Destroy(gameObject);
        }*/
    }

    void GoodBoy()
    {
        direction = finalPlace.transform.position - transform.position;
        transform.position += direction * Time.deltaTime * speed;
        transform.LookAt(finalPlace.transform.position);
    }
    /*void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Platform")
        {
            Physics.IgnoreCollision(other.collider, transform.GetComponent<Collider>());
        }
        if (other.gameObject.tag == "Platform")
        {
            print("Ouch");
            //Physics.IgnoreCollision(other.collider, transform.GetComponent<Collider>(), false);
        }
    }*/
}

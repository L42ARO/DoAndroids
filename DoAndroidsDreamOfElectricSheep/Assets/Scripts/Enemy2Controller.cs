using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    // Start is called before the first frame update
    float speed=2;
    GameObject Player;
    Rigidbody rb;
    int n = 0;
    Vector3 target;
    bool hit = false;
    float timer = 0;
    public GameObject GUN1;
    public GameObject GUN2;
    public GameObject AIM1;
    public GameObject AIM2;

    void Start()
    {
        //speed = Random.Range(0.5f, 2);
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        timer = 3;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.playerAlive)
        {
            if ((Mathf.Abs(Player.transform.position.y - transform.position.y)) < 2)
            {
                if (timer>=3 && hit)
                {
                    FightClub();
                    timer = 1;
                }
                Predator();
                LookAtPlayer();
                ItFollows();
                CountTime();
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x * 0.05f, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.x * 0.05f);
            }
            PressF();
        }
    }
    void Predator()
    {
        speed = 2;
        target = new Vector3(100, 100, 100);
        if (AttackPlayer.reset)
        {
            for (int g = 0; g < AttackPlayer.corners2.Length; g++)
            {
                Vector3 c = AttackPlayer.corners2[g] - transform.position;
                if (c.magnitude<target.magnitude && !AttackPlayer.chosen[g])
                {
                    target = c;
                    n = g;
                }
            }
            AttackPlayer.chosen[n] = true;
            //print("NEW");
        }
        //if (PlayerController.corners[n] == null) n = 0;
        //print(n+" "+(GameManager.EnemyCount*2));
        
        target = AttackPlayer.corners2[n<(GameManager.EnemyCount+1)?n:0] - transform.position;
        //print(target);
        Debug.DrawRay(transform.position,target,Color.green);
        //Instantiate(GameObject.Find("Cube"), AttackPlayer.corners[n], Quaternion.identity);
        if (target.magnitude < 0.5f) hit = true;
        else hit = false;
    }
    void ItFollows()
    {
        Vector3 movement = new Vector3(target.x * speed * Time.deltaTime, 0, target.z * speed * Time.deltaTime);
        Vector3 newPos = transform.position + movement;
        rb.MovePosition(newPos);
        
        transform.LookAt(new Vector3 (Player.transform.position.x,transform.position.y,Player.transform.position.z));
    }
    public void PressF()
    {
        if (transform.position.y < Player.transform.position.y-10)
        {
            GameManager.EnemyCount-=1;
            //GameManager.instance.changeHud();
            //print("The enemy is falling press F in the chat");
            Destroy(gameObject);
            
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(other.collider, transform.GetComponent<Collider>()) ;
        }
    }
    void FightClub()
    {
        
        GameObject Proyectile1=Instantiate(Resources.Load<GameObject>("Proyectile1"), GUN1.transform.position + new Vector3(0, 0, 0), GUN1.transform.rotation);
        GameObject Proyectile2=Instantiate(Resources.Load<GameObject>("Proyectile1"), GUN2.transform.position + new Vector3(0, 0, 0), GUN2.transform.rotation);
        Proyectile1.GetComponent<ProyectileController1>().enemy2Col = this.gameObject.GetComponent<Collider>();
        Proyectile2.GetComponent<ProyectileController1>().enemy2Col = this.gameObject.GetComponent<Collider>();
    }
    void CountTime()
    {
        if (timer >= 1)
        {
            timer += Time.deltaTime;
        }
    }
    void LookAtPlayer()
    {
        AIM1.transform.LookAt(Player.transform.position);
        AIM2.transform.LookAt(Player.transform.position);
    }
}

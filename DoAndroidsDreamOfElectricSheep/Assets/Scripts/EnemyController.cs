using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    float speed=1.75f;
    GameObject Player;
    Rigidbody rb;
    int n = 0;
    Vector3 target;
    bool hit = false;
    bool iGo = false;
    float timer = 0;
    void Start()
    {
        //speed = Random.Range(0.5f, 2);
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.playerAlive)
        {
            if ((Mathf.Abs(Player.transform.position.y - transform.position.y)) < 2)
            {
                if ((Player.transform.position - transform.position).magnitude < 3f && !hit)
                    iGo = true;
                else speed = 2;
                if (!iGo || Player.GetComponent<PlayerController>().translating)
                    Predator();
                else
                {
                    iGo = true;
                    FightClub();
                }

                ItFollows();
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x*0.05f, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.x * 0.05f);
            }
            PressF();
        }
        CountTime();
    }
    void Predator()
    {
        speed = 1.5f;
        iGo = false;
        target = new Vector3(100, 100, 100);
        if (AttackPlayer.reset)
        {
            for (int g = 0; g < AttackPlayer.corners.Length; g++)
            {
                Vector3 c = AttackPlayer.corners[g] - transform.position;
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
        
        target = AttackPlayer.corners[n<(GameManager.EnemyCount+1)?n:0] - transform.position;
        //print(target);
        Debug.DrawRay(transform.position,target,Color.green);
        //Instantiate(GameObject.Find("Cube"), AttackPlayer.corners[n], Quaternion.identity);
        if (target.magnitude < 0.5f && hit) hit = false;
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
        else if (other.gameObject.tag =="Player" && iGo)
        {
            timer = 1;
            //GameManager.hiting = false;
        }
    }
    void FightClub()
    {
        //GameManager.hiting = true;
        //print("myTurn");
        speed = 15;
        target = (Player.transform.position - transform.position)+transform.forward*2;
    }
    void CountTime()
    {
        if (timer >= 1)
        {
            timer += Time.deltaTime;
            if (timer >= 1.03f)//timer before retreating
            {
                //print("Timer: " + timer);
                timer = 0;
                iGo = false;
                hit = true;
            }
        }
    }
}

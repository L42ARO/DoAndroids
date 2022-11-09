using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Controller : MonoBehaviour
{
    float beamSpeed = 0.5f;
    float flySpeed = 0.01f;
    bool flyActive = false;
    bool targetAquired = false;
    bool vulnerable = false;
    GameObject Player;
    GameObject Animal;
    public GameObject Thrusters;
    public GameObject ChargeUp;
    public GameObject Bubble;
    public GameObject CaptureSpot;
    public GameObject BeamAim;//this has to be manyally done in the unity editor since if we add a GameObject.Find, when there are more than 1 the script will get confused
    public GameObject Ray;
    Rigidbody rb;
    //[SerializeField] private LineRenderer TractorBeam;//this also has to be done manually
    LineRenderer TractorBeam;
    float waiting;
    Quaternion targetRotation;
    Vector3 finalPos;
    Vector3 objective;
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0,20,0);
        TractorBeam = Ray.GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        ChargeUp.GetComponent<Light>().intensity = 0;
        Player = GameObject.Find("Player");
        Animal = GameObject.Find("Animal");
        Bubble.SetActive(false);
        Thrusters.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((GameManager.playerAlive||(!Animal.GetComponent<AnimalController>().free))&&!redDead)
        {//we must check if the player is alive in order to avoid errros when the player dies
            float distToLevel = Animal.transform.position.y - GameManager.level * 30;
            if ((Mathf.Abs(distToLevel) < 5 && distToLevel > 0)||flyActive)//to avoid lasers before the player arrives we add this funciton that checks the distance to the player
            {
                Vector3 distToAnimal = transform.position - Animal.transform.position;
                if (distToAnimal.magnitude > 2.7f)
                {
                    vulnerable = false;
                    takePosition();
                    
                }
                else
                {
                    if (!flyActive)
                    {
                        targetRotation = Quaternion.LookRotation(transform.position - Player.transform.position);
                        
                    }
                    
                    flyActive = true;
                    Animal.GetComponent<AnimalController>().free = false;
                    Bubble.SetActive(false);
                    Ray.SetActive(false);
                    Animal.transform.position = CaptureSpot.transform.position;
                    Animal.transform.rotation = CaptureSpot.transform.rotation;
                    StartCoroutine(FallingWithStlye(1));
                }
            }
            else
            {
                GetComponent<Rigidbody>().useGravity = false;
            }
            if (!Animal.GetComponent<AnimalController>().free && transform.position.y<GameManager.level*30)
            {
                Animal.GetComponent<AnimalController>().free = true;
                Bubble.SetActive(false);
                Ray.SetActive(false);
            }
                
            PressF();
            TargetAquired();
        }
    }
    void takePosition()
    {
        GetComponent<Rigidbody>().useGravity = true;
        transform.LookAt(new Vector3(Animal.transform.position.x, transform.position.y, Animal.transform.position.z));
        if (waiting < 4)
        {
            Thrusters.SetActive(true);
            waiting += Time.deltaTime;
        }
        else
        {
            Thrusters.SetActive(false);
            Ray.SetActive(true);
            EngageTractorBeam();
            Bubble.SetActive(true);
        }
    }
    void EngageTractorBeam()//funciton that makes the lasers go from the tip of the inhibitors to the player
    {
        Animal.GetComponent<AnimalController>().free=false;
        Bubble.transform.position = Animal.transform.position;
        TractorBeam.SetPosition(0, BeamAim.transform.position);
        TractorBeam.SetPosition(1, Animal.transform.position);
        Vector3 beamDist = BeamAim.transform.position - Animal.transform.position;
        Animal.transform.position += beamDist * Time.deltaTime * beamSpeed;
    }
    public IEnumerator FallingWithStlye(float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float factor = t / dur;//use this for lerping
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.25f*Time.deltaTime);
            yield return null;//basically wait for next frame
        }
        vulnerable = true;
        Thrusters.SetActive(true);
        t = 0f;
        while (t < 2.0f)
        {
            //transform.position += Vector3.up * Time.deltaTime * 0.05f;
            ChargeUp.GetComponent<Light>().intensity = 0 + (t / 2.0f);
            t += Time.deltaTime;
            yield return null;
        }
        GetComponent<Rigidbody>().useGravity = false;
        while (transform.position.y<=GameManager.level*30+17)
        {
            objective = new Vector3(transform.position.x, (GameManager.level * 30 + 18), transform.position.z) - transform.position;
            transform.position += objective*Time.deltaTime*flySpeed;
            yield return null;
        }
        finalPos = transform.position;
        
    }

    public void PressF()
    {
        if (transform.position.y < GameManager.level*30 - 10)
        {

            Animal.GetComponent<AnimalController>().free = true;
            GameManager.EnemyCount -= 1;
            //GameManager.instance.changeHud();
            //print("The enemy is falling press F in the chat");
            Destroy(gameObject);

        }
    }
    public void TargetAquired()
    {
        if (transform.position.y > GameManager.level * 30 + 10 && !Animal.GetComponent<AnimalController>().free)
        {
            //print("wtf");
            flySpeed = 0.01f;
            //GetComponent<Rigidbody>().useGravity = false;
            GameManager.playerAlive = false;
        }
    }
    bool redDead=false;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Proyectile" && !redDead && vulnerable)//it shouldn't die twice
        {
            //print("i'm free falling");
            Animal.GetComponent<AnimalController>().free = true;
            redDead = true;//he's dead he can't die again so we check that to avoid reducing the enmy count more than it already is
            GameManager.EnemyCount -= 1;
            //GameManager.instance.changeHud();
            Destroy(gameObject, 0.2f);//we wait the 0.2 seconds so that it coordintates witht he bullet dispearing

        }
    }
}

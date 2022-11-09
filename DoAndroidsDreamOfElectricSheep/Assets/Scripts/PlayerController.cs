using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed=5;
    float aimTime = 1.5f;
    public int shootForce = 5;
    bool pressedShoot;
    bool aiming;
    float timer = 2;
    public bool translating=false;
    Rigidbody rb;
    LineRenderer aimLaser;
    LineRenderer backLaser;
    GameObject Gun;
    GameObject AIM;
    public GameObject arrow;
    public float gunPower;
    [SerializeField] private Animator LaserFade;
    [SerializeField] private Animator backLaserFade;
    private void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        aimLaser = GameObject.Find("Laser").GetComponent<LineRenderer>();
        backLaser = GameObject.Find("BackLaser").GetComponent<LineRenderer>();
        Gun = GameObject.Find("Gun");
        AIM = GameObject.Find("AIM");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WalkHandler();
        //SwipeWalk();
        //SwipeDirection();
        if (!GameManager.instance.win && GameManager.playerAlive)
        {
            ShootHer();
            if (pressedShoot) FollowMouse();
        }
        PressF();
    }
    void WalkHandler()
    {
        float hAxis = Input.GetAxis("Horizontal");//x axis
        float vAxis = Input.GetAxis("Vertical");//z axis
        Vector3 movement = new Vector3(hAxis * walkSpeed * Time.deltaTime, 0, vAxis * walkSpeed * Time.deltaTime);
        Vector3 newPos = transform.position + movement;
        rb.MovePosition(newPos);
    }
    void ShootHer()
    {
            if (Input.GetMouseButton(0))
            {
                LaserFade.SetBool("aim", true);
                backLaserFade.SetBool("aim", true);
                MousePosition();
                FollowMouse();
                BackAIM(gunPower);
                aiming = true;
                gunPower += Time.deltaTime/((aimTime-1)+(Mathf.Pow(3,GameManager.Inhibitors)));//since any number elvevated to 0 is 1, the quantity we need without inhibitors is actually 1.75 so we rest 1 to the time we wanted
                gunPower = gunPower > 1 ? 1 : gunPower;
                //print(gunPower);
                //pressedShoot = true;
            }
            else if (timer > 0.5f && aiming)
            {
                rb.AddForce(-transform.forward * (60*gunPower), ForceMode.Impulse);
                aiming = false;
                translating = true;
                LaserFade.SetBool("aim", false);
                LaserFade.SetBool("fade", true);
                backLaserFade.SetBool("aim", false);
                backLaserFade.SetBool("fade", true);
                arrow.SetActive(false);
                pressedShoot = false;
                Instantiate(Resources.Load<GameObject>("Proyectile"), Gun.transform.position + new Vector3(0, 0, 0), Gun.transform.rotation);
                timer = 0;
                gunPower = 0;
            }
            /*else {
                pressedShoot = false;
            }*/
            if (timer >= 0.1f) 
            {
                backLaserFade.SetBool("fade", false);
                LaserFade.SetBool("fade", false);
                translating = false;
            } 
            timer += Time.deltaTime;
    }
    Vector3 hitPoint;
    public void MousePosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane(Vector3.up, transform.position);
        if (p.Raycast(mouseRay, out float hitDist))
        {
            hitPoint = mouseRay.GetPoint(hitDist);
        }
    }
    public void FollowMouse()
    {
        transform.LookAt(hitPoint);
        Vector3 hitPoint2 = new Vector3(hitPoint.x, AIM.transform.position.y, hitPoint.z);
        AIM.transform.LookAt(hitPoint2);
        aimLaser.SetPosition(0, AIM.transform.position);
        aimLaser.SetPosition(1, hitPoint2);
    }
    void BackAIM(float magnitude)
    {
        Vector3 backVector = transform.position - transform.forward*(5*magnitude)+new Vector3(0, 0.5f, 0);
        backLaser.SetPosition(1, backVector);
        backLaser.SetPosition(0, transform.position+new Vector3(0,0.5f,0));
        arrow.SetActive(true);
        arrow.transform.position = backVector;
    }
    void PressF()
    {
        if ((transform.position.y < ((GameManager.level-GameManager.devLevel)*30)-5) && !GameManager.instance.win)
        {
            GameManager.playerAlive = false;
            //print("Press F in the chat for yourself");
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //transform.position = transform.position;
            //Destroy(gameObject);
        }
    }
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public Vector3 SwipeDirection()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                currentSwipe.Normalize();
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    print("Up");
                    return new Vector3(0,0,1);
                }
                else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    print("Down");
                    return new Vector3(0, 0, -1);
                }
                else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    print("Left");
                    return new Vector3(-1, 0, 0);
                }
                else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    print("Right");
                    return new Vector3(1, 0, 0);
                }
                else {
                    return new Vector3(0,0,0);
                }
                
            }
            else return new Vector3(0, 0, 0);

        }
        else return new Vector3(0, 0, 0);
    }
    Vector3 currentDirection = new Vector3(0, 0, 0);
    bool moving = false;
    float moveTimer = 0;
    void SwipeWalk()
    {
        if (!moving)
        {
            currentDirection = SwipeDirection();
            if (currentDirection.magnitude > 0)
            {
                moving = true;
                moveTimer = 0;
            }
        }
        else
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > 0.2f) moving = false;
        }
        Vector3 newPos = transform.position + (currentDirection * Time.deltaTime * walkSpeed);
        rb.MovePosition(newPos);
        
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Proyectile")
        {
            Physics.IgnoreCollision(other.collider, transform.GetComponent<Collider>(),true);
        }
    }
}

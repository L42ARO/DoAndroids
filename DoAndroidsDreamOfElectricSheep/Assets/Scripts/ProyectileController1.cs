using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileController1 : MonoBehaviour
{
    float speed=15;
    Rigidbody rb;
    public Collider enemy2Col;
    private void Awake()
    {
        /*speed = 15*GameObject.Find("Player").GetComponent<PlayerController>().gunPower + 15;
        enemy2Col = GameObject.Find("Player").GetComponent<Collider>();*/
        rb = gameObject.GetComponent<Rigidbody>();

    }
    private void Start()
    {
        Physics.IgnoreCollision(enemy2Col, transform.GetComponent<Collider>());
    }
    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
        if (Mathf.Abs(transform.position.x) >20  || Mathf.Abs(transform.position.z) > 20) Destroy(gameObject);
        Physics.IgnoreCollision(enemy2Col, transform.GetComponent<Collider>());
    }
    void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "Proyectile" || other.gameObject.tag=="Proyectile Enemy" || other.gameObject.tag=="Enemy")//chekcing wether it's a proyecitle or an anmey should always go first
        {
            Physics.IgnoreCollision(other.collider, transform.GetComponent<Collider>());
        }
        else if (other.gameObject.tag != "Enemy")
        {
            Destroy(gameObject, 0.2f);
        }

    }
}

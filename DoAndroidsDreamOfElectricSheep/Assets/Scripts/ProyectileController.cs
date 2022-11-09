using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileController : MonoBehaviour
{
    float speed=15;
    Rigidbody rb;
    Collider playerCol;
    private void Awake()
    {
        speed = 15*GameObject.Find("Player").GetComponent<PlayerController>().gunPower + 15;
        playerCol = GameObject.Find("Player").GetComponent<Collider>();
        rb = gameObject.GetComponent<Rigidbody>();
        Physics.IgnoreCollision(playerCol, transform.GetComponent<Collider>());
    }
    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
        if (Mathf.Abs(transform.position.x) >20  || Mathf.Abs(transform.position.z) > 20) Destroy(gameObject);
        if (GameManager.playerAlive)Physics.IgnoreCollision(playerCol, transform.GetComponent<Collider>());
    }
    void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "Proyectile" || other.gameObject.tag == "Proyectile Enemy")
        {
            Physics.IgnoreCollision(other.collider, transform.GetComponent<Collider>());
        }
        if (other.gameObject.tag != "Player")
        {
            Destroy(gameObject, (other.gameObject.CompareTag("Enemy")?0.4f:0.001f));
        }
        

    }
}

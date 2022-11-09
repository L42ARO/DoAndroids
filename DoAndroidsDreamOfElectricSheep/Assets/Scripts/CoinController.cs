using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float[] rotationAxes;
    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        rotationAxes = new float[3];
        for(int i=0; i < 3; i++)
        {
            rotationAxes[i] = Random.Range(-0.5f, 0.5f);
        }
    }
    private void FixedUpdate()
    {
        transform.Rotate(rotationAxes[0],rotationAxes[1],rotationAxes[2], Space.World);
        if (!Physics.Raycast(transform.position, Vector3.down,5.0f))
        {
            StartCoroutine(CoinFalling());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            int gained = Random.Range(1,6);//coins will be changed to USB's holding Crypto, therefore the currency found should be random
            //We are pending finding some way to signal the user the new quantity found, remeber it will tacke animation again.
            GameManager.instance.CoinGained(gained);
            Destroy(gameObject);
        }
    }
    public IEnumerator CoinFalling()
    {
        float t = 0;
        while (t < 2)
        {
            t += Time.deltaTime;
            yield return null;
        }
        //GetComponent<Collider>().isTrigger = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
}

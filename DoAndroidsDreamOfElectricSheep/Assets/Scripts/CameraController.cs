using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float speed = 8;
    float speed2 = 0.25f;
    bool go = false;
    float target;
    Vector3 direction;
    GameObject Player;
    GameObject Animal;
    private void Start()
    {
        target = transform.transform.position.y;
        direction = transform.position;
        Player = GameObject.Find("Player");
        Animal = GameObject.Find("Animal");
    }
    // Update is called once per frame
    void Update()
    {
        if (!go)
        {
            if (GameManager.instance.win)
            {
                go = true;
                target += 30;
                direction.y = target;
            }
            else
            {
                Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 4, Time.deltaTime * speed2);
                if (GameManager.playerAlive) Camera.main.transform.LookAt(Player.transform.position);
                else StartCoroutine(LookAtAnimal(10));
            }
        }
        
        if (transform.position.y<target && go)
        {
            transform.position += Vector3.up*Time.deltaTime*speed;
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 25, Time.deltaTime * speed);
        }
        else if(transform.position.y>=target && go)
        {
            go = false;
        }
    }
    Vector3 finalPos;
    public IEnumerator LookAtAnimal(float dur)
    {
        float t = 0f;
        while ((Animal.transform.position-Camera.main.transform.position).magnitude>10)
        {
            t += Time.deltaTime;
            float factor = t / dur;//use this for lerping

            Quaternion targetRotation = Quaternion.LookRotation(Animal.transform.position - Camera.main.transform.position);
            // Smoothly rotate towards the target point.
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 60, Time.deltaTime * 0.5f);
            Camera.main.transform.position += (Animal.transform.position - Camera.main.transform.position) * Time.deltaTime * 0.05f;
            Camera.main.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
            finalPos = transform.position;
            yield return null;//basically wait for next frame
        }
    }
}

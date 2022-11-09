using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalScene : MonoBehaviour
{
    float speed = 8;
    bool go = false;
    float target;
    Vector3 direction;
    private void Start()
    {
        target = transform.transform.position.y;
        direction = transform.position;
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
        }
        if (transform.position.y < target && go)
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
        }
        else if (transform.position.y >= target && go)
        {
            go = false;
        }
    }
}

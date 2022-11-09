using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorCameraController : MonoBehaviour
{
    Vector3 dir = new Vector3(0, 0, 0);
    Vector3 newPos;
    Vector3 permDir;
    Vector3 newCat;

    private void Start()
    {
        newPos = transform.position;
        newCat = transform.position;
    }
    private void Update()
    {
        CheckNextPos();
        CheckNextCategory();

    }
    public void NextPos(Vector3 nP)
    { 
        newPos = new Vector3(nP.x, transform.position.y, transform.position.z);
        permDir = newPos - transform.position;
    }
    public void CheckNextPos()
    {
        if (transform.position.x != newPos.x)
        {
            dir = newPos - transform.position;
            if ((permDir.x > 0 && dir.x > 0.02) || (permDir.x < 0 && dir.x < -0.02))
            {
                transform.position += permDir * Time.deltaTime * ((5 * (dir.magnitude / permDir.magnitude)) + 0.25f);//this last part assures a dynamic velocity that starts at 5 and finishes at 0.25
            }
            else
            {
                transform.position = newPos;
            }
        }
    }
    public void NextCategory(Vector3 nC)
    {
        
        newCat = new Vector3(transform.position.x,nC.y, transform.position.z);
        permDir = newCat - transform.position;
        print(permDir);
        
    }
    public void CheckNextCategory()
    {
        if (transform.position.y != newCat.y)
        {
            dir = newCat - transform.position;
            if ((permDir.y > 0 && dir.y > 0.02) || (permDir.y < 0 && dir.y < -0.02))
            {
                transform.position += permDir * Time.deltaTime * ((5 * (dir.magnitude / permDir.magnitude)) + 0.25f);//this last part assures a dynamic velocity that starts at 5 and finishes at 0.25
            }
            else
            {
                transform.position = newCat;
            }
        }
    }
    IEnumerator slowlyMove()
    {
        while (dir.magnitude > 0.01f)
        {
            transform.position += dir * Time.deltaTime * 0.5f;
            dir = newPos - transform.position;
        }
        yield return null;
    }
}

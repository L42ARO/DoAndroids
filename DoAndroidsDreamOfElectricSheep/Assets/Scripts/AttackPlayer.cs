using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    int prevEnemyCount = 0;
    //GameObject 
    public static int enemyCount = 0;
    public static bool[] chosen;
    public static Vector3[] corners;
    public static Vector3[] corners2;
    void Start()
    {
        CornerUpdate(0);   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CornerUpdate(0);
    }

    public static bool reset = true;
    float range = 360;
    float angle = 0;
    public void CornerUpdate(int o = 0)
    {

        enemyCount = GameManager.EnemyCount;
        if (enemyCount != prevEnemyCount || GameManager.instance.Platform[0].GetComponent<PlatformController>().ZoneChange())//PlatformController.ZoneChange()
        {
            //o = 1;
            //attack angle change
            range = GameManager.instance.Platform[0].GetComponent<PlatformController>().ZoneCheck(1);//PlatformController.ZoneCheck(1);// the 1 is for the function to return the total angle and not only the starting angle
            angle = GameManager.instance.Platform[0].GetComponent<PlatformController>().ZoneCheck(0);//PlatformController.ZoneCheck(0);// the 0 is for the function to return the resulting angle
            reset = true;
            prevEnemyCount = enemyCount;
            chosen = new bool[enemyCount+1];
            for (int i = 0; i < chosen.Length; i++)
                chosen[i] = false;
        }
        else
        {
            reset = false;
            //o = 0;
        }
        float division = range / (enemyCount != 0 ? (enemyCount+1) : 2);
        corners = new Vector3[(enemyCount != 0 ? (enemyCount+1) : 2)];
        corners2 = new Vector3[(enemyCount != 0 ? (enemyCount + 1) : 2)];
        float newAngle = angle;

        for (int u = 0; u < corners.Length; u++)
        {
            corners[u] = transform.position + GameManager.PolToCart(newAngle, 2.5f);
            corners2[u] = transform.position + GameManager.PolToCart(newAngle, 5f);
            //print(angle);
            if (o==1) Instantiate(GameObject.Find("GridRef"), corners2[u], Quaternion.identity);
            newAngle += division;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    // Start is called before the first frame update
    float shrinkSpeed = 0.7f;
    public /*static*/ Vector3[,] theGrid;
    public /*static*/ int[,,] window = new int[,,] { /*0*/{ { 0, 90 }, { 0, 180 }, { 90, 90 } },/*1*/{ { 270, 180 }, { 0, 360 }, { 90, 180 } },/*2*/{ { 270, 90 }, { 180, 180 }, { 180, 90 } } };
    public /*static*/ GameObject Player;
    GameObject[] GridRef = new GameObject[16];
    public float elapsed;
    Vector3 s;
    private void Awake()
    {
        if (GetComponent<PlatformController>().enabled)
        {
            elapsed = 0;
            Player = GameObject.Find("Player");
            //print("Hello");
            TheGrid();
            s = transform.localScale;

        }
    }
    void Update()
    {
        ShrunkTheKids();
        TheGrid();
        IgnoreElevator();
    }
    void ShrunkTheKids()
    {
        if (GameManager.playerAlive)
        {
            
            if ((transform.localScale.x >= 1 || transform.localScale.z >= 1) && !GameManager.stopPlatform && transform.position.y < (Player.transform.position.y - 0.5f)&& Mathf.Abs(transform.position.y-Player.transform.position.y)<5)
            {
                transform.localScale = new Vector3(s.x - 0.9f*(elapsed+Time.deltaTime), s.y, s.z - 0.9f * (elapsed + Time.deltaTime)); 
                //print((0.2f + Mathf.Pow(2, (elapsed * -1))));
                //transform.localScale = new Vector3(s.x - 1 * shrinkSpeed * Time.deltaTime, s.y, s.z - 1 * shrinkSpeed * Time.deltaTime);
                elapsed += Time.deltaTime;
            }
        }
    }
    public void TheGrid(int o = 0)
    {
        Vector3 pS = GetComponent<Collider>().bounds.size;
        theGrid = new Vector3[4, 4];
        float ux = pS.x / 3; float uz = pS.z / 3;
        float cz = 0;
        int gridRefCount = 0;
        for (int h = 0; h < 4; h++)
        {
            float cx = 0;
            for (int i = 0; i < 4; i++)
            {
                theGrid[h, i] = new Vector3(-pS.x / 2 + cx, transform.position.y, -pS.z / 2 + cz);
                if (o == 1)
                {
                    GridRef[gridRefCount]=Instantiate(Resources.Load<GameObject>("GridRef"), theGrid[h, i] + new Vector3(0, 2, 0), GameObject.Find("GridRef").GetComponent<Transform>().rotation);
                    gridRefCount++;
                }
                else if (o == 2)
                {
                    GridRef[gridRefCount].transform.position = theGrid[h, i] + new Vector3(0, 2, 0);
                    gridRefCount++;
                }
                cx += ux;
            }
            cz += uz;
        }

    }
    public /*static*/float ZoneCheck(int r)
    {
        int x = PlayerZone()[0];
        int y = PlayerZone()[1];

        return window[y, x, r];
    }
    public /*static*/ int px = -1; public /*static*/ int py = -1;
    public /*static*/ bool ZoneChange()
    {
        int x = PlayerZone()[0];
        int y = PlayerZone()[1];
        if (px != x || py != y /*|| GameManager.win*/)
        {
            //print("Zone: " + y + "," + x);
            //print("NEW STRT ANGLE: " + window[y, x, 0] + "NEW TOTAL ANGLE:" + window[y, x, 1]);
            px = x;
            py = y;
            return true;
        }
        else if (GameManager.instance.win) return true;
        else return false;
    }
    int[,] zoneQuant = new int[,] { {0,0,0},{0,0,0},{0,0,0} };
    public Vector3 ZoneInstantiation(float Fy = 0, float margin = 1, int objValue = 1)
    {
        int[] pZone = PlayerZone();
        //print("player at:" + pZone[0] + ", " + pZone[1]);
        int x; int y;
        do
        {
            x = Random.Range(0, 3);
            y = Random.Range(0, 3);
        } while (x == pZone[0] && y == pZone[1]);
        for (int i=0; i<3; i++)
        {
            for (int h = 0; h < 3; h++)
            {
                if (zoneQuant[x, y] > zoneQuant[i, h] && (i != pZone[0] || h != pZone[1]))
                {
                    x = i;
                    y = h;
                }
            }
        }

        zoneQuant[x, y]+=objValue;
        Vector3[] limits = ZoneEdges(x,y);
        float Fx = Random.Range(limits[0].x+margin, limits[1].x-margin); // The +1 and -1 is so the enemies aren't created on the borders
        float Fz = Random.Range(limits[0].z+margin, limits[2].z-margin);
        string quantityCheck = "";
        for(int n = 0; n < 3; n++)
        {
            quantityCheck += " | ";
            for (int m = 0; m < 3; m++)
            {
                quantityCheck += zoneQuant[n, m]+",";
            }
        }
        //print(quantityCheck);
        return new Vector3(Fx, Fy, Fz);
        
    }
    public int[] PlayerZone()
    {
        int x = 2; int y = 2;
        for (int i = 1; i < 3; i++)
            if (Player.transform.position.x < theGrid[0, i].x)
            {
                x = i - 1;
                break;
            }
        for (int i = 1; i < 3; i++)
            if (Player.transform.position.z < theGrid[i, 0].z)
            {
                y = i - 1;
                break;
            }
        return new int [2]{x,y};
    }
    public Vector3[] ZoneEdges(int x, int y)
    {
        return new Vector3[4]{theGrid[x,y],theGrid[x, y+1],theGrid[x+1, y],theGrid[x+1, y+1] };
    }
    private void IgnoreElevator()
    {
        if (GameManager.playerAlive)
        {
            if (Player.transform.position.y - 0.5f < transform.position.y)
                Physics.IgnoreCollision(Player.GetComponent<Collider>(), transform.GetComponent<Collider>());
            else if (Player.transform.position.y - 0.5f > transform.position.y)
                Physics.IgnoreCollision(Player.GetComponent<Collider>(), transform.GetComponent<Collider>(), false);
        }
        
    }
}

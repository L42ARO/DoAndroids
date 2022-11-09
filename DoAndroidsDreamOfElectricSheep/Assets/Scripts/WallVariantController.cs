using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class WallVariantController : MonoBehaviour
{
    public GameObject[] commercialType= new GameObject [3];
    public GameObject[] wallType = new GameObject [2];
    public GameObject[] commercialsT1 = new GameObject[2];
    public GameObject[] commercialsT2 = new GameObject[2];


    void Start()
    {
        //make inactive all objects in the wall to only select certain ones
        for(int x = 0; x < wallType.Length; x++)
            wallType[x].SetActive(false);

        for (int x = 0; x < commercialType.Length; x++)
            commercialType[x].SetActive(false);
        for (int x = 0; x < commercialsT1.Length; x++)
            commercialsT1[x].SetActive(false);
        for (int x = 0; x < commercialsT2.Length; x++)
            commercialsT2[x].SetActive(false);
        RepAvoidance(GameManager.instance.wallHistory);
        GameManager.instance.wallHistory=updateWHArray(GameManager.instance.wallHistory);

    }

    int wTNum, cTNum, cNum =0;
    public void RepAvoidance(string[] wallHist)
    {
        //NUMBER ASSIGN PHASE
        wTNum = Random.Range(0, wallType.Length);
        cTNum = wTNum!=0?0:Random.Range(0, commercialType.Length);
        if (cTNum==1)
        {
            cNum = Random.Range(0, commercialsT1.Length);
        }
        else if (cTNum == 2)
        {
            cNum = Random.Range(0, commercialsT2.Length);
        }
        string currentWall = ""+wTNum + cTNum + cNum;
        for(int x=0; x < wallHist.Length; x++)
        {
            if (wallHist[x] == currentWall)
            {
                wTNum = 0; cTNum = 0; cNum = 0;
                break;
            }
        }
        print("" + wTNum + cTNum + cNum);
        //INSTANTIATION PHASE
        wallType[wTNum].SetActive(true);
        commercialType[cTNum].SetActive(true);
        if (cTNum == 1)
        {
            commercialsT1[cNum].SetActive(true);
        }
        else if (cTNum == 2)
        {
            commercialsT2[cNum].SetActive(true);
        }

    }
    public string [] updateWHArray(string[] wallHist)
    {
        int len = wallHist.Length;
        if(len==0){
            return new string[6] { "000", "000", "000", "000", "000", "000" };
        }
        string[] updatedWallHist = new string[len];
        for(int x = 1; x < len; x++)
        {
            updatedWallHist[x - 1] = wallHist[x];
        }
        updatedWallHist[(len==0)?0:len-1]= "" + wTNum + cTNum + cNum;
        print(string.Join(",",updatedWallHist));
        return updatedWallHist;
    }
}

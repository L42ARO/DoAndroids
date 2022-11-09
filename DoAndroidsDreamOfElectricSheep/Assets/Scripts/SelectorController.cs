using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorController : MonoBehaviour
{
    public int[] presentSelection = {0,0};
    GameObject PrevButton;
    GameObject NextButton;
    GameObject SelectButton;
    GameObject AvaiabilityText;
    public bool[] owned = {true,false,false};
    public int[] prices = { 0, 50, 100};

    private void Start()
    {
        PrevButton = GameObject.Find("PrevButton");
        NextButton = GameObject.Find("NextButton");
        SelectButton = GameObject.Find("SelectButton");
        AvaiabilityText = GameObject.Find("AvaiabilityText");
        CheckLimits();
        ChangeAvailability();
    }
    public void CheckLimits()
    {
        bool[] buttonState = { true , true};
        Vector3[] buttonPos = { PrevButton.transform.position, NextButton.transform.position };
        if(presentSelection[0] == 0)
        {
            buttonState[0] = false;
            buttonPos[0] = new Vector3(0, -538, 0);
            
        }
        if (presentSelection[0] == 2)
        {
            buttonState[1] = false;
            buttonPos[1] = new Vector3(0, -538, 0);
        }
        PrevButton.SetActive(buttonState[0]);
        NextButton.SetActive(buttonState[1]);
    }
    public void ChangeAvailability()
    {
        AvaiabilityText.GetComponent<Text>().text = owned[presentSelection[0]] ? "PLAY" : (prices[presentSelection[0]].ToString()+"$");
        SelectButton.GetComponent<Image>().color = owned[presentSelection[0]] ? Color.green: Color.red;
    }
    public void ChangeButton(int change_)
    {
        Vector3 currentPos = new Vector3 (5.5f * (presentSelection[0]), 0, 0);
        Vector3 nextOption =  currentPos+ new Vector3(5.5f, 0, 0)*change_;
        Camera.main.GetComponent<SelectorCameraController>().NextPos(nextOption);
        presentSelection[0]+=change_;
        CheckLimits();
        ChangeAvailability();
    }
    public void ChangeCategory(int change_)
    {
        Vector3 currentPos = new Vector3(5.5f * (presentSelection[0]), 0, 0);
        Vector3 nextPos = new Vector3(5.5f*(presentSelection[1]), (-8 * change_)+ 2.71f, 0);
        Camera.main.GetComponent<SelectorCameraController>().NextCategory(nextPos);
        //Camera.main.GetComponent<SelectorCameraController>().NextPos(nextPos);
    }
    public void SelectOutfit()
    {
        owned[presentSelection[0]] = true;
        ChangeAvailability();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OthelloInput : MonoBehaviour
{
    public bool touchUse = false;
    private RaycastHit RayHit;
    OthelloGameController _OhelloGameController;


    void Update()
    {
        if (!touchUse)
        {
            GetInputByMousePos();
        }
        else
        {
            GetInputByTouchPos();
        }
    }
    void Start()
    {
           _OhelloGameController = GameObject.Find("OthelloGameController").GetComponent<OthelloGameController>();
    
    }


    void GetInputByMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

       
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                if (hit.collider.tag == "BoardBluck")
                {
                    OthelloBoardBluck bluck = hit.collider.gameObject.GetComponent<OthelloBoardBluck>();

                    _OhelloGameController.GetInputOn(bluck);
                }


            }
        }
    }
    void GetInputByTouchPos()
    {
        Touch touch = Input.GetTouch(0);
        if ((Input.touchCount>0)&&(touch.phase==TouchPhase.Began))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;





            if (Physics.Raycast(ray, out hit, 10000f))
            {
                if (hit.collider.tag == "BoardBluck")
            {
                    OthelloBoardBluck bluck = hit.collider.gameObject.GetComponent<OthelloBoardBluck>();
                    _OhelloGameController.GetInputOn(bluck);
                }


            }





        }

        new WaitForSeconds(.1f); //0.1초마다 호출
    }








}



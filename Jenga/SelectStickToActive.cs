using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStickToActive : MonoBehaviour
{
    public GameObject LeftStick;
    public GameObject RightStick;
    public GameObject MiddleStick;

    private int ChooseStick;
    private Builder GameController;

    // Start is called before the first frame update
    void Start()
    {
        ChooseStick = 0;
        GameController = FindObjectOfType<Builder>();
    }

    /// <summary>
    /// 배치되었지만 비활성화된 스틱을 활성화함
    /// </summary>
    public void PlaceStick()
    {
        switch (ChooseStick)
        {
            case 0:
                MiddleStick.GetComponent<ActiveStick>().ActiveStickOnTop();
                ChooseStick++;
                break;
            case 1:
                LeftStick.GetComponent<ActiveStick>().ActiveStickOnTop();
                ChooseStick++;
                break;
            case 2:
                RightStick.GetComponent<ActiveStick>().ActiveStickOnTop();
                GameController.MakeNewLayer();
                break;
        }
    }

    /// <summary>
    /// 스틱이 마우스 레이캐스트에 영향을 받게함. 전까진 플레이어가 
    /// 건드릴 수 없음. gameObject.SetActive()를 하려면 PlaceStick()사용
    /// </summary>
    public void ActiveStickToRaycast()
    {
        MiddleStick.tag = "Stick";
        LeftStick.tag = "Stick";
        RightStick.tag = "Stick";
    }

    /// <summary>
    /// 마우스 레이캐스트 영향 비활성화
    /// </summary>
    public void DeactiveStickToRaycast()
    {
        MiddleStick.tag = "TStick";
        LeftStick.tag = "TStick";
        RightStick.tag = "TStick";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//자이로센서를 이용해서 스트라이커를 움직이는 클래스
public class GyroMove : MonoBehaviourPunCallbacks
{
    public float Speed = 4.0f;
    public float DeadzoneX = 0.05f;
    public float DeadzoneY = 0.03f;

    private Quaternion DeviceRotation;
    private Vector3 AnglesVector;
    private Rigidbody Rb;
    private bool ShouldInit = true;

    private float InitX; // 회전
    private float InitY; // 앞뒤
    //private float InitZ; // 좌우
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (!Input.gyro.enabled)
        {
            if(!ShouldInit) ShouldInit = true;
            return;
        }

        if (ShouldInit) InitXYZ();

        MoveStriker();
    }  

    private void InitXYZ()
    {
        InitX = Input.gyro.attitude.x;
        InitY = Input.gyro.attitude.y;
        //InitZ = Input.gyro.attitude.z;
        ShouldInit = false;
    }

    private void MoveStriker()
    {
        Vector3 StrikerSpeed = new Vector3(0, 0, 0);
        
        if(Input.gyro.attitude.x - InitX > DeadzoneX)
        {
            StrikerSpeed.x = PhotonNetwork.IsMasterClient?-Speed:Speed;
        }
        else if(Input.gyro.attitude.x - InitX < -DeadzoneX)
        {
            StrikerSpeed.x = PhotonNetwork.IsMasterClient ? Speed : -Speed;
        }

        if(Input.gyro.attitude.y - InitY < -DeadzoneY)
        {
            StrikerSpeed.z = PhotonNetwork.IsMasterClient ? Speed : -Speed;
        }
        else if (Input.gyro.attitude.y - InitY > DeadzoneY)
        {
            StrikerSpeed.z = PhotonNetwork.IsMasterClient ? -Speed : Speed;
        }
                
        Rb.velocity = StrikerSpeed;
    }
}
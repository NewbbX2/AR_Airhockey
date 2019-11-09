using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//자이로센서를 이용해서 스트라이커를 움직이는 클래스
public class GyroMove : MonoBehaviourPunCallbacks
{
    public float Speed = 4.0f;
    public float DeadzoneX = 5.0f;
    public float DeadzoneY = 3.0f;

    private Quaternion DeviceRotation;
    private Vector3 AnglesVector;
    private Rigidbody Rb;
    private bool ShouldInit = true;

    private float InitZ; // 회전
    private float InitY; // 앞뒤
                         //private float InitZ; // 좌우

    void Awake()
    {
        Input.gyro.enabled = true;
    }
    void Start()
    {
        Input.gyro.enabled = false;
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
        InitZ = Input.gyro.attitude.eulerAngles.z;
        InitY = Input.gyro.attitude.eulerAngles.y;
        //InitZ = Input.gyro.attitude.z;
        ShouldInit = false;
    }

    private void MoveStriker()
    {
        Vector3 StrikerSpeed = new Vector3(0, 0, 0);
        
        if(Input.gyro.attitude.eulerAngles.z - InitZ > DeadzoneX)
        {
            StrikerSpeed.x = PhotonNetwork.IsMasterClient? -Speed : Speed;
        }
        else if(Input.gyro.attitude.eulerAngles.z - InitZ < -DeadzoneX)
        {
            StrikerSpeed.x = PhotonNetwork.IsMasterClient ? Speed : -Speed;
        }

        if(Input.gyro.attitude.eulerAngles.y - InitY < -DeadzoneY)
        {
            StrikerSpeed.z = PhotonNetwork.IsMasterClient ? Speed : -Speed;
        }
        else if (Input.gyro.attitude.eulerAngles.y - InitY > DeadzoneY)
        {
            StrikerSpeed.z = PhotonNetwork.IsMasterClient ? -Speed : Speed;
        }

        Debug.Log(string.Format("Init Z : " + InitZ));
        Debug.Log(string.Format("Init Y : " + InitY));
        Debug.Log(string.Format("Y : " + Input.gyro.attitude.eulerAngles.y));
        Debug.Log(string.Format("Z : " + Input.gyro.attitude.eulerAngles.z));
        Debug.Log(string.Format("Speed : " + Rb.velocity));
        Rb.velocity = StrikerSpeed;
    }
}
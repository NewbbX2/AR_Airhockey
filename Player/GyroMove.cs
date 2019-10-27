using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroMove : MonoBehaviour
{
    //초기 회전각을 저장할 변수
    private float initialOrientationX;
    private float initialOrientationY;
    private float initialOrientationZ;

    private Rigidbody Rb;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;

        //게임 시작시의 초기값 저장
        initialOrientationX = Input.gyro.rotationRateUnbiased.x;
        initialOrientationY = Input.gyro.rotationRateUnbiased.y;
        initialOrientationZ = -Input.gyro.rotationRateUnbiased.z;
    }

    // Update is called once per frame
    void Update() 
    {
        Quaternion deviceRotation = GetDeviceRotation();
        Vector3 StrikerSpeed = deviceRotation.eulerAngles;
        Rb.velocity = new Vector3(StrikerSpeed.x, 0, StrikerSpeed.z);   

        //transform.rotation = deviceRotation;
        //gameObject.transform.Rotate(0, initialOrientationY - Input.gyro.rotationRateUnbiased.y, 0);
    }

    private static Quaternion GetDeviceRotation()
    {
        return HasGyroscope ? ReadGyroscopeRotation() : Quaternion.identity;
    }

    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// 핸드폰을 세웠을 때 기준으로 했음. 앞뒤 좌우 교체는 89줄에서 바꾸면됨. 아예 눕혔을 때 방향은 64줄 x자리에 y
/// 기본 중립 모양을 바꾸고 싶으면 64번줄 float상수 변경
public class GyroMove : MonoBehaviour
{
    public Camera ARCamera;
    public Quaternion deviceRotation;
    public GameObject RetationView;
    public Vector3 StrikerSpeed;

    //초기 회전각을 저장할 변수
    private float initialOrientationX;
    private float initialOrientationY;
    private float initialOrientationZ;

    private float InitGyroscopeAttitudeX;
    private float InitGyroscopeAttitudeY;
    private float InitGyroscopeAttitudeZ;

    private Rigidbody Rb;

    private float FM = 30;//force multipiler

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;

#if !UNITY_EDITOR
        deviceRotation = GetDeviceRotation();
#else
        deviceRotation = RetationView.transform.rotation;
#endif

        //게임 시작시의 초기값 저장
        initialOrientationX = deviceRotation.x;//Input.gyro.rotationRateUnbiased.x;
        initialOrientationY = deviceRotation.y;
        initialOrientationZ = deviceRotation.z;

        InitGyroscopeAttitudeX = Input.gyro.rotationRateUnbiased.x;
        InitGyroscopeAttitudeY = Input.gyro.rotationRateUnbiased.y;
        InitGyroscopeAttitudeZ = Input.gyro.rotationRateUnbiased.z;

        //MakeContentAppearAt(Transform content, Vector3 position, Quaternion rotation);
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        deviceRotation = GetDeviceRotation();
        RetationView.transform.rotation = DeviceRotationToEditorRotation(GetDeviceRotation());
#else
        deviceRotation = RetationView.transform.rotation;
#endif
        //AlterGyroscopeAttitude();
        AnglToVelo(deviceRotation);
        Rb.velocity = new Vector3(StrikerSpeed.x / 10, 0, StrikerSpeed.z / 10 - 10f);

        //transform.rotation = deviceRotation;
        //gameObject.transform.Rotate(0, initialOrientationY - Input.gyro.rotationRateUnbiased.y, 0);
    }

    private void AnglToVelo(Quaternion dR)
    {
        Vector3 TranslatedAngles = deviceRotation.eulerAngles;

        if (TranslatedAngles.x > 180)
        {
            TranslatedAngles.x = TranslatedAngles.x - 360;
        }
        if (TranslatedAngles.y > 180)
        {
            TranslatedAngles.y = TranslatedAngles.y - 360;
        }
        if (TranslatedAngles.z > 180)
        {
            TranslatedAngles.z = TranslatedAngles.z - 360;
        }
        StrikerSpeed = TranslatedAngles;

        //StrikerSpeed = new Vector3(TranslatedAngles.y-initialOrientationY, TranslatedAngles.x- initialOrientationX, TranslatedAngles.z- initialOrientationZ);
        StrikerSpeed = new Vector3(-(TranslatedAngles.z), TranslatedAngles.y, (TranslatedAngles.x));
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

    private void AlterGyroscopeAttitude()
    {
        //if(Input.gyro.rotationRateUnbiased.x> InitGyroscopeAttitudeX)
        if (Mathf.Abs(deviceRotation.x - initialOrientationX) > 0.1)
        {
            if (deviceRotation.x > initialOrientationX)
            {
                Rb.AddForce(Vector3.forward * FM);
            }
            else
            {
                Rb.AddForce(Vector3.back * FM);
            }
        }
        if (Mathf.Abs(deviceRotation.y - initialOrientationY) > 0.1)
        {
            if (deviceRotation.y > initialOrientationY)
            {
                Rb.AddForce(Vector3.right * FM);
            }
            else
            {
                Rb.AddForce(Vector3.left * FM);
            }
        }
    }

    private Quaternion DeviceRotationToEditorRotation(Quaternion DeviceRotation)
    {
        //핸드폰으로 똑바로 세웠을 때
        //좌우 기울이기 일치 return new Quaternion(DeviceRotation.y, DeviceRotation.x, DeviceRotation.z, DeviceRotation.w);
        //축회전은 일치return new Quaternion(DeviceRotation.z, DeviceRotation.y, DeviceRotation.x, DeviceRotation.w);
        //앞뒤 기울이기 일치return new Quaternion(DeviceRotation.x, DeviceRotation.z, DeviceRotation.y, DeviceRotation.w);
        //좌우 반전 return new Quaternion(DeviceRotation.y, DeviceRotation.z, DeviceRotation.x, DeviceRotation.w);
        //앞뒤 기울이기 일치return new Quaternion(DeviceRotation.z, DeviceRotation.x, DeviceRotation.y, DeviceRotation.w);
        return new Quaternion(DeviceRotation.w, DeviceRotation.z, DeviceRotation.y, DeviceRotation.x);
    }
}

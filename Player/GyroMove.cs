using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// 핸드폰을 세웠을 때 기준으로 했음. 앞뒤 좌우 교체는 89줄에서 바꾸면됨. 아예 눕혔을 때 방향은 64줄 x자리에 y
/// 기본 중립 모양을 바꾸고 싶으면 64번줄 float상수 변경
public class GyroMove : MonoBehaviour
{
    private Quaternion deviceRotation;
    public Vector3 StrikerSpeed;

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
        AnglToVelo(deviceRotation);
        Rb.velocity = new Vector3(StrikerSpeed.x / 10, 0, StrikerSpeed.z / 10 - 10f);

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

}

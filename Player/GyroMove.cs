using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// 중립 상태, 가로로 핸드폰 위가 왼손에 오도록 잡을 때, 각도 x=28,y=-93,z=88
/// 기본 중립 모양을 바꾸고 싶으면 64번줄 float상수 변경
public class GyroMove : MonoBehaviour
{
    public Quaternion deviceRotation;
    public Vector3 AnglesVector;

    private Rigidbody Rb;
    private float PM = 2;//Speed multipiler

   // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;

        deviceRotation = GetDeviceRotation();
    }

    // Update is called once per frame
    void Update()
    {
        deviceRotation = GetDeviceRotation();
        RetationView.transform.rotation = DeviceRotationToEditorRotation(GetDeviceRotation());

        AnglToVelo(deviceRotation);
        MoveStriker();
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
        Text.text = string.Format("x = " + TranslatedAngles.x + "\n" + "y = " + TranslatedAngles.y + "\n" + "z = " + TranslatedAngles.z);
        AnglesVector = TranslatedAngles;
    }

    private void MoveStriker()
    {
        Vector3 StrikerSpeed = new Vector3(0, 0, 0);
        //기울기x 는 앞뒤 기울기
        if (AnglesVector.x > 30)
        {
            StrikerSpeed.z = PM;
        }
        else if (AnglesVector.x < 26)
        {
            StrikerSpeed.z  = -PM;
        }

        //기울기 z는 좌우 기울기
        if (AnglesVector.z > 93)
        {
            StrikerSpeed.x = PM;
        }
        else if (AnglesVector.z < 83)
        {
            StrikerSpeed.x = -PM;
        }

        Rb.velocity = StrikerSpeed;
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
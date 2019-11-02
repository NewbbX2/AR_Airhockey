using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

/// 중립 상태, 가로로 핸드폰 위가 왼손에 오도록 잡을 때, 각도 x=28,y=-93,z=88
/// 기본 중립 모양을 바꾸고 싶으면 64번줄 float상수 변경
public class GyroMove : MonoBehaviour
{
    #region 테스트용
    public GameObject RetationView;
    public TextMeshProUGUI Text;
    #endregion

    public Quaternion deviceRotation;
    public Vector3 AnglesVector;

    //초기 회전각을 저장할 변수
    //private float initialOrientationX;
    //private float InitGyroscopeAttitudeX;

    private Rigidbody Rb;
    private float PM = 2;//Speed multipiler

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
        //initialOrientationX = deviceRotation.x;//Input.gyro.rotationRateUnbiased.x;
        //InitGyroscopeAttitudeX = Input.gyro.rotationRateUnbiased.x;

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
        MoveStriker();
        //Rb.velocity = new Vector3(StrikerSpeed.z / 10, 0, StrikerSpeed.x / 10);


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
        Text.text = string.Format("x = " + TranslatedAngles.x + "\n" + "y = " + TranslatedAngles.y + "\n" + "z = " + TranslatedAngles.z);
        AnglesVector = TranslatedAngles;

        //StrikerSpeed = new Vector3(-(TranslatedAngles.z), TranslatedAngles.y, (TranslatedAngles.x));
    }

    private void MoveStriker()
    {
        Vector3 StrikerSpeed = new Vector3(0, 0, 0);

        if (AnglesVector.x > 30)
        {
            StrikerSpeed.x = PM;
        }
        else if (AnglesVector.x < 26)
        {
            StrikerSpeed.x = -PM;
        }

        if (AnglesVector.z > 93)
        {
            StrikerSpeed.z = PM;
        }
        else if (AnglesVector.z < 83)
        {
            StrikerSpeed.z = -PM;
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

    /*
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
    */
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
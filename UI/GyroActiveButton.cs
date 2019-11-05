using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GyroActiveButton : MonoBehaviour
{
    private TextMeshProUGUI ButtonText;
    bool IsGyroOn = false;

    private void Start()
    {
        ButtonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnButtonClicked()
    {
        ButtonText.text = "GYRO ";
        if (SetGyro(!IsGyroOn))
        {
            Debug.Log("GYRO ON");
            ButtonText.text += "on";
        }
        else
        {
            Debug.Log("GYRO OFF");
            ButtonText.text += "off";
        }
    }

    public bool SetGyro(bool state)
    {
        Input.gyro.enabled = state;
        IsGyroOn = state;
        return state;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vu_UIController : MonoBehaviour
{
    public Text SnackbarText;
    public Text RoomLabelText;

    private void Start()
    {
        SnackbarText.text = string.Empty;
        RoomLabelText.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void MessagePrint(string message)
    {
        SnackbarText.text = message;
    }

  
}

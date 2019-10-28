using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 버튼을 게임컨트롤러에서 생성하고 이 스크립트를 붙일것
/// 
public class ButtonScript : MonoBehaviour
{
    private ARHockeyGameController GameController;
    //private TextMesh ButtonText;
    // Start is called before the first frame update
    void Start()
    {
        GameController = FindObjectOfType<ARHockeyGameController>();
        //Text ButtonText = this.GetComponentInChildren<Text>();
    }

    public void ResetGame()
    {
        GameController.ResetGame();
    }
}

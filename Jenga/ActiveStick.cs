using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStick : MonoBehaviour
{
    private Builder GameController;

    // Start is called before the first frame update
    void Start()
    {
        GameController = FindObjectOfType<Builder>();

        //처음 층이 생성되면 개별 스틱은 비활성화
        if (gameObject.tag == "TopLayer")
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 오브젝트 활성화
    /// </summary>
    public void ActiveStickOnTop()
    {
        gameObject.SetActive(true);
        Debug.Log("Stick set");
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.tag == "KillZone")
        {
            GameController.BuildOnTop();
            Destroy(this.gameObject);
        }
    }
}

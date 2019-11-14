using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMoving : MonoBehaviour
{
    public float moveSize = 5.0f;
    private Rigidbody rgbody;

    // Start is called before the first frame update
    void Start()
    {
        rgbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePos("left");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePos("right");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePos("up");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePos("down");
        }
    }

    private void MovePos(string dir)
    {
        Vector3 newPos = Vector3.zero; 

        switch (dir)
        {
            case "left":
                newPos = new Vector3(transform.position.x - moveSize, 0.0f, 0.0f);
                break;
            case "right":
                newPos = new Vector3(transform.position.x + moveSize, 0.0f, 0.0f);
                break;
            case "up":
                newPos = new Vector3(0.0f, 0.0f, transform.position.z + moveSize);
                break;                
            case "down":
                newPos = new Vector3(0.0f, 0.0f, transform.position.z - moveSize);
                break;
        }

        rgbody.MovePosition(Vector3.Lerp(transform.position, newPos, Time.deltaTime * 10.0f));
    }
}

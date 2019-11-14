using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public GameObject target;

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public float DelayTime;

    private void Start()
    {
        Vector3 FixedPos =
            new Vector3(
            target.transform.position.x + offsetX,
            target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);
        transform.position = FixedPos;
    }

    void Update()
    {
        Vector3 FixedPos =
            new Vector3(
            target.transform.position.x + offsetX,
            target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, FixedPos, Time.deltaTime * DelayTime);
        //Debug.Log(FixedPos);
    }
}

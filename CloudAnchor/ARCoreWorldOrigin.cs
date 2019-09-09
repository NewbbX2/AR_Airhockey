using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;
using GoogleARCore.Examples.Common;

/// <summary>
/// Cloud Anchor의 기준이 되는 AR 앵커를 생성하기 위해서 사용되는 보조적인 클래스입니다.
/// </summary>
public class ARCoreWorldOrigin : MonoBehaviour
{
    #region AR셋팅
    public Transform ARCoreDeviceTransform; // ar 디바이스 위치
    public GameObject DetectedPlanePrefab; // 바닥 프리팹
    private bool IsOriginPlaced = false; // 앵커로 부터 생성됐는지 알려줌
    private Transform AnchorTransform; // 앵커 트랜스폼
    private List<DetectedPlane> NewPlanes = new List<DetectedPlane>(); // 앵커 설치후 인지된 바닥 리스트
    private List<GameObject> Planes = new List<GameObject>(); // 앵커 설치 전 인지된 바닥 리스트
    #endregion

    private void Start()
    {
        
    }
    // Update is called once per frame
    private void Update()
    {
        //traking 상태이면 종료
        if(Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        Pose worldPose = WorldToAnchorPose(Pose.identity);

        Session.GetTrackables<DetectedPlane>(NewPlanes, TrackableQueryFilter.New);
        foreach(var plane in NewPlanes)
        {
            GameObject planeObject = Instantiate(DetectedPlanePrefab, worldPose.position, worldPose.rotation, transform);
            planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(plane);

            if (!IsOriginPlaced)
            {
                Planes.Add(planeObject);
            }
        }
    }

    public void SetWorldOrigin(Transform anchorTransform)
    {
        if (IsOriginPlaced)
        {
            Debug.LogWarning("The WorldOrigin can be set only once");
            return;
        }

        IsOriginPlaced = true;
        AnchorTransform = anchorTransform;
        Pose worldPose = WorldToAnchorPose(new Pose(ARCoreDeviceTransform.position, ARCoreDeviceTransform.rotation));
        ARCoreDeviceTransform.SetPositionAndRotation(worldPose.position, worldPose.rotation);

        foreach(GameObject plane in Planes)
        {
            if(plane != null)
            {
                plane.transform.SetPositionAndRotation(worldPose.position, worldPose.rotation);
            }
        }
       
        //plane 셋팅 자리
    }

    public bool Raycast(float x, float y, TrackableHitFlags filter, out TrackableHit hitResult)
    {
        bool foundHit = Frame.Raycast(x, y, filter, out hitResult);
        if (foundHit)
        {
            Pose worldPose = WorldToAnchorPose(hitResult.Pose);
            TrackableHit newHit = new TrackableHit(worldPose, hitResult.Distance, hitResult.Flags, hitResult.Trackable);
            hitResult = newHit;
        }

        return foundHit;
    }

    #region 앵커 배치 관련 메서드
    private Pose WorldToAnchorPose(Pose pose)
    {
        if (!IsOriginPlaced)
        {
            return pose;
        }

        Matrix4x4 anchorTRSWorld = Matrix4x4.TRS(AnchorTransform.position, AnchorTransform.rotation, Vector3.one).inverse;
        Vector3 position = anchorTRSWorld.MultiplyPoint(pose.position);
        Quaternion rotation = pose.rotation * Quaternion.LookRotation(anchorTRSWorld.GetColumn(2), anchorTRSWorld.GetColumn(1));
        return new Pose(position, rotation);
    }
    #endregion
}

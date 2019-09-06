using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class ARCoreWorldOrigin : MonoBehaviour
{
    #region AR셋팅
    public Transform ARCoreDeviceTransform; // ar 디바이스 위치
    private bool IsOriginPlaced = false; // 앵커로 부터 생성됐는지 알려줌
    private Transform AnchorTransform;

    #endregion
        
    // Update is called once per frame
    void Update()
    {
        //traking 상태이면 종료
        if(Session.Status != SessionStatus.Tracking)
        {
            return;
        }    
        
        //plane 생성 자리
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

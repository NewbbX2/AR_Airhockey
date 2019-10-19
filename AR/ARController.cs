using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GoogleARCore;

using Photon.Pun;
using Photon.Realtime;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARController : MonoBehaviourPunCallbacks, IPunObservable
{
    public Camera FirstPersonCamera;

    #region AR Core 공간 감지 변수들
    //public GameObject DetectedPlanePrefab; // 감지된 평면을 시각화
    public GameObject HockeyTablePrefab; // 실제로 배치 되는 테이블 프리팹
    public Text SnackBarText;
    private List<AugmentedImage> ImageList = new List<AugmentedImage>();
    #endregion

    private bool IsSpawn = false;
    //private const float ModelRotation = 180.0f;
            
    public void Update()
    {
        // 이미 배치되었으면 종료
        if (IsSpawn)
        {            
            return;
        }

        // 터치 없으면 업데이트 종료
        Touch touch;        
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }

        /* Canvas를 통한 eventSystem이 존재하면 설치
        // UI 터치하고 있으면 업데이트 종료
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }
        */

        //이미지로 발동
        Session.GetTrackables<AugmentedImage>(ImageList, TrackableQueryFilter.Updated);
        foreach(var image in ImageList)
        {
            if(image.TrackingState == TrackingState.Tracking && !IsSpawn)
            {
                SnackBarText.text = "Tracking now";
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                var gameTable = Instantiate(HockeyTablePrefab, anchor.transform.position, anchor.transform.rotation);
                //Instantiate(HockeyTablePrefab, anchor.transform);
            }
            else if(image.TrackingState == TrackingState.Stopped)
            {
                SnackBarText.text = "not Tracking now";
            }
        }

        // 평면 찾기 위한 Raycast 설정
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        //Debug.Log(touch.position);
        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {            
            //Ray가 부딪힌 장소와 카메라 포지션을 이용해서 생성된 평면 뒤가 아닌가 확인
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                Debug.Log("Touched");
                Debug.Log(hit.Trackable);
                if (hit.Trackable is DetectedPlane)
                {
                    DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                    //수평 윗면인지 확인해서 인스턴스 생성
                    if (detectedPlane.PlaneType == DetectedPlaneType.HorizontalUpwardFacing)
                    {
                        var gameTable = PhotonNetwork.Instantiate(HockeyTablePrefab.name, hit.Pose.position, hit.Pose.rotation);
                        Debug.Log("Table is set");
                        gameTable.transform.Rotate(0, 0, 0, Space.Self);
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                        gameTable.transform.parent = anchor.transform;
                        IsSpawn = true;
                    }
                }
                Debug.Log("No trackable");
            }
        }        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
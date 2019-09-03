﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

using GoogleARCore;
using GoogleARCore.CrossPlatform;

//에디터 실행시 인스턴트 프리뷰 실행 시키기
#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class CloudAnchorController : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum ApplicationMode
    {
        Ready,
        Hosting,
        Resolving,
    }

    public Text SnackbarText;

    #region AR관련 부속품
    [Header("ARCore")]
    public GameObject ARCoreRoot;
    public Text RoomLabel;
    public ARCoreWorldOrigin WorldOrigin;
    private string CloudAnchor_Id = string.Empty;
    #endregion

    #region 내부 변수
    private bool IsOriginPlaced = false; // 앵커가 있는지 확인
    private bool AnchorAlreadyInstantiated = false; // 앵커가 이미 있는지 확인
    private bool AnchorFinishedHosting = false; // 앵커 호스팅이 끝났는지 확인
    private bool IsQuitting = false; // 앱이 종료되는지 확인
    private Component WorldOriginAnchor = null; // 앵커
    private Pose? LastHitPos = null; // AR hit test에서 마지막으로 hit한 곳
    private ApplicationMode CurrentMode = ApplicationMode.Ready;
    private bool ShouldResolve = false;

    #endregion

    private void Awake()
    {
        //AR 카메라 주사율을 프레임 레이트 타겟으로 설정. 수직동기화 걸려있으면 이 옵션이 비활성화
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    private void Start()
    {
        ARCoreRoot.SetActive(false);
        RoomLabel.text = "Room" + PhotonNetwork.CurrentRoom.Name;
        _ResetStatus();

        //마스터면 hosting, 아니면 resolving
        if (PhotonNetwork.IsMasterClient)
        {
            HostingModeActivate();
        }
        else
        {
            ResolvingModeActivate();
        }

        OnEnterRoom();
    }

    // Update is called once per frame
    private void Update()
    {
        _UpdateApplicationLifecycle();
                
        //hosting, resolving 둘다 아니면 그냥 종료
        if(CurrentMode != ApplicationMode.Hosting &&
            CurrentMode != ApplicationMode.Resolving)
        {
            return;
        }

        //resolving 상태이나 앵커가 없으면 종료
        if(CurrentMode == ApplicationMode.Resolving && !IsOriginPlaced)
        {
            return;
        }

        //터치 입력 없으면 업데이트 종료
        Touch touch;
        if(Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        //터치 하면 커스텀된 Raycast 발동
        TrackableHit arcoreHitResult = new TrackableHit();
        LastHitPos = null;

        if(WorldOrigin.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out arcoreHitResult))
        {
            LastHitPos = arcoreHitResult.Pose;
        }

        if(LastHitPos != null)
        {
            // 터치한 곳에 AR오브젝트가 있다면 이곳이 실행됨
        }

        //앵커 resolve
        if (ShouldResolve)
        {
            SnackbarText.text = string.Empty;
            ResolveAnchorFromId(CloudAnchor_Id);
        }
        else if(!PhotonNetwork.IsMasterClient)
        {
            ShowDebugMessage("Plase wait Player setting table...");
        }
    }

    #region 앵커 배치 관련 메서드
    //기반 월드 셋
    public void SetWorldOrigin(Transform anchorTransform)
    {
        if (IsOriginPlaced)
        {
            Debug.LogWarning("The world origin can be set only once");
            return;
        }

        IsOriginPlaced = true;
        WorldOrigin.SetWorldOrigin(anchorTransform);
    }

    //호스팅 모드 활성
    public void HostingModeActivate()
    {
        if(CurrentMode == ApplicationMode.Hosting)
        {
            CurrentMode = ApplicationMode.Ready;
            _ResetStatus();
            Debug.Log("Reset app from hosting to ready");
        }
        CurrentMode = ApplicationMode.Hosting;
        ARCoreRoot.SetActive(true);
    }

    //앵커 받아오기 활성
    public void ResolvingModeActivate()
    {
        if(CurrentMode == ApplicationMode.Resolving)
        {
            CurrentMode = ApplicationMode.Ready;
            _ResetStatus();
            Debug.Log("Reset app from resolving to ready");
        }

        CurrentMode = ApplicationMode.Resolving;
        ARCoreRoot.SetActive(true);
    }

    //앵커 cloud id를 resolve하고 프리팹을 그위에 instantiate
    public void ResolveAnchorFromId(string cloudAnchorId)
    {
        OnAnchorInstantiated(false);

        // 디바이스가 트래킹하고 있지 않으면 앵커 resolve를 기다리자
        if(Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        ShouldResolve = false;

        XPSession.ResolveCloudAnchor(cloudAnchorId).ThenAction(
            (System.Action<CloudAnchorResult>)(result =>
            {
                if(result.Response != CloudServiceResponse.Success)
                {
                    Debug.LogError(string.Format(
                        "Client could not resolve Cloud Anchor {0}: {1}",
                        cloudAnchorId, result.Response)
                        );
                    OnAnchorResolved(false, result.Response.ToString());
                    ShouldResolve = true;
                    return;
                }
                Debug.Log(string.Format("Client successfully resolved Cloud Anchor {0}", cloudAnchorId));
                OnAnchorResolved(true, result.Response.ToString());
                OnResolve(result.Anchor.transform);
            }
            ));

    }

    public void OnAnchorInstantiated(bool isHost)
    {
        //이미 앵커가 배치됬으면 실행 안함
        if (AnchorAlreadyInstantiated)
        {
            return;
        }
        if (isHost)
        {
            ShowDebugMessage("Hosting Cloud Anchor");
        }
        else
        {
            ShowDebugMessage("Cloud Anchor added to session. Now resolve anchor");
        }
        AnchorAlreadyInstantiated = true;
    }

    public void OnAnchorHostinged(bool success, string response)
    {
        AnchorFinishedHosting = success;
        if (success)
        {
            ShowDebugMessage("Cloud Anchor successfully hosted!");
        }        
    }

    public void OnAnchorResolved(bool success, string response)
    {
        if (success)
        {
            ShowDebugMessage("Cloud Anchor successfully hosted!");
        }
    }

    public void OnResolve(Transform anchorTransform)
    {
        SetWorldOrigin(anchorTransform);
    }

    public void HostLastPlacedAnchor(Component lastPlacedAnchor)
    {
        var anchor = (Anchor)lastPlacedAnchor;

        XPSession.CreateCloudAnchor(anchor).ThenAction(result =>
        {
            if(result.Response != CloudServiceResponse.Success)
            {
                Debug.Log(string.Format("Host Cloud Anchor is failed: {0}", result.Response));
                OnAnchorHostinged(false, result.Response.ToString());
                return;
            }
            Debug.Log(string.Format("Cloud Anchor {0} is created and saved", result.Anchor.CloudId));
            CloudAnchor_Id = result.Anchor.CloudId;
            OnAnchorHostinged(true, result.Response.ToString());
        }
        );
    }

    #endregion

    #region 앱 관련 상태 조정
    //앱 상태를 리셋
    private void _ResetStatus()
    {
        CurrentMode = ApplicationMode.Ready;
        if(WorldOriginAnchor != null)
        {
            Destroy(WorldOriginAnchor.gameObject);
        }

        WorldOriginAnchor = null;
    }

    //앱 생명주기를 업데이트
    private void _UpdateApplicationLifecycle()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }

        var sleepTimeout = SleepTimeout.NeverSleep;
        if(Session.Status != SessionStatus.Tracking)
        {
            sleepTimeout = SleepTimeout.SystemSetting;
        }

        Screen.sleepTimeout = sleepTimeout;

        if(IsQuitting)
        {
            return;
        }

        if(Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _QuitWithReason("Camera permission is needed to play AR Air Hockey");
        }
    }

    private void _QuitWithReason(string reason) {
        if (IsQuitting)
        {
            return;
        }
        ShowDebugMessage(reason);
        IsQuitting = true;
        Invoke("_DoQuit", 5.0f);
    } 

    private void _DoQuit()
    {
        Application.Quit();
    }
    #endregion

    #region 네트워크
    private void OnEnterRoom()
    {
        //모드 지정
        if(CurrentMode == ApplicationMode.Hosting)
        {
            ShowDebugMessage("Tab to create Air hockey table");
        }
        else if(CurrentMode == ApplicationMode.Resolving)
        {
            ShowDebugMessage("Waiting for creating Air hockey table");
        }
        else
        {
            _QuitWithReason("Connected with neither hosting nor resolving mode. Please restart app");
        }

        //resolve 해야하는지 설정
        if(CloudAnchor_Id != string.Empty)
        {
            ShouldResolve = true;
        } 
    }

    private void InstantiatedAnchor()
    {
    }
    #endregion
       
    public void ShowDebugMessage(string debugMessage)
    {
        SnackbarText.text = debugMessage;
    }


    #region Cloud ID

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CloudAnchor_Id);
        }
        else
        {
            CloudAnchor_Id = (string)stream.ReceiveNext();
        }
    }
    #endregion
}

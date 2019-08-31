using System;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class AnchorNetworkManager : NetworkManager
#pragma warning restore 618

{
    #region 액션 선언 
    public event Action OnClientConnected; // 서버에 연결되면 호출
    public event Action OnClientDisconnected; // 서버에서 연결 해제되면 호출
    #endregion

#pragma warning disable 618
    public override void OnClientConnect(NetworkConnection conn)
#pragma warning restore 618
    {
        base.OnClientConnect(conn);
        Debug.Log("Connected to Server : " + conn.lastError);
        if(OnClientConnected != null)
        {
            OnClientConnected();
        }
    }

#pragma warning disable 618
    public override void OnClientDisconnect(NetworkConnection conn)
#pragma warning restore 618
    {
        base.OnClientDisconnect(conn);
        Debug.Log("Disconnected from Server" + conn.lastError);
        if(OnClientDisconnected != null)
        {
            OnClientDisconnected();
        }
    }
}

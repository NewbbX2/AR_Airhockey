using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableEventHandler : DefaultTrackableEventHandler
{
    private Vu_NetworkController NetCon;
    private Vu_UIController UICon;

    private bool ShouldConnect = false;

    private void Awake()
    {
        NetCon = FindObjectOfType<Vu_NetworkController>();
        UICon = FindObjectOfType<Vu_UIController>();
    }

    protected override void Start()
    {
        base.Start();        
    }

    protected void Update()
    {
        ShouldConnect = NetCon.Ready;
    }

    protected override void OnTrackingFound()
    {        
        if (ShouldConnect)
        {
            ShouldConnect = NetCon.EnterToRoom();
            NetCon.Ready = false;
        }       

    }

    protected override void OnTrackingLost()
    {
        //base.OnTrackingLost();
        UICon.MessagePrint("Lost target. Please aim image");
    }
}

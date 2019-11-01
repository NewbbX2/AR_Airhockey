using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableEventHandler : DefaultTrackableEventHandler
{
    private Vu_NetworkController NetCon;
    private Vu_UIController UICon;

    bool ShouldConnect = true;

    private void Awake()
    {
        NetCon = FindObjectOfType<Vu_NetworkController>();
        UICon = FindObjectOfType<Vu_UIController>();
    }

    protected override void Start()
    {
        base.Start();        
    }

    protected override void OnTrackingFound()
    {        
        if (ShouldConnect)
        {
            ShouldConnect = NetCon.EnterToRoom();
        }       

    }

    protected override void OnTrackingLost()
    {
        //base.OnTrackingLost();
        UICon.MessagePrint("Lost target. Please aim image");
    }
}

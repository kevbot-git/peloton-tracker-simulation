using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MasterBehaviour : BluetoothDevice
{
    public override void Start()
    {
        base.Start();
        Colour = baseColour = Color.blue;
        //StartCoroutine(BlinkTask(Color.red)); // Can be annoying
    }

    // Master devices initiate ping events, therefore shouldn't need to supply a Ping Packet
    public void Ping()
    {
        base.Ping(new PingPacket(new PingEvent(this, DateTime.Now.Millisecond)));
    }

    protected override void OnPinged(PingPacket packet, bool backtrack = false)
    {
        base.OnPinged(packet);
        Log("Received ping. Trace length: " + packet.Trace.Count + ". Content: " + packet.ToString());
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (GetConnections().Count > 0)
        {
            Ping();
        }
    }
}

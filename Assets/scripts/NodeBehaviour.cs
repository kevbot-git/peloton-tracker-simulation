using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour : BluetoothDevice
{
    public override void Start()
    {
        base.Start();
        Colour = baseColour = Color.green;
        //StartCoroutine(BlinkTask(Color.green)); // Can be annoying
    }

    protected override void OnPinged(PingPacket packet, bool backtrack = false)
    {
        base.OnPinged(packet);

        // If there the sender is the only connection this device has...
        if (GetConnections(packet.Sender).Count > 0)
        {
            Log("Forwarding to " + GetConnections(packet.Sender).Count + " other connections...");
            Ping(packet); // Forward as normal
        }
        else
        {
            Log("No one else to send to; backtracking...");
            Ping(packet, true); // Backtrack
        }
            
    }
}

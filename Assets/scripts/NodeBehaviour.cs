using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour : BluetoothDevice
{
    // Default number when not linked directly or indirectly to a master device
    private static readonly int UNLINKED = -1;

    private int linksFromMaster = -1;

    public override void Start()
    {
        base.Start();
        StartCoroutine(BlinkTask(Color.green));
    }
}

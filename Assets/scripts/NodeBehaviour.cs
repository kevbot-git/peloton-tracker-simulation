using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour : BluetoothDevice
{
    public override void Start()
    {
        base.Start();
        StartCoroutine(BlinkTask(Color.green));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBehaviour : BluetoothDevice
{
    public override void Start()
    {
        base.Start();
        StartCoroutine(BlinkTask(Color.red));
    }
}

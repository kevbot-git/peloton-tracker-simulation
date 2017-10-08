using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBehaviour : BluetoothDevice
{
    public void OnEnable()
    {
        Debug.Log(This.name + " started");
    }
}

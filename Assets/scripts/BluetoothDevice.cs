using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluetoothDevice {

    private SphereCollider connectionCollider;

    public BluetoothDevice(GameObject gameObject, float connectionRange)
    {
        connectionCollider = gameObject.AddComponent<SphereCollider>();
        connectionCollider.center = Vector3.zero;
        connectionCollider.radius = connectionRange;
    }
}

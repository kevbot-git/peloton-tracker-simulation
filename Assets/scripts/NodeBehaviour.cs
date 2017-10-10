using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour : BluetoothDevice
{
    public override void Start()
    {
        base.Start();
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        Debug.Log(Time.time);
        yield return new WaitForSeconds(5);
        Debug.Log(Time.time);
    }
}

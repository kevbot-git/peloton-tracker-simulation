using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BluetoothDevice: MonoBehaviour
{
    public GameObject This;
    public float ConnectionRange = 5.0f;

    private SphereCollider connectionCollider;
    private LineRenderer lineRenderer;

    private static List<GameObject> paired;
    private static readonly string BLUETOOTH_DEVICE = "BluetoothDevice";

    public virtual void Start()
    {
        paired = new List<GameObject>();

        connectionCollider = gameObject.AddComponent<SphereCollider>();
        connectionCollider.center = Vector3.zero;
        connectionCollider.radius = ConnectionRange;
        connectionCollider.isTrigger = true;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.025f;
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = lineRenderer.endColor = Color.green;
    }

    public void Update()
    {
        lineRenderer.positionCount = paired.Count;
        foreach(GameObject o in paired)
        {
            lineRenderer.SetPosition(0, This.transform.position);
            lineRenderer.SetPosition(1, o.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == BLUETOOTH_DEVICE)
        {
            Log("Adding " + other.name);
            paired.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == BLUETOOTH_DEVICE)
        {
            Log("Removing " + other.name);
            paired.Remove(other.gameObject);
            Log(paired.Count);
        }
    }

    private void Log(object message)
    {
        Debug.Log("[" + This.name + "] " + message);
    }
}

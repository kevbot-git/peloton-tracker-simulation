using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BluetoothDevice: MonoBehaviour
{
    public GameObject DeviceGameObject;
    public float ConnectionRange = 5.0f;

    private SphereCollider connectionCollider;

    private static BluetoothConnectionSet paired;
    private static readonly string BLUETOOTH_DEVICE = "BluetoothDevice";

    public virtual void Start()
    {
        paired = new BluetoothConnectionSet(7); // Set max connections to 7, as is the case with Bluetooth 5.0 mesh specification

        connectionCollider = gameObject.AddComponent<SphereCollider>();
        connectionCollider.center = Vector3.zero;
        connectionCollider.radius = ConnectionRange;
        connectionCollider.isTrigger = true;

    }

    public virtual void Update()
    {
        //
    }

    private void OnTriggerEnter(Collider other)
    {
        BluetoothDevice otherDevice = other.gameObject.GetComponent<BluetoothDevice>();
        if (otherDevice)
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

    public IEnumerator BlinkTask(Color colour, float delay = 1f)
    {
        while(true)
        {
            Color startColour = DeviceGameObject.GetComponent<Renderer>().material.color;
            yield return new WaitForSeconds(delay);
            DeviceGameObject.GetComponent<Renderer>().material.color = colour;
            yield return new WaitForSeconds(delay);
            DeviceGameObject.GetComponent<Renderer>().material.color = startColour;
        }
    }

    public BluetoothConnectionSet GetConnections(GameObject exclude = null)
    {
        if (exclude == null)
        {
            return paired;
        }
        else
        {
            BluetoothConnectionSet s = paired;
            s.Remove(exclude);
            return s;
        }
    }

    private void Log(object message)
    {
        Debug.Log("[" + DeviceGameObject.name + "] " + message);
    }
}

public class BluetoothConnectionSet
{
    private HashSet<GameObject> set;
    private int maxConnections;

    public BluetoothConnectionSet(int maxConnections = 7)
    {
        this.maxConnections = maxConnections;
        set = new HashSet<GameObject>();
    }

    public bool Add(GameObject gameObject)
    {
        if(set.Count < maxConnections)
        {
            var r = set.Add(gameObject);
            Debug.Log("set: " + set.Count + " max: " + maxConnections + " out: " + r);
            return r;
        }
        return false;
    }

    public bool Remove(GameObject gameObject)
    {
        return set.Remove(gameObject);
    }

    public IEnumerator<GameObject> GetEnumerator()
    {
        return set.GetEnumerator();
    }

    public int Count
    {
        get { return set.Count; }
    }
}

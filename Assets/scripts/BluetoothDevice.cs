using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BluetoothDevice: MonoBehaviour
{
    public static readonly int MAX_CONNECTIONS = 7; // Set max connections to 7, as is the case with Bluetooth 5.0 mesh specification

    public float ConnectionRange = 5.0f;

    private SphereCollider connectionCollider;

    private BluetoothConnectionSet paired;
    private static GameObject selectedDevice = null;
    private static readonly string BLUETOOTH_DEVICE = "BluetoothDevice";

    private LineRenderer line;

    public virtual void Start()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = line.endWidth = .1f;
        line.positionCount = 0;

        paired = new BluetoothConnectionSet(MAX_CONNECTIONS);

        connectionCollider = gameObject.AddComponent<SphereCollider>();
        connectionCollider.center = Vector3.zero;
        connectionCollider.radius = ConnectionRange;
        connectionCollider.isTrigger = true;
    }

    public virtual void Update()
    {
        //
    }

    private static void SelectDevice(GameObject device)
    {
        selectedDevice = device;
        Debug.Log("Selected " + device.name);
    }

    private void OnMouseDown()
    {
        SelectDevice(gameObject);
    }

    private void OnMouseDrag()
    {
        Vector3 screenPoint = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 newPos = ray.GetPoint(Camera.main.transform.position.y);
        newPos.y = 0;

        transform.position = newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        BluetoothDevice otherDevice = other.gameObject.GetComponent<BluetoothDevice>();
        if (otherDevice)
        if(other.tag == BLUETOOTH_DEVICE)
        {
            paired.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == BLUETOOTH_DEVICE)
        {
            paired.Remove(other.gameObject);
        }
    }

    public IEnumerator BlinkTask(Color colour, float delay = 1f)
    {
        while(true)
        {
            Color startColour = gameObject.GetComponent<Renderer>().material.color;
            yield return new WaitForSeconds(delay);
            gameObject.GetComponent<Renderer>().material.color = colour;
            yield return new WaitForSeconds(delay);
            gameObject.GetComponent<Renderer>().material.color = startColour;
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
        Debug.Log("[" + gameObject.name + "] " + message);
    }

    public static GameObject GetSelectedDevice()
    {
        return selectedDevice;
    }

    public LineRenderer GetLineRenderer()
    {
        return line;
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

    public List<GameObject> AsList()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject g in set)
        {
            list.Add(g);
        }
        return list;
    }

    public int Count
    {
        get { return set.Count; }
    }
}

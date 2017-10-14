using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class BluetoothDevice: MonoBehaviour
{
    public static readonly int MAX_CONNECTIONS = 7; // Set max connections to 7, as is the case with Bluetooth 5.0 mesh specification

    public float ConnectionRange = 5.0f;
    public float SpeedOfLight = 500f; // This affects how quickly devices communicate

    private SphereCollider connectionCollider;

    private BluetoothConnectionSet paired;
    private static BluetoothDevice selectedDevice = null;
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
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // Check if the mouse is dragging this object
                if (hit.transform.gameObject == gameObject)
                {
                    Vector3 screenPoint = Input.mousePosition;

                    Vector3 newPos = ray.GetPoint(Camera.main.transform.position.y);
                    newPos.y = 0;

                    // Make this object follow the mouse
                    transform.position = newPos;
                }
            }
        }
    }

    private static void SelectDevice(BluetoothDevice device)
    {
        selectedDevice = device;
    }

    private void OnMouseDown()
    {
        SelectDevice(this);
        if (paired.Count > 0)
        {
            foreach (BluetoothDevice d in paired)
            {
                Ping(d);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == BLUETOOTH_DEVICE)
        {
            BluetoothDevice otherDevice = other.gameObject.GetComponent<BluetoothDevice>();
            OnConnect(otherDevice);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == BLUETOOTH_DEVICE)
        {
            BluetoothDevice otherDevice = other.gameObject.GetComponent<BluetoothDevice>();
            OnDisconnect(otherDevice);
        }
    }

    public virtual void OnPinged(BluetoothDevice from) {
        Log(from.DeviceGameObject.name + " pinged me!");
    }

    public void Ping(BluetoothDevice target) {
        StartCoroutine(PingTask(target));
    }

    protected virtual void OnConnect(BluetoothDevice device)
    {
        paired.Add(device);
    }

    protected virtual void OnDisconnect(BluetoothDevice device)
    {
        paired.Remove(device);
    }

    private IEnumerator PingTask(BluetoothDevice target) {
        Log("Pinging " + target.gameObject.name + "...");
        // Delay by taking into account the displacement between the two devices
        float displacement = Vector3.Distance(target.transform.position, transform.position);
        yield return new WaitForSeconds(1f / SpeedOfLight * displacement);
        target.OnPinged(this);
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

    public BluetoothConnectionSet GetConnections(BluetoothDevice exclude = null)
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

    public GameObject DeviceGameObject
    {
        get { return gameObject; }
    }

    protected void Log(object message)
    {
        Debug.Log("[" + gameObject.name + "] " + message);
    }

    public static BluetoothDevice GetSelectedDevice()
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
    private HashSet<BluetoothDevice> set;
    private int maxConnections;

    public BluetoothConnectionSet(int maxConnections = 7)
    {
        this.maxConnections = maxConnections;
        set = new HashSet<BluetoothDevice>();
    }

    public bool Add(BluetoothDevice device)
    {
        if(set.Count < maxConnections)
        {
            var r = set.Add(device);
            return r;
        }
        return false;
    }

    public bool Remove(BluetoothDevice device)
    {
        return set.Remove(device);
    }

    public IEnumerator<BluetoothDevice> GetEnumerator()
    {
        return set.GetEnumerator();
    }

    public List<BluetoothDevice> AsList()
    {
        List<BluetoothDevice> list = new List<BluetoothDevice>();
        foreach (BluetoothDevice d in set)
        {
            list.Add(d);
        }
        return list;
    }

    public List<GameObject> DeviceGameObjects
    {
        get { return set.Select(o => o.DeviceGameObject).ToList();  }
    }

    public int Count
    {
        get { return set.Count; }
    }
}

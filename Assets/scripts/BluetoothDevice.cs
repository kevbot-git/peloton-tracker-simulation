using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class BluetoothDevice: MonoBehaviour
{
    public static readonly int MAX_CONNECTIONS = 7; // Set max connections to 7, as is the case with Bluetooth 5.0 mesh specification

    public float ConnectionRange = 5.0f;

    private SphereCollider connectionCollider;

    private BluetoothConnectionSet paired;
    private static BluetoothDevice selectedDevice = null;
    private static readonly string BLUETOOTH_DEVICE = "BluetoothDevice";
    private static readonly float SPEED_OF_LIGHT = 5f; // This affects how quickly devices communicate

    private LineRenderer line;
    protected Color baseColour;

    public virtual void Start()
    {
        baseColour = Colour;
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = line.endWidth = .1f;
        line.positionCount = 0;

        paired = new BluetoothConnectionSet(MAX_CONNECTIONS);

        connectionCollider = gameObject.AddComponent<SphereCollider>();
        connectionCollider.center = Vector3.zero;
        connectionCollider.radius = ConnectionRange;
        connectionCollider.isTrigger = true;

        //StartCoroutine(PingTask());
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

    protected virtual void OnMouseDown()
    {
        SelectDevice(this);
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

    public void Ping(PingPacket packet, bool backtrack = false)
    {
        if(backtrack)
        {
            StartCoroutine(PingTask(packet.Sender, DateTime.Now.Millisecond, packet, backtrack));
        }
        else
        {
            // Ping every device that doesn't appear in this packet's stack
            foreach (BluetoothDevice d in paired)
            {
                if (d.GetType() == typeof(MasterBehaviour) || !packet.Trace.Select(o => o.Sender).Contains(d))
                {
                    StartCoroutine(PingTask(d, DateTime.Now.Millisecond, packet));
                }
                else
                {
                    Log(d.name + " is already in this ping trace!");
                }
            }
        }
    }

    protected virtual void OnPinged(PingPacket packet, bool backtrack = false) {
        Log(packet.Sender.name + " pinged me!");
        StartCoroutine(BlinkTask(Color.magenta, .2f, false));
    }

    protected virtual void OnConnect(BluetoothDevice device)
    {
        paired.Add(device);
    }

    protected virtual void OnDisconnect(BluetoothDevice device)
    {
        paired.Remove(device);
    }

    private IEnumerator PingTask(BluetoothDevice target, int timeSentMillis, PingPacket packet, bool backtrack = false) {
        Log("Pinging " + target.name + "...");

        // Delay by taking into account the displacement between the two devices
        float displacement = Vector3.Distance(target.transform.position, transform.position);
        yield return new WaitForSeconds(1f / SPEED_OF_LIGHT * displacement);

        // Append a new PingEvent to the packet and forward it
        target.OnPinged(new PingPacket(new PingEvent(this, timeSentMillis), packet), backtrack);
    }

    public IEnumerator BlinkTask(Color colour, float delay = 0.5f, bool loop = true)
    {
        do
        {
            Colour = colour;
            yield return new WaitForSeconds(delay);
            Colour = baseColour;
            yield return new WaitForSeconds(delay);
        } while (loop);
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
        Debug.Log("[" + name + "] " + message);
    }

    public static BluetoothDevice GetSelectedDevice()
    {
        return selectedDevice;
    }

    public LineRenderer GetLineRenderer()
    {
        return line;
    }

    public Color Colour
    {
        get { return gameObject.GetComponent<Renderer>().material.color; }
        set { gameObject.GetComponent<Renderer>().material.color = value; }
    }
}

public class PingPacket
{
    public Queue<PingEvent> Trace { get; private set; }
    public BluetoothDevice Sender { get { return Trace.Peek().Sender; } }

    public PingPacket(PingEvent pingEvent, PingPacket history = null)
    {
        // If the sender of this ping packet was the ping initiator, create an empty history queue
        if (history == null)
        {
            // Initialise a trace
            Trace = new Queue<PingEvent>();
        }
        else
        {
            // Copy the sender's trace
            Trace = history.Trace;
        }

        // Enqueue new event to the trace
        Trace.Enqueue(pingEvent);
    }

    public override string ToString()
    {
        string s = "PingPacket {\n";
        List<PingEvent> list = Trace.ToList();
        for (int i = 0; i < Trace.Count; i++)
        {
            s += list[i].ToString();
            if (i + 1 < Trace.Count)
                s += "\n";
        }
        s += "\n}";
        return s;
    }
}

public class PingEvent
{
    public BluetoothDevice Sender { get; private set; }
    public int TimeSentMillis { get; private set; }

    public PingEvent(BluetoothDevice sender, int timeSendMillis)
    {
        Sender = sender;
        TimeSentMillis = timeSendMillis;
    }

    public override string ToString()
    {
        return "PingEvent { Sender: " + Sender.name + "; Sent: " + TimeSentMillis + " }";
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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlBehaviour : MonoBehaviour
{
    public GameObject MasterDevicePrefab;
    public GameObject NodeDevicePrefab;

    public int numberOfMasterDevices = 2;
    public int numberOfNodeDevices = 20;

    public int gridSpacing = 5;
    public int spread = 1;

    public static int GUI_MARGIN = 12;

    private List<GameObject> devices;

    void Start ()
    {
        System.Random random = new System.Random();

        // This confines devices to starting in a roughly square area
        int gridWidth = (int) Mathf.Ceil(Mathf.Sqrt(numberOfMasterDevices + numberOfNodeDevices) + spread);

        devices = new List<GameObject>();

        // Make a list of all the possible positions
        List<int> allPositions = Enumerable.Range(0, gridWidth * gridWidth - 1).ToList();

        // Shuffle and limit to number of devices
        allPositions = allPositions.OrderBy(item => random.Next()).ToList().GetRange(0, numberOfMasterDevices + numberOfNodeDevices);

        // Loop through list of positions, instantiating master devices, then node devices
        for (int i = 0; i < allPositions.Count; i++)
        {
            int z = (int) Mathf.Floor(allPositions[i] / gridWidth) * gridSpacing;
            int x = allPositions[i] % gridWidth * gridSpacing;

            if (i < numberOfMasterDevices)
            {
                GameObject o = Instantiate(MasterDevicePrefab, new Vector3(x, 0, z), Quaternion.identity);
                o.name = "Master Device " + (i + 1);
                devices.Add(o);
            }
            else
            {
                GameObject o = Instantiate(NodeDevicePrefab, new Vector3(x, 0, z), Quaternion.identity);
                o.name = "Node Device " + (i - numberOfMasterDevices + 1);
                devices.Add(o);
            }
        }
    }

    private void Update()
    {
        BluetoothDevice selected = BluetoothDevice.GetSelectedDevice();

        foreach (GameObject g in devices)
        {
            LineRenderer line = g.GetComponent<LineRenderer>();
            line.positionCount = 0;
        }

        if (selected)
        {
            foreach (GameObject o in selected.GetConnections().DeviceGameObjects)
            {
                LineRenderer line = o.GetComponent<LineRenderer>();
                line.positionCount = 2;
                line.SetPositions(new Vector3[] { selected.transform.position, o.transform.position });
            }
        }
    }

    private void OnGUI()
    {
        string title = "";
        BluetoothDevice selected = BluetoothDevice.GetSelectedDevice();

        if (selected)
            title = selected.DeviceGameObject.name;

        GUILayout.BeginArea(new Rect(GUI_MARGIN, GUI_MARGIN, Screen.width / 3, Screen.height), title);

        // If a device has been selected, list all of its connections
        if (selected)
        {
            // Create blank label to correct strange spacing
            GUILayout.Label("");

            BluetoothConnectionSet selectedConnections = BluetoothDevice.GetSelectedDevice().GetConnections();

            foreach (BluetoothDevice connected in selectedConnections)
            {
                GUILayout.Label(connected.DeviceGameObject.name);
            }
        }

        GUILayout.EndArea();
    }
}


/*
 
    
            new Rect(Vector2.zero, new Vector2(10, 10)),
            0, deviceList, 
     
     */

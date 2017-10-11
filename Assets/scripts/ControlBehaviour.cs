using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class ControlBehaviour : MonoBehaviour
{
    public GameObject MasterDevicePrefab;
    public GameObject NodeDevicePrefab;

    public int numberOfMasterDevices = 2;
    public int numberOfNodeDevices = 20;

    public int gridSpacing = 5;

    private List<GameObject> devices;

    void Start ()
    {
        System.Random random = new System.Random();
        int gridWidth = (int) Mathf.Ceil(Mathf.Sqrt(numberOfMasterDevices + numberOfNodeDevices));

        devices = new List<GameObject>();

        List<int> allPositions = Enumerable.Range(0, numberOfMasterDevices + numberOfNodeDevices).ToList();

        allPositions = allPositions.OrderBy(item => random.Next()).ToList();

        for (int i = 0; i < numberOfMasterDevices + numberOfNodeDevices; i++)
        {
            int z = (int) Mathf.Floor(allPositions[i] / gridWidth);
            int x = allPositions[i] % gridWidth;

            if (i < numberOfMasterDevices)
            {
                GameObject o = Instantiate(MasterDevicePrefab, new Vector3(x * gridSpacing, 0, z * gridSpacing), Quaternion.identity);
                o.name = "Master Device " + (i + 1);
                devices.Add(o);
            }
            else
            {
                GameObject o = Instantiate(NodeDevicePrefab, new Vector3(x * gridSpacing, 0, z * gridSpacing), Quaternion.identity);
                o.name = "Node Device " + (i - numberOfMasterDevices + 1);
                devices.Add(o);
            }
        }
    }
	
	void Update ()
    {
		
	}
}

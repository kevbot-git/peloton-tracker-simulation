using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBehaviour : MonoBehaviour
{
    public GameObject MasterDevicePrefab;
    public GameObject NodeDevicePrefab;

	void Start ()
    {
		for (int z = 0; z <= 2 ; z++)
        {
            for (int x = 0; x <= 2; x++)
            {
                GameObject o = Instantiate(MasterDevicePrefab, new Vector3((x - 1) * 4, 0, (z - 1) * 4), Quaternion.identity);
                o.name = "Device " + (z * 3 + x + 1);
            }
        }
	}
	
	void Update ()
    {
		
	}
}

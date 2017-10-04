using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBehaviour : MonoBehaviour {

	public GameObject This;
    public float ConnectionRange = 5.0f;

    private BluetoothDevice bluetoothDevice;

	// Use this for initialization
	void Start () {
		Debug.Log (This.name + " started");
        bluetoothDevice = new BluetoothDevice(This, ConnectionRange);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

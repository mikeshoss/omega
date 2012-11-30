using UnityEngine;
using System.Collections;

public class AttachTo : MonoBehaviour {
	
	public GameObject obj;
	public bool attachToX;
	public bool attachToY;
	public bool attachToZ;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate () {
		Vector3 v = transform.position;
		
		if (attachToX)
			v.x = obj.transform.position.x;
		if (attachToY)
			v.y = obj.transform.position.y;
		if (attachToZ)
			v.z = obj.transform.position.z;
		
		transform.position = v;
	}
}

using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	
	private Vector3 mMoveVelocity = new Vector3(0,0,0);
	public PlayerScript mPs;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {	
		
		//v.x = mPs.transform.position.x;
		
		
		float horOffset = (transform.position.x - mPs.transform.position.x) / (Screen.width);
		
		if (horOffset < -0.2 && mPs.Direction == 1)
		{
			transform.Translate(mPs.MoveVelocity * Time.deltaTime);
		} else if (horOffset > 0.2 && mPs.Direction == -1)
		{
			transform.Translate(mPs.MoveVelocity * Time.deltaTime);
		}
		
		Vector3 v = transform.position;
		v.y = mPs.transform.position.y;
		transform.position = v;

	}
}

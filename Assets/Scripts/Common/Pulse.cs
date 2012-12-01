using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {
	
	public float minimumRange = 0.9f;
	public float maximumRange = 1.2f;
	public float pulseSpeed = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = transform.localScale;
		
		v.x = minimumRange + Mathf.Sin (pulseSpeed * Time.time) * (maximumRange - minimumRange);
		v.y = minimumRange + Mathf.Sin (pulseSpeed * Time.time) * (maximumRange - minimumRange);
		
		
		transform.localScale = v;
	}
}

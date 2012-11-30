using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {
	
	public float degreesPerSecond;
	public bool randomizeRotationSpeed = true;
	// Use this for initialization
	void Start () {
		if (randomizeRotationSpeed)
			degreesPerSecond = Random.Range(0, 45);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, degreesPerSecond * Time.deltaTime);
	}
}

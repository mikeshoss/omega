using UnityEngine;
using System.Collections;

public class SnowflakeFall : MonoBehaviour {

	public float swayMagnitude;
	public float fallMagnitude;
	public float floatSpeed;
	
	public bool randomizeMagnitude = true;
	public bool randomizeScale = true;
	public bool randomizePosition = true;
	public bool randomizeFloatSpeed = true;
	
	private bool dealtWith;
	// Use this for initialization
	void Start () {
		if (randomizeMagnitude)
		{
			swayMagnitude = Random.Range(200, 300);
			fallMagnitude = Random.Range(500, 600);
			
			Debug.Log (swayMagnitude + " " + fallMagnitude);
		}
		
		if (randomizeScale)
		{
			Vector3 scale = gameObject.transform.localScale;
			scale *= Random.Range(0.3f, 0.6f);
			transform.localScale = scale;
		}
		
		if (randomizePosition)
		{
			
			transform.parent = GameObject.Find("BackgroundContainer").transform;
			Vector3 v = new Vector3(transform.parent.position.x + Random.Range(-1440, 1440), transform.parent.position.y + Random.Range(1000, 1200 * 2), -900);
			transform.position = v;
		}
		
		if (randomizeFloatSpeed)
		{
			floatSpeed = Random.Range(1, 4);	
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		Vector3 v = transform.position;
		
		v.x += Mathf.Sin(floatSpeed * Time.time) * (swayMagnitude * Time.deltaTime);
		v.y -= Mathf.Abs(Mathf.Sin(floatSpeed * Time.time)) * (fallMagnitude * Time.deltaTime);
		transform.position = v;
		
		if (v.y < transform.parent.position.y - 720 && !dealtWith)
		{
			dealtWith = true;
			Destroy(gameObject, 1);
			GameObject go = (GameObject)Instantiate(Resources.Load("Snowflake"));
		}
	}
}

using UnityEngine;
using System.Collections;
public class SnowflakeGenerator : MonoBehaviour {
	
	private const int MAX_SNOWFLAKE = 15;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < MAX_SNOWFLAKE; i++)
		{
			GameObject go = (GameObject)Instantiate(Resources.Load("Snowflake"));	
			go.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
}

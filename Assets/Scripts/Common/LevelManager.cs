using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private static bool mIsPaused = false;
	// Use this for initialization
	void Start () {
		GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
		
		foreach(GameObject go in nodes)
		{
			go.active = false;	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static bool IsPaused()
	{
		return mIsPaused;	
	}
	
	public static void SetPaused(bool val)
	{
		mIsPaused = val;	
	}
}

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Texture mTexture;
	
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
}

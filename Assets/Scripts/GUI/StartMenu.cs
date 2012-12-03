using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartMenu : MonoBehaviour {
	
	private int mSelected = 0;
	private const int MAX_SELECT = 1;
	public List<MenuOption> options;
	
	// Use this for initialization
	void Start () {
		options[0].SetTexture(1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.W))
		{
			options[mSelected].SetTexture(0);
			
			if (--mSelected < 0)
				mSelected = MAX_SELECT;
			
			options[mSelected].SetTexture(1);
		}
		
		if (Input.GetKeyDown(KeyCode.S))
		{
			options[mSelected].SetTexture(0);
			
			if (++mSelected > MAX_SELECT)
				mSelected = 0;
			
			options[mSelected].SetTexture(1);
		}
		
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (mSelected == 0)
			{
				Application.LoadLevel("level-01");	
			}
			
			if (mSelected == 1)
			{
				Application.Quit();	
			}
		}
		
	}
}

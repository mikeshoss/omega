using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartMenu : MonoBehaviour {
	
	private int mSelected = 0;
	private const int MAX_SELECT = 1;
	public List<GUITexture> optionGUI;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < optionGUI.Count; i++)
		{
			Rect r = optionGUI[i].pixelInset;
			r.width = 200;
			optionGUI[i].pixelInset = r;
			
			
			r = optionGUI[mSelected].pixelInset;
			r.width = 400;
			optionGUI[mSelected].pixelInset = r;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (--mSelected < 0)
				mSelected = MAX_SELECT;
			
			for (int i = 0; i < optionGUI.Count; i++)
			{
				Rect r = optionGUI[i].pixelInset;
				r.width = 200;
				optionGUI[i].pixelInset = r;
				
				
				r = optionGUI[mSelected].pixelInset;
				r.width = 400;
				optionGUI[mSelected].pixelInset = r;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.S))
		{
			if (++mSelected > MAX_SELECT)
				mSelected = 0;
			
			for (int i = 0; i < optionGUI.Count; i++)
			{
				Rect r = optionGUI[i].pixelInset;
				r.width = 200;
				optionGUI[i].pixelInset = r;
				
				
				r = optionGUI[mSelected].pixelInset;
				r.width = 400;
				optionGUI[mSelected].pixelInset = r;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (mSelected == 0)
			{
				Application.LoadLevel("example");	
			}
		}
		
	}
}

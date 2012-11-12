using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {
	
	public Texture	mHealthBar;
	public Texture  mEnergyBar;
	public Texture  mBarFrame;
	public Texture  mActionBarFrame;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI ()
	{
		GUI.DrawTexture(ResizeGUI(new Rect(420,620,400,93.75f)), mActionBarFrame);
		GUI.DrawTexture(ResizeGUI(new Rect(420,560,200,38)), mHealthBar);
		GUI.DrawTexture(ResizeGUI(new Rect(660,560,200,38)), mEnergyBar);
		GUI.DrawTexture(ResizeGUI(new Rect(420,560,200,38)), mBarFrame);
		GUI.DrawTexture(ResizeGUI(new Rect(660,560,200,38)), mBarFrame);
		
	}
	
	Rect ResizeGUI(Rect _rect)
	{
	    float FilScreenWidth = _rect.width / 1280;
	    float rectWidth = FilScreenWidth * Screen.width;
	    float FilScreenHeight = _rect.height / 720;
	    float rectHeight = FilScreenHeight * Screen.height;
	    float rectX = (_rect.x / 1280) * Screen.width;
	    float rectY = (_rect.y / 720) * Screen.height;
	
	    return new Rect(rectX,rectY,rectWidth,rectHeight);
	}
	// Update is called once per frame
	void Update () {
	
	}
}

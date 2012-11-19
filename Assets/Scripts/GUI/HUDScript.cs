using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {
	
	public GUITexture	mHealthBar;
	public GUITexture   mEnergyBar;
	public GUITexture   mHealthBarFrame;
	public GUITexture   mEnergyBarFrame;
	public GUITexture   mActionBarFrame;
	public PlayerScript mPlayer;
	
	private Rect	mActionBarInitialRect;
	private Rect	mHealthBarRect;
	private Rect	mEnergyBarRect;
	private Rect	mHealthBarFrameRect;
	private Rect	mEnergyBarFrameRect;
	// Use this for initialization
	void Start () {
		mActionBarInitialRect = mActionBarFrame.pixelInset;
		mHealthBarRect = mHealthBar.pixelInset;
		mHealthBarFrameRect = mHealthBarFrame.pixelInset;
		mEnergyBarRect = mEnergyBar.pixelInset;
		mEnergyBarFrameRect = mEnergyBarFrame.pixelInset;
		
	}
	
	void OnGUI ()
	{
		ResizeGUI(mActionBarFrame, mActionBarInitialRect);
		Rect r = mHealthBarRect;
		r.width *= (mPlayer.Health / mPlayer.MaxHealth);
		if (r.width < 0)
		{
			r.width = 0;	
		}
		ResizeGUI(mHealthBar, r);
		ResizeGUI(mHealthBarFrame, mHealthBarFrameRect);
		ResizeGUI(mEnergyBar, mEnergyBarRect);
		ResizeGUI(mEnergyBarFrame, mEnergyBarFrameRect);
	}

	
	void ResizeGUI(GUITexture texture, Rect r)
	{
		float FilScreenWidth = r.width / 1280;
	    float rectWidth = FilScreenWidth * Screen.width;
	    float FilScreenHeight = r.height / 720;
	    float rectHeight = FilScreenHeight * Screen.height;
	    float rectX = (r.x / 1280) * Screen.width;
	    float rectY = (r.y / 720) * Screen.height;
		
		texture.pixelInset = new Rect(rectX,rectY,rectWidth,rectHeight);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

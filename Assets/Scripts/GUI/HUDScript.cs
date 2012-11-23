using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {
	
	public GUITexture	mHealthBar;
	public GUITexture   mEnergyBar;
	public GUITexture   mHealthBarFrame;
	public GUITexture   mEnergyBarFrame;
	public GUITexture   mActionBarFrame;
	public GUITexture	mSkill1;
	public GUITexture	mSkill2;
	public GUITexture	mSkill3;
	public GUITexture	mSkill4;
	
	public PlayerScript mPlayer;
	
	private Rect	mActionBarInitialRect;
	private Rect	mHealthBarRect;
	private Rect	mEnergyBarRect;
	private Rect	mHealthBarFrameRect;
	private Rect	mEnergyBarFrameRect;
	private Rect	mSkillRect1;
	private Rect	mSkillRect2;
	private Rect	mSkillRect3;
	private Rect	mSkillRect4;
	
	// Use this for initialization
	void Start () {
		mActionBarInitialRect = mActionBarFrame.pixelInset;
		mHealthBarRect = mHealthBar.pixelInset;
		mHealthBarFrameRect = mHealthBarFrame.pixelInset;
		mEnergyBarRect = mEnergyBar.pixelInset;
		mEnergyBarFrameRect = mEnergyBarFrame.pixelInset;
		mSkillRect1 = mSkill1.pixelInset;
		mSkillRect2 = mSkill2.pixelInset;
		mSkillRect3 = mSkill3.pixelInset;
		mSkillRect4 = mSkill4.pixelInset;
		
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
		
			r = mEnergyBarRect;
			r.width *= (mPlayer.Energy / mPlayer.Player.MaxEnergy);
			if (r.width < 0)
			{
				r.width = 0;	
			}
			ResizeGUI(mEnergyBar, r);
			ResizeGUI(mEnergyBarFrame, mEnergyBarFrameRect);
			ResizeGUI(mSkill1, mSkillRect1);
			ResizeGUI(mSkill2, mSkillRect2);
			ResizeGUI(mSkill3, mSkillRect3);
			ResizeGUI(mSkill4, mSkillRect4);
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

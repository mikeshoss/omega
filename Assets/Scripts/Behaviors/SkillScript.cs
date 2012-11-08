using UnityEngine;
using System.Collections;

public abstract class SkillScript : MonoBehaviour {
	
	protected SkillAI			mAI;
	protected exSprite 			mSprite;
	protected Skill 			mSkill;
	protected CombatantScript 	mOrigin;
	protected CombatantScript	mTarget;
	protected bool				mIsActive;
	void Start ()
	{
		mSprite = (exSprite)GetComponent<exSprite>();
		mIsActive = false;
		mAI = (SkillAI)gameObject.AddComponent("SkillAI");
	}
	
	public Skill Skill
	{
		get
		{
			return mSkill;	
		}
		set
		{
			mSkill = value;	
		}
	}
	
	public CombatantScript Origin
	{
		get
		{
			return mOrigin;	
		}
		set
		{
			mOrigin = value;	
		}
	}
	
	public CombatantScript Target
	{
		get
		{
			return mTarget;	
		}
		set
		{
			mTarget = value;	
		}
	}
	
	public bool IsActive()
	{
		return mIsActive;
	}
	
	public void ShouldEnd ()
	{
		mIsActive = false;	
	}
	
	public abstract void Startup ();
	
	public abstract void Active ();
	
	public abstract void End ();
	
}//end class
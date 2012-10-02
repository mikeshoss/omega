using UnityEngine;
using System.Collections;


public abstract class ICombatant : MonoBehaviour {

	protected exSprite 			mSprite;
	protected int				mCurrentHitFlag;
	protected Vector3			mDirectionalVelocity;
	protected float				mMovingSpeed;
	protected Skill[]			mActiveSkills;
	protected string			mOrientation;
	
	void Start ()
	{
		mActiveSkills = new Skill[1];
		mCurrentHitFlag = 0;
		mDirectionalVelocity = new Vector3(0,0,0);
		mMovingSpeed = 0;
		mOrientation = "right";
		mSprite = (exSprite)GetComponent<exSprite>();
	}
	
	public void UseSkill (int index)
	{
		if (index >= 0 && index < mActiveSkills.Length)
		{
			
		}
	}
	
	public string GetOrientation ()
	{
		return mOrientation;	
	}
}

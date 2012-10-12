using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(CharacterController))]
public abstract class ICombatant : MonoBehaviour {
	
	protected MoveBehaviour mMovement;
	protected exSprite mSprite;
	
	protected HitFlag mHitFlag;
	protected List<SkillSet> mSkillSets = new List<SkillSet>();
	protected SkillSet mSelectedSkillSet;
	protected Skill mSelectedSkill;
	
	public MoveBehaviour movement
	{
		get
		{
			return mMovement;	
		}
		set
		{
			mMovement = value;	
		}
	}
	
	public HitFlag hitFlag
	{
		get
		{
			return mHitFlag;	
		}
		set
		{
			mHitFlag = value;	
		}
	}
	
	public enum HitFlag
	{
		GROUNDED,
		AERIAL
	}
	
	public enum AnimType
	{
		NONE,
		IDLE,
		MOVE,
		JUMP,
		ATTACK1,
		ATTACK2,
		ATTACK3,
		HIT1,
		HIT2,
		HIT3,
		DIE
	}
	
	void Start ()
	{
		mMovement = null;
		mHitFlag = HitFlag.GROUNDED;
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		mMovement.OnControllerColliderHit(hit);
	}
	
	public abstract void UpdateAnimation ();
	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CombatantType
{
	PLAYER,
	ENEMY
}

public class CombatantScript : MonoBehaviour {
	
	
	protected CharacterController 	mCharacter;
	protected exSprite				mSprite;
	protected List<Skill>			mSelectedSkills;
	protected List<Skill> 			mLearnedSkills;
	protected CombatantType			mCombatantType;
	
	protected Vector3				mMoveVelocity;
	
	protected float 				mHealth;
	protected float 				mEnergy; 
	
	protected int 					mDirection;
	protected int					mCurrentSkill;
	
	protected bool			 		mIsAirborne;
	protected bool[]				mIsSkillCooling = new bool[4];
	
	protected const float kGravity = 5000.0f;
	protected const float kFriction = 4000.0f;

	public exSprite Sprite
	{
		get
		{
			return mSprite;
		}
	}

	public List<Skill> SelectedSkills
	{
		get
		{
			return mSelectedSkills;
		}
	}

	public List<Skill> LearnedSkills
	{
		get
		{
			return mLearnedSkills;
		}
	}
	
	public CombatantType GetCombatantType
	{
		get
		{
			return mCombatantType;	
		}
	}
	
	public Vector3 MoveVelocity
	{
		get
		{
			return mMoveVelocity;
		}
	}

	public float Health
	{
		get
		{
			return mHealth;
		}
	}

	public float Energy
	{
		get
		{
			return mEnergy;
		}
	}

	public int Direction
	{
		get
		{
			return mDirection;
		}
	}

	public int CurrentSkill
	{
		get
		{
			return mCurrentSkill;
		}
	}

	public bool IsAirborne
	{
		get
		{
			return mIsAirborne;
		}
	}

	public bool[] IsSkillCooling
	{
		get
		{
			return mIsSkillCooling;
		}
	}
	
	public void ApplyDamage(float magnitude)
	{
		mHealth -= magnitude;		
	}
	
	public void ApplyHealing(float magnitude)
	{
		mHealth += magnitude;	
	}
}

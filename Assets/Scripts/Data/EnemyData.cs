using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill{
	
	
}

public class EnemyData {
	
	 /*
	 * Member Variables 
	 */
	private float mMaxHealth;
	private float mMaxEnergy;
	private List<Skill> mSkillsAble; //monster skills
	private Transform mSavedTransform; //not sure if this should be here?
	private int mExp;
	private int mLevel;
	 /*
	 * Constants
	 */	
	 // the monster experience player gets when killed?
	//Jump
	private const int kJumpMax = 1;
	private const float kJumpWaitTime = 0.0f; // 0.4 seconds
	private const float kJumpVelocity = 100.0f; // initial jump velocity
	//Run
	private const float kRunAccel = 50.0f;
	private const float kMaxRunSpeed = 500.0f;
	//Health
	private const float kBaseHealth = 95.0f; 
	private const float kHealthIncrement = 5.0f; // 5 percent
	//Energy
	private const float kBaseEnergy = 45.0f;
	private const float kEnergyIncrement = 5.0f; // 5 percent		
	
	/*
	 * Member Variable Properties
	 */
	
	public float MaxHealth
	{
		get
		{
			return mMaxHealth;	
		}
	}
	
	public float MaxEnergy
	{
		get
		{
			return mMaxEnergy;
		}
	}
	
	public int Level
	{
		get
		{
			return mLevel;
		}
	}
	
	public List<Skill> SkillsAble
	{
		get
		{
			return mSkillsAble;
		}
	}
	
	public Transform SavedTransform
	{
		set
		{
			mSavedTransform = value;	
		}
	}
	
	/*
	 * Constant Properties
	 */
	

	public int MaxJump
	{
		get
		{
			return kJumpMax;
		}
	}

	public float JumpWaitTime
	{
		get
		{
			return kJumpWaitTime;
		}
	}	

	public float JumpVelocity
	{
		get
		{
			return kJumpVelocity;	
		}
	}

	public float RunAcceleration
	{
		get
		{
			return kRunAccel;
		}
	}

	public float MaxRunSpeed
	{
		get
		{
			return kMaxRunSpeed;	
		}
	}

	public float BaseHealth
	{
		get
		{
			return kBaseHealth;	
		}
	}
	
	public EnemyData (int level, List<Skill> skillsAble)
	{
		mLevel = level;
		mMaxHealth = kBaseHealth + (level * kHealthIncrement);
		mMaxEnergy = kBaseEnergy + (level * kEnergyIncrement);
		mSkillsAble = skillsAble;
	}

	public EnemyData (int level, List<Skill> skillsAble, Transform mSavedPosition)
		: this(level, skillsAble)
	{
		mSavedPosition = mSavedPosition;
	}
	
	
	
}//end class





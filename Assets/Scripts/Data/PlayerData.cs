using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill {
	
}


public class PlayerData {
	
	private int[] kLevelCaps = new int[]
	{
		0,10,30,60,100,150,210,280,370,500
	};
	
	/*
	 * Member Variables 
	 */
	private float mMaxHealth; //100
	private float mMaxEnergy; //100
	private int mLevel;  //1
	private float mExp; //0
	private float mRequiredExp;
	private List<Skill> mSkillsLearned; //list of skills such as punch, fireball, barrier
	private List<Skill> mSkillsSelected;
	private Transform mSavedTransform;
	
	/*
	 * Constants
	 */
	private const int kLevelMax = 10;
	//Jump
	private const int kJumpMax = 2;
	private const float kJumpWaitTime = 0.4f; // 0.4 seconds
	private const float kJumpVelocity = 800.0f; // initial jump velocity
	//Run
	private const float kRunAccel = 50000.0f;
	private const float kMaxRunSpeed = 800.0f;
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
	
	public int CurrentLevel
	{
		get
		{
			return kLevelMax;	
		}
	}
	
	public float EXP
	{
		get
		{
			return mExp;	
		}
	}
	
	public float RequiredEXP
	{
		get
		{
			return mRequiredExp;	
		}
	}
	
	public List<Skill> SkillsLearned
	{
		get
		{
			return mSkillsLearned;
		}
	}
	
	public List<Skill> SkillsSelected
	{
		get
		{
			return mSkillsSelected;	
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
	public int MaxLevel
	{
		get
		{
			return kLevelMax;	
		}
	}
	
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
	
	public float HealthIncrement
	{
		get
		{
			return kHealthIncrement;	
		}
	}
	
	public float BaseEnergy
	{
		get
		{
			return kBaseEnergy;
		}
	}
	
	public float EnergyIncrement
	{
		get
		{
			return kEnergyIncrement;	
		}
	}
	
	public PlayerData (int level, List<Skill> skillsLearned, List<Skill> skillsSelected)
	{
		mLevel = level;
		mMaxHealth = kBaseHealth + (level * kHealthIncrement);
		mMaxEnergy = kBaseEnergy + (level * kEnergyIncrement);
		mSkillsLearned = skillsLearned;
		mSkillsSelected = skillsSelected;
	}
	
	public PlayerData (int level, List<Skill> skillsLearned, List<Skill> skillsSelected, Transform mSavedPosition)
		: this(level, skillsLearned, skillsSelected)
	{
		mSavedPosition = mSavedPosition;
	}
}

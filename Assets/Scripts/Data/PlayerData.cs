using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill {
	
}


public class PlayerData {
	
	/*
	 * Member Variables 
	 */
	private float mHealth; //100
	private float mEnergy; //100
	private int mLevel;  //1
	private List<Skill> mSkillsLearned; //list of skills such as punch, fireball, barrier
	private List<Skill> mSkillsSelected;
	private Transform mSavedPosition;
	
	/*
	 * Constants
	 */
	private const int kLevelMax = 10;
	private const int kMaxJump = 2;
	private const float kJumpWait = 0.4f; // 0.4 seconds
	private const float kJumpVelocity = 100.0f;
	
	private const float kBaseHealth = 95.0f; 
	private const float kHealthIncrement = 5.0f; // 5 percent
	private const float kBaseEnergy = 45.0f;
	private const float kEnergyIncrement = 5.0f; // 5 percent
	
	
	
	public PlayerData (int level, List<Skill> skillsLearned, List<Skill> skillsSelected)
	{
		mLevel = level;
		mHealth = kBaseHealth + (level * kHealthIncrement);
		mEnergy = kBaseEnergy + (level * kEnergyIncrement);
		mSkillsLearned = skillsLearned;
		mSkillsSelected = skillsSelected;
	}
	
	public PlayerData (int level, List<Skill> skillsLearned, List<Skill> skillsSelected, Transform mSavedPosition)
		: this(level, skillsLearned, skillsSelected)
	{
		mSavedPosition = mSavedPosition;
	}
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum SkillList
{
	FIREBALL,
	ICEPICK,
	NUM_SKILLS
}

enum AnimType
{
	ATTACK,
	DAMAGE_LIGHT,
	DAMAGE_HEAVY,
	MOVE_RUN,
	MOVE_IDLE,
	MOVE_JUMP,
	NUM_ANIMATION_TYPES
}

public class Skill {
	
	private CombatantScript mOrigin;

	private int 	mId;
	
	private string 	mName;
	private string 	mDesc;
	private float 	mMagnitude; // used to evaluate the strength of a skill

	private float 	mOriginDelay;
	
	private float	mTargetRecoveryTime; // The amount of time a target is unable to respond
	
	private float 	mStartupTime; // The amount of time the skill is in startup
	private float   mActiveTime; // The amount of time the skill is in active
	private float 	mEndTime; 	  // The amount of time the skill is in end

	private string 	mResource;
	
	/*Member variable properties*/
	public string Name
	{
		get
		{
			return mName;
		}
	}

	public string Description
	{
		get
		{
			return mDesc;
		}
	}

	public float Magnitude
	{
		get
		{
			return mMagnitude;
		}
	}

	public float OriginDelay
	{
		get
		{
			return mOriginDelay;
		}
	}

	public float TargetRecoveryTime
	{
		get
		{
			return mTargetRecoveryTime;
		}
	}

	public float StartupTime
	{
		get
		{
			return mStartupTime;
		}
	}

	public float ActiveTime
	{
		get
		{
			return mActiveTime;
		}
	}
	
	public float EndTime
	{
		get
		{
			return mEndTime;	
		}
	}
	
	public string Resource
	{
		get
		{
			return mResource;	
		}
	}
	
	public GameObject InstantiateObject()
	{
		Debug.Log ("Instantiate " + mResource);
		return (GameObject)MonoBehaviour.Instantiate(Resources.Load(mResource));	
	}
	
	public Skill(int id, string name, string desc, float magnitude, 
		CombatantScript origin, float originDelay,
		float targetRecoveryTime, float skillStartupTime, float skillActiveTime, float skillEndTime,
		string resource)
	{
		mId = id;
		mName = name;
		mDesc = desc;
		mMagnitude = magnitude;
		origin = mOrigin;
		mOriginDelay = originDelay;
		mTargetRecoveryTime = targetRecoveryTime;
		mStartupTime = skillStartupTime;
		mActiveTime = skillActiveTime;
		mEndTime = skillEndTime;
		mResource = resource;
	}

	
}//end class


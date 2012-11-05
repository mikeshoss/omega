using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillData {
	
	private int mSkillId;
	private string mSkillName;
	private string mSKillDesc;
	private int mSkillCurLevel;
	private int mSkillMaxLevel;
	private float mSkillMagnitude;
	private float mSkillRange;
	private string mSkillTypeInfo;
	private float mSkillSplRange;
	private float mSkillSplMagnitude;
	private bool mSkillFollowOrigin;
	private float mSkillDelay;
	private int mSkillElement; //should the element types be set with ID's? ex water : 1 earth : 2 jizz : 9
	
	
	/*Member variable properties*/
	
	public string SkillName
	{
		get
		{
			return mSkillName;	
		}
	}
	
	public int SkillId
	{
		get
		{
			return mSkillId;	
		}
	}
	
	public string SkillDesc
	{
		get
		{
			return mSKillDesc;
		}
	}
	
	public int SkillCurLevel
	{
		get
		{
			return mSkillCurLevel;
		}
	}
	
	public int SkillMaxLevel
	{
		get
		{
			return mSkillMaxLevel;
		}
	}
	
		public float SkillMagnitude
	{
		get
		{
			return mSkillMagnitude;
		}
	}
	
	public float SkillRange
	{
		get
		{
			return mSkillRange;
		}
	}
	
	public string SkillTypeInfo
	{
		get
		{
			return mSkillTypeInfo;
		}
	}
		
	public float SkillSplRange
	{
		get
		{
			return mSkillSplRange;
		}
	}
	
	public float SkillSplMagnitude
	{
		get
		{
			return mSkillSplMagnitude;
		}
	}
	
	public bool SkillFollowOrigin
	{
		get
		{
			return mSkillFollowOrigin;
		}
	}
	
	public float SkillDelay
	{
		get
		{
			return mSkillDelay;
		}
	}
	
	public int SkillElement
	{
		get
		{
			return mSkillElement;
		}
	}
	
	public SkillData(int id, string name, string desc, int curLevel, int maxLevel, float magnitude, float range, string typeinfo, float splRange, float splMagnitude, bool followOrigin, float delay, int element)
	{
	 mSkillId = id;
	 mSkillName = name;
	 mSKillDesc = desc;
	 mSkillCurLevel = curLevel;
	 mSkillMaxLevel = maxLevel;
	 mSkillMagnitude = magnitude;
	 mSkillRange = range;
	 mSkillTypeInfo = typeinfo;
	 mSkillSplRange = splRange;
	 mSkillSplMagnitude = splMagnitude;
	 mSkillFollowOrigin = followOrigin;
	 mSkillDelay = delay;
	 mSkillElement = element;
	}
	
}//end class


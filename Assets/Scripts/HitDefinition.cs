using UnityEngine;
using System.Collections;

public class HitDefinition {
	
	private ICombatant.HitFlag[] mHitFlags;
	private int mAnimType;
	private Priority mPriority;
	
	public ICombatant.HitFlag[] hitFlags
	{
		get
		{
			return mHitFlags;	
		}
	}
	
	public int animType
	{
		get
		{
			return mAnimType;	
		}
	}
	
	public Priority priority
	{
		get
		{
			return mPriority;	
		}
	}
	
	public HitDefinition () {
		mHitFlags = new ICombatant.HitFlag[2];
		mHitFlags[0] = ICombatant.HitFlag.GROUNDED;
		mHitFlags[1] = ICombatant.HitFlag.AERIAL;
	}
	
	public HitDefinition (ICombatant.HitFlag[] hitFlags, int animType, Priority priority)
	{
		mHitFlags = hitFlags;
		mAnimType = animType;
		mPriority = priority;
	}
}

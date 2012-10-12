using UnityEngine;
using System.Collections;

public class HitDefinition {
	
	private ICombatant.HitFlag[] mHitFlags;
	private ICombatant.AnimType mAnimType;
	private Priority mPriority;
	
	public ICombatant.HitFlag[] hitFlags
	{
		get
		{
			return mHitFlags;	
		}
	}
	
	public ICombatant.AnimType animType
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
	
	public HitDefinition (ICombatant.HitFlag[] hitFlags, ICombatant.AnimType animType, Priority priority)
	{
		mHitFlags = hitFlags;
		mAnimType = animType;
		mPriority = priority;
	}
}

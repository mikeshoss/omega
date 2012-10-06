using UnityEngine;
using System.Collections;

public abstract class BaseBehaviour {
	
	protected ICombatant mOrigin;
	
	protected bool mIsEnabled;
	
	protected BType mBehaviourType;
	
	public enum BType
	{
		NONE,
		AIR_MOVE_BEHAVIOUR,
		GROUND_MOVE_BEHAVIOUR
	}
	
	public BaseBehaviour (ICombatant origin)
	{
		mOrigin = origin;
		mIsEnabled = true;
		mBehaviourType = BType.NONE;
	}
	
	public abstract void Update ();
	
	public void Enable ()
	{
		mIsEnabled = true;	
	}
	
	public void Disable ()
	{
		mIsEnabled = false;	
	}
	
	public BType GetBType()
	{
		return mBehaviourType;
	}
	
}
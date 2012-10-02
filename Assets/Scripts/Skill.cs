using UnityEngine;
using System.Collections;

public abstract class Skill {
	
	public enum Element
	{
		NONE,
		FIRE,
		ICE
	};
	
	protected int 			mDamage;
	protected Element 		mElement;
	protected HitDefinition mHitDef;
	protected ICombatant	mOrigin;
	
	public Skill (ICombatant origin)
	{
		mOrigin = origin;
	}
	
	public ICombatant GetOrigin ()
	{
		return mOrigin;	
	}
}

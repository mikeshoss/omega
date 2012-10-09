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
	
	public int damage
	{
		get
		{
			return mDamage;	
		}
	}
	
	public Element element
	{
		get
		{
			return mElement;	
		}
	}
	
	public HitDefinition hitDefinition
	{
		get
		{
			return mHitDef;	
		}
	}
	
	public ICombatant origin
	{
		get
		{
			return mOrigin;	
		}
	}
	
	
	public Skill (ICombatant origin)
	{
		mOrigin = origin;
	}
	
	public Skill (ICombatant origin, int damage, Element element, HitDefinition hitDef)
		: this(origin)
	{
		mDamage = damage;
		mElement = element;
		mHitDef = hitDef;
	}
}

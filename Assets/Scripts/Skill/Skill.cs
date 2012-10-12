using UnityEngine;
using System.Collections;

public abstract class Skill {
	
	public enum Element
	{
		NONE,
		FIRE,
		ICE
	};
	
	public enum SkillType
	{
		FIREBALL,
		ICEPICK,
		TOTAL_SKILLS
	};
	
	protected int 			mBaseDamage;
	protected int			mCalcDamage;
	protected int			mLevel;
	protected Element 		mElement;
	protected HitDefinition mHitDef;
	protected ICombatant	mOrigin;
	protected SkillType 	mSkillType;
	
	public int baseDamage
	{
		get
		{
			return mBaseDamage;	
		}
	}
	
	public int calculatedDamage
	{
		get
		{
			return mCalcDamage;	
		}
		set
		{
			mCalcDamage = value;	
		}
	}
	
	public int level
	{
		get
		{
			return mLevel;	
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
		set
		{
			mOrigin = value;	
		}
	}
	
	
	public Skill (
		ICombatant origin,
		int baseDamage,
		int calcDamage,
		int level,
		Element element, 
		HitDefinition hitDef)
	{
		mOrigin = origin;
		mBaseDamage = baseDamage;
		mCalcDamage = calcDamage;
		mLevel = level;
		mElement = element;
		mHitDef = hitDef;
	}
	
	public abstract void Execute ();
	
	public abstract void Print ();
}

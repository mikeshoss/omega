using UnityEngine;
using System.Collections;

public class Icepick : Skill {
	
	public Icepick (
		ICombatant origin,
		int baseDamage,
		int calcDamage,
		int level,
		Element element, 
		HitDefinition hitDef)
		: base (origin,baseDamage,calcDamage,level,element,hitDef)
	{
		mSkillType = Skill.SkillType.ICEPICK;
	}
	
	public override void Execute ()
	{
		GameObject go = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Icepick"));
		
		SkillScript ss = (SkillScript)go.GetComponent<SkillScript>();
		ss.Initialize(this);
	}
	
	public override void Print ()
	{
		Debug.Log ("icepick");	
	}
}

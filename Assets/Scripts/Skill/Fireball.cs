using UnityEngine;
using System.Collections;

public class Fireball : Skill {
	
	public Fireball (
		ICombatant origin,
		int baseDamage,
		int calcDamage,
		int level,
		Element element, 
		HitDefinition hitDef)
		: base (origin,baseDamage,calcDamage,level,element,hitDef)
	{
		mSkillType = Skill.SkillType.FIREBALL;
	}
	
	public override void Execute ()
	{
		GameObject go = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Fireball"));
		
		SkillScript ss = (SkillScript)go.GetComponent<SkillScript>();
		ss.Initialize(this);
	}
	
	public override void Print ()
	{
		Debug.Log ("fireball");	
	}
}

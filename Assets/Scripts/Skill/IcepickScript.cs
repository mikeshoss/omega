using UnityEngine;
using System.Collections;

[RequireComponent (typeof(exSprite))]
public class IcepickScript : SkillScript {
	
	private int direction = 1;
	
	protected override void Execute ()
	{
		transform.Translate(direction * 1000 * Time.deltaTime,0,0);
	}
	
	public override void Initialize (Skill skill)
	{
		mSkill = skill;
		
		if (mSkill != null)
		{
			ICombatant origin = mSkill.origin;
			
			direction = origin.movement.direction;
			
			Vector3 localScale = transform.localScale;
			localScale.x = Mathf.Abs(transform.localScale.x) * direction;
			transform.localScale = localScale;
			
			exSprite sprite = GetComponent<exSprite>();
			
			Vector3 originLocation = origin.transform.position;
			
			originLocation.x += direction * 150;
			transform.position = originLocation;
		}
	}
}

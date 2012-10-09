using UnityEngine;
using System.Collections;

public class Fireball : Skill {
	
	public Fireball (ICombatant origin) 
		: base (origin, 10, Skill.Element.FIRE, new HitDefinition())
	{
	}
	
}

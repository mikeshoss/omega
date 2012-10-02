using UnityEngine;
using System.Collections;

[RequireComponent (typeof(exSprite))]
public class FireballScript : SkillScript {
	
	private int direction = 1;
	
	void Start () 
	{
		ICombatant origin = mSkill.GetOrigin();
		mSprite = GetComponent<exSprite>();
		
		Vector3 originLocation = origin.transform.position;
		
		if (origin.GetOrientation() == "left")		
			direction = -1;	
		
		originLocation.x += direction * 150;
		transform.position = originLocation;
		origin.GetComponent<exSprite>();
	}
	
	protected override void Execute ()
	{
		transform.Translate(direction * 1000 * Time.deltaTime,0,0);
	}
}

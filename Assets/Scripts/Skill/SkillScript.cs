using UnityEngine;
using System.Collections;

[RequireComponent (typeof(exSprite))]
public abstract class SkillScript : MonoBehaviour {
	
	protected Skill mSkill;
	
	void Update ()
	{
		if (mSkill != null)
			Execute();
	}
	
	protected abstract void Execute ();
	
	public abstract void Initialize (Skill skill);
}

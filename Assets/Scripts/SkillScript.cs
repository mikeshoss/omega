using UnityEngine;
using System.Collections;

[RequireComponent (typeof(exSprite))]
public abstract class SkillScript : MonoBehaviour {
	
	protected Skill 	mSkill;
	protected exSprite 	mSprite;
	
	void Update ()
	{
		Execute();
	}
	
	protected abstract void Execute ();
	
	public void SetSkill (Skill skill)
	{
		mSkill = skill;	
	}
}

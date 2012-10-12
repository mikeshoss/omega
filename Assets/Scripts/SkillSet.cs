using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {

	private List<Skill> mSkills = new List<Skill>();
	
	public SkillSet (params Skill[] skills)
	{
		for (int i = 0; i < skills.Length; i++)
		{
			mSkills.Add(skills[i]);	
		}
	}
	
	public Skill GetSkill (int index)
	{
		return mSkills[index];
	}
}

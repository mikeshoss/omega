using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData {
	
	private List<Skill> mAllSkills = new List<Skill>();
	private List<SkillSet> mSkillSets = new List<SkillSet>();
	private SkillSet mSelectedSkillSet;
	
	public List<SkillSet> skillSets
	{
		get
		{
			return mSkillSets;	
		}
	}
	
	public SkillSet selectedSkillSet
	{
		get
		{
			return mSelectedSkillSet;	
		}
	}
	
	public PlayerData (PlayerScript ps)
	{
		ICombatant origin = (ICombatant)ps;
		
		Skill fireball = new Fireball(ps,100,100,1,Skill.Element.FIRE,new HitDefinition());
		Skill icepick = new Icepick(ps,100,100,1,Skill.Element.ICE,new HitDefinition());
		
		SkillSet ss = new SkillSet(fireball, icepick);
		
		mSkillSets.Add(ss);
		
		mAllSkills.Add(fireball);
		mAllSkills.Add(icepick);
		
		mSelectedSkillSet = ss;
	}
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class SkillAI : MonoBehaviour, IAgent {
	
	private SkillScript mSkill;
	private Tree tree;

	IEnumerator Start ()
	{
		mSkill = (SkillScript)GetComponent<SkillScript>();
		Debug.Log("Shit Started");
		tree = BLBehaveLib.InstantiateTree(BLBehaveLib.TreeType.AI_Skill, this);
		
		while (Application.isPlaying && tree != null)
		{
			yield return null;
			AIUpdate();	
		}
	}
	
	void AIUpdate()
	{
		tree.Tick();
	}
	
	public void Reset (Tree sender)
	{
		
	}
	
	public int SelectTopPriority (Tree sender, params int[] IDs)
	{
		return 0;
	}
	
	public BehaveResult Tick (Tree sender, bool init)
	{
		Debug.Log ("Skill ticked");
		return BehaveResult.Failure;
	}
	
	/*
	 * DECORATORS
	 */
	/*
	 * Game Running
	 */
	public BehaveResult TickGameRunningDecorator (Tree sender)
	{
		return BehaveResult.Success;	
	}
	
	public BehaveResult TickStartupAction (Tree sender)
	{
		mSkill.Startup();
		return BehaveResult.Success;
	}
	
	public BehaveResult TickActiveAction (Tree sender)
	{
		mSkill.Active();
		
		if (!mSkill.IsActive())
		{
			return BehaveResult.Success;
		}	
		return BehaveResult.Running;
	}
	
	public BehaveResult TickEndAction (Tree sender)
	{
		mSkill.End();
		return BehaveResult.Success;
	}
}
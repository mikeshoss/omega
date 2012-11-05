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
		tree = BLBehaveLib.InstantiateTree(BLBehaveLib.TreeType.AI_Skill, this);
		
		while (Application.isPlaying && tree != null)
		{
			yield return new WaitForSeconds(1.0f / tree.Frequency);
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
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickActiveAction (Tree sender)
	{
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickEndAction (Tree sender)
	{
		return BehaveResult.Failure;
	}
}
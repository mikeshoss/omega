using UnityEngine;
using System.Collections;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class ImplBehaveExample : MonoBehaviour, IAgent {
	
	Tree tree;
	PlayerScript player;
	
	IEnumerator Start ()
	{	
		player = (PlayerScript)GetComponent<PlayerScript>();
		tree = BLBehaveLib.InstantiateTree(BLBehaveLib.TreeType.AI_Player, this);
		
		while (Application.isPlaying && tree != null)
		{
			yield return null;
			AIUpdate();
		}
	}
	
	
	public void AIUpdate()
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
	 * Action Elements
	 */
	
	
	public BehaveResult TickCheckRequestingAttackDecorator (Tree sender)
	{
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickDoAttackAction (Tree sender)
	{
		Debug.Log ("DoAttack");
		return BehaveResult.Success;
	}
	
	
	/*
	 * Jump Behavior
	 */
	
	/*
	 * Condition: Jump Max
	 */
	public BehaveResult TickJumpMaxAction (Tree sender)
	{
		//Debug.Log ("Jump Max");
		if (player.ConditionJumpMax())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}
	
	/*
	 * Condition: Jump Wait
	 */
	public BehaveResult TickJumpWaitAction (Tree sender)
	{
		//Debug.Log ("Jump Wait");
		if (player.ConditionJumpWait())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}
	
	/*
	 * Condition: Input
	 */
	public BehaveResult TickJumpInputAction (Tree sender)
	{
		//Debug.Log ("Jump Input");
		if (player.ConditionJumpInput())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}

	/*
	 * Animate
	 */
	public BehaveResult TickJumpAnimateAction (Tree sender)
	{
		//Debug.Log ("Jump Animate");
		player.AnimateJump();
		return BehaveResult.Success;
	}
	
	/*
	 * Action
	 */
	public BehaveResult TickJumpAction (Tree sender)
	{
		//Debug.Log ("Jump Animate");
		player.ActionJump();
		return BehaveResult.Success;
	}
	
	
	/*
	 * Run Behavior
	 */
	
	/*
	 * Condition
	 */
	public BehaveResult TickRunInputAction (Tree sender)
	{
		//Debug.Log ("Run Condition");
		if (player.ConditionRunInput ())
			return BehaveResult.Success;
		else
			return BehaveResult.Failure;
	}
	
	/*
	 * Action
	 */
	public BehaveResult TickRunAction (Tree sender)
	{
		//Debug.Log ("Run Action");
		player.ActionRun();
		return BehaveResult.Success;
	}
	
	/*
	 * Animate
	 */
	public BehaveResult TickRunAnimateAction (Tree sender)
	{
		//Debug.Log ("Run Animate");
		player.AnimateRun();
		return BehaveResult.Success;
	}
	
	
	/*
	 * Idle Behavior
	 */
	
	/*
	 * Condition
	 */
	public BehaveResult TickIdleCheckAction (Tree sender)
	{
		//Debug.Log ("Idle Check");
		if (player.ConditionIdleCheck())
			return BehaveResult.Success;
		else
			return BehaveResult.Failure;
	}
	
	/*
	 * Action
	 */
	public BehaveResult TickIdleAction (Tree sender)
	{
		//Debug.Log ("Idle Action");
		return BehaveResult.Success;
	}
	
	/*
	 * Animate
	 */
	public BehaveResult TickIdleAnimateAction (Tree sender)
	{
		//Debug.Log ("Idle Animate");
		
		player.AnimateIdle();
		return BehaveResult.Success;
	}
	
	
	/*
	 * Attack
	 */
	
	/*
	 * Condition
	 */
	public BehaveResult TickAttackConditionAction (Tree sender)
	{
		return BehaveResult.Failure;	
	}
}

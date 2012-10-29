using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class EnemyAI : MonoBehaviour, IAgent {
	
	private EnemyScript mEnemy;
	private Tree tree;

	IEnumerator Start ()
	{
		mEnemy = (EnemyScript)GetComponent<EnemyScript>();
		tree = BLBehaveLib.InstantiateTree(BLBehaveLib.TreeType.AI_Enemy, this);
		
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
	
	/*
	 * Dead or Alive
	 */
	
	/*
	 * DEAD
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickIsDeadAction (Tree sender)
	{
		if (mEnemy.IsDead())
			return BehaveResult.Success;	
		return BehaveResult.Failure;	
	}
	/*
	 * Actions
	 */
	public BehaveResult TickDeadAction (Tree sender)
	{
		mEnemy.DeadAction();
		return BehaveResult.Running;
	}
	
	/*
	 * DYING
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickIsDyingAction (Tree sender)
	{
		if (mEnemy.IsDying())
			return BehaveResult.Success;
		return BehaveResult.Failure;	
	}
	/*
	 * Actions
	 */
	public BehaveResult TickAnimateDieAction (Tree sender)
	{
		mEnemy.AnimateDie();
		return BehaveResult.Success;	
	}
	public BehaveResult TickDieAction (Tree sender)
	{
		return BehaveResult.Running;	
	}
	
	/*
	 * ALIVE
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickIsAliveAction (Tree sender)
	{
		return BehaveResult.Success;
	}
	
	/*
	 * ACTION
	 */
	/*
	 * HOSTILE
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickIsHostileAction (Tree sender)
	{
		return BehaveResult.Failure;	
	}
	
	/*
	 * ATTACK
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickIsSkillSelectedAction (Tree sender)
	{
		return BehaveResult.Failure;	
	}
	public BehaveResult TickIsInRangeAction (Tree sender)
	{
		return BehaveResult.Failure;
	}
	/*
	 * Actions
	 */
	public BehaveResult TickAnimateAttackAction (Tree sender)
	{
		return BehaveResult.Success;
	}
	public BehaveResult TickAttackAction (Tree sender)
	{	
		return BehaveResult.Success;
	}
	
	
	/*
	 * SKILL
	 */
	/*
	 * Conditions
	 */
	/*
	 * Actions
	 */
	public BehaveResult TickSelectSkillAction (Tree sender)
	{
		return BehaveResult.Success;
	}
	
	/*
	 * SEEK
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickIsSeekingAction (Tree sender)
	{
		return BehaveResult.Failure;	
	}
	/*
	 * Actions
	 */
	public BehaveResult TickSetDesiredSeekLocationAction (Tree sender)
	{
		return BehaveResult.Success;	
	}
	public BehaveResult TickFindPathAction(Tree sender)
	{
		return BehaveResult.Success;
	}
	public BehaveResult TickAnimateSeekAction(Tree sender)
	{
		return BehaveResult.Success;
	}
	public BehaveResult TickSeekAction(Tree sender)
	{
		return BehaveResult.Success;
	}
	
	/*
	 * WANDER
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickIsWanderingAction (Tree sender)
	{
		return BehaveResult.Failure;
	}
	/*
	 * Actions
	 */
	public BehaveResult TickSetDesiredWanderLocationAction (Tree sender)
	{	
		return BehaveResult.Success;
	}
	/*
	 * FindPath defined above
	 */
	public BehaveResult TickAnimateWanderAction (Tree sender)
	{
		return BehaveResult.Success;
	}
	public BehaveResult TickWanderAction (Tree sender)
	{
		return BehaveResult.Success;
	}
	
	/*
	 * IDLE
	 */
	/*
	 * Conditions
	 */
	/*
	 * Actions
	 */
	public BehaveResult TickAnimateIdleAction(Tree sender)
	{
		return BehaveResult.Success;
	}
	public BehaveResult TickIdleAction(Tree sender)
	{
		return BehaveResult.Success;
	}
}
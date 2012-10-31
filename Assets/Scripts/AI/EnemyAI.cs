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
		mEnemy.DieAction();
		
		if (mEnemy.IsDead())
			return BehaveResult.Success;
		
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
		if (mEnemy.IsAlive())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}
	
	//TODO:
	
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
		if (mEnemy.IsHostile())
			return BehaveResult.Success;
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
		if (mEnemy.IsSkillSelected())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;	
	}
	public BehaveResult TickIsInRangeAction (Tree sender)
	{
		if (mEnemy.IsEnemyInRange())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}
	/*
	 * Actions
	 */
	public BehaveResult TickAnimateAttackAction (Tree sender)
	{
		mEnemy.AnimateAttack();
		return BehaveResult.Success;
	}
	public BehaveResult TickAttackAction (Tree sender)
	{	
		mEnemy.AttackAction();
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
		mEnemy.SelectSkill();
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
		if (mEnemy.IsSeeking())
			return BehaveResult.Success;

		return BehaveResult.Failure;	
	}
	/*
	 * Actions
	 */
	public BehaveResult TickSetDesiredSeekLocationAction (Tree sender)
	{
		mEnemy.SetDesiredSeekLocation();
		return BehaveResult.Success;	
	}
	public BehaveResult TickFindPathAction(Tree sender)
	{
		mEnemy.FindPath();
		return BehaveResult.Success;
	}
	public BehaveResult TickAnimateSeekAction(Tree sender)
	{
		mEnemy.AnimateSeek();
		return BehaveResult.Success;
	}
	public BehaveResult TickSeekAction(Tree sender)
	{
		mEnemy.SeekAction();
		return BehaveResult.Success;
	}
	
	/*
	 * WANDER
	 */
	/*
	 * Conditions
	 */
	/*
	 * Actions
	 */
	public BehaveResult TickSetDesiredWanderLocationAction (Tree sender)
	{	
		mEnemy.SetDesiredWanderLocation();
		return BehaveResult.Success;
	}
	/*
	 * FindPath defined above
	 */
	public BehaveResult TickAnimateWanderAction (Tree sender)
	{
		mEnemy.AnimateWander();
		return BehaveResult.Success;
	}
	public BehaveResult TickWanderAction (Tree sender)
	{
		mEnemy.WanderAction();
		
		if (!mEnemy.IsWandering()) {
			return BehaveResult.Success;
		}
		if (mEnemy.IsHostile())
		{
			mEnemy.IdleAction();
			return BehaveResult.Failure;	
		}
		return BehaveResult.Running;
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
		mEnemy.AnimateIdle();
		return BehaveResult.Success;
	}
	public BehaveResult TickIdleAction(Tree sender)
	{
		mEnemy.IdleAction();
		
		if (!mEnemy.IsIdle()) {
			return BehaveResult.Success;
		}
		if (mEnemy.IsHostile())
		{
			return BehaveResult.Failure;	
		}
		return BehaveResult.Running;
	}
}
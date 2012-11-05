using UnityEngine;
using System.Collections;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class PlayerAI : MonoBehaviour, IAgent {
	
	private PlayerScript mPlayer;
	private Tree tree;
	
	IEnumerator Start ()
	{
		mPlayer = (PlayerScript)GetComponent<PlayerScript>();
		tree = BLBehaveLib.InstantiateTree(BLBehaveLib.TreeType.AI_Player, this);
		
		while (Application.isPlaying && tree != null)
		{
			yield return new WaitForSeconds(0.0f);
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
	 * Not Airborne
	 */
	public BehaveResult TickNotAirborneDecorator (Tree sender)
	{
		if (!mPlayer.IsAirborne())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
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
		return BehaveResult.Failure;	
	}
	/*
	 * Actions
	 */
	public BehaveResult TickDeadAction (Tree sender)
	{
		return BehaveResult.Running;
	}
	
	/*
	 * DYING
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickShouldDieAction (Tree sender)
	{
		return BehaveResult.Failure;	
	}
	/*
	 * Actions
	 */
	public BehaveResult TickAnimateDieAction (Tree sender)
	{
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
	 * ACTION
	 */

	/*
	 * ATTACK
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickCheckAttackInputAction (Tree sender)
	{
		if (mPlayer.CheckAttackInput())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}
	public BehaveResult TickCheckAttackUsableAction (Tree sender)
	{
		Debug.Log("Check if Attack is Usable");
		mPlayer.CheckAttackCooldown();
		return BehaveResult.Failure;
	}
	/*
	 * Actions
	 */
	public BehaveResult TickAnimateAttackAction (Tree sender)
	{
		Debug.Log("Check if Attack Animation is carried out");
		mPlayer.AnimateAttack();
		return BehaveResult.Success;
	}
	public BehaveResult TickAttackAction (Tree sender)
	{
		mPlayer.Attack();
		return BehaveResult.Success;
	}
	
	/*
	 * JUMP
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickCheckJumpInputAction (Tree sender)
	{
		if (mPlayer.CheckJumpInput())
			return BehaveResult.Success;	
		
		return BehaveResult.Failure;
	}
	public BehaveResult TickCheckJumpWaitTimeAction (Tree sender)
	{
		if (mPlayer.CheckJumpWaitTime())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}	
	public BehaveResult TickCheckJumpMaxAction (Tree sender)
	{
		if (mPlayer.CheckJumpMax())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}
	/*
	 * Actions
	 */
	public BehaveResult TickAnimateJumpAction (Tree sender)
	{
		mPlayer.AnimateJump();
		return BehaveResult.Success;
	}
	public BehaveResult TickJumpAction (Tree sender)
	{	
		mPlayer.JumpAction();
		return BehaveResult.Success;
	}
	
	/*
	 * GROUNDED ACTION
	 */
	
	/*
	 * RUN
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickCheckRunInputAction(Tree sender)
	{
		if(mPlayer.CheckRunInput())
			return BehaveResult.Success;	
		
		return BehaveResult.Failure;
	}
	/*
	 * Actions
	 */
	public BehaveResult TickAnimateRunAction(Tree sender)
	{
		mPlayer.AnimateRun();
		return BehaveResult.Success;
	}
	public BehaveResult TickRunAction(Tree sender)
	{
		mPlayer.RunAction();
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
		mPlayer.AnimateIdle();
		return BehaveResult.Success;
	}
	public BehaveResult TickIdleAction(Tree sender)
	{
		mPlayer.IdleAction();
		return BehaveResult.Success;
	}
	
	/*
	 * SKILL
	 */
	/*
	 * Conditions
	 */
	public BehaveResult TickCheckSkillInputAction (Tree sender)
	{
		if (mPlayer.CheckSkillInput())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;	
	}
	public BehaveResult TickCheckSkillExistsAction (Tree sender)
	{
		if (mPlayer.CheckSkillExists())
			return BehaveResult.Success;
		
		return BehaveResult.Failure;
	}
	/*
	 * Actions
	 */
	public BehaveResult TickActiveSkillAction (Tree sender)
	{
		mPlayer.ActiveSkill();
		return BehaveResult.Success;
	}
}
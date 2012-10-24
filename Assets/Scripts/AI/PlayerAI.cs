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
		tree = BLBehaveLib.InstantiateTree(BLBehaveLib.TreeType.Player_Playertree, this);
		
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
	
	public BehaveResult TickCheckAttackDecorator (Tree sender)
	{
		return BehaveResult.Success;
	}
	
	public BehaveResult TickCheckSkillInputAction (Tree sender)
	{
		if (mPlayer.SkillCheckInput())
			return BehaveResult.Success;
			return BehaveResult.Failure;	
	}
	
	public BehaveResult TickCheckSkillActiveAction (Tree sender)
	{
		if (mPlayer.CheckSkillActive())
			return BehaveResult.Success;
			return BehaveResult.Failure;
	}
	
	public BehaveResult TickActiveSkillAction (Tree sender)
	{
			Debug.Log("Check if a Skill is present");
			return BehaveResult.Failure;
	}
	
	public BehaveResult TickCheckAttackInputAction (Tree sender)
	{
		if (mPlayer.AttackCheckInput())
			return BehaveResult.Success;
			return BehaveResult.Failure;
	}
	
	public BehaveResult TickCheckAttackUsableAction (Tree sender)
	{
		Debug.Log("Check if Attack is Usable");
		return BehaveResult.Success;
	}
	
	public BehaveResult TickAnimateAttackAction (Tree sender)
	{
		Debug.Log("Check if Attack Animation is carried out");
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickAttackAction (Tree sender)
	{
		Debug.Log("Check if Attack Carried out");
		return BehaveResult.Failure;
	}
	
	/*
	 * Jump
	 */
	public BehaveResult TickCheckJumpInputAction (Tree sender)
	{
		if (mPlayer.CheckJumpInput())
		{
			return BehaveResult.Success;	
		}
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickCheckAirborneAction (Tree sender)
	{
		if (mPlayer.CheckAirborne())
		{
			return BehaveResult.Success;
		}
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickJumpCheckWaitTimeAction (Tree sender)
	{
		if (mPlayer.CheckJumpWaitTime())
			return BehaveResult.Success;
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickCheckMaxJumpAction (Tree sender)
	{
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickJumpAction (Tree sender)
	{
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickAnimateJumpAction (Tree sender)
	{
		return BehaveResult.Failure;
	}
}

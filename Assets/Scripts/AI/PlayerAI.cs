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
		tree = BLBehaveLib.InstantiateTree(BLBehaveLib.TreeType.Player_PlayerAI, this);
		
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
	
	public BehaveResult TickJumpAction (Tree sender)
	{
		Debug.Log("Jump");
		mPlayer.JumpAction();
		return BehaveResult.Success;
	}
	
	public BehaveResult TickRunAction (Tree sender)
	{
		Debug.Log("Run");
		return BehaveResult.Success;
	}
	
	public BehaveResult TickIdleAction (Tree sender)
	{
		Debug.Log("Idle");
		return BehaveResult.Success;
	}
	
	public BehaveResult TickJumpCheckInputAction (Tree sender)
	{
		if (mPlayer.JumpCheckInput())
			return BehaveResult.Success;
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickJumpCheckAirborneAction (Tree sender)
	{
		if (mPlayer.JumpCheckAirborne())
			return BehaveResult.Success;
		return BehaveResult.Failure;
	}
	
	
	
	public BehaveResult TickRunCheckAction (Tree sender)
	{
		
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickIdleCheckAction (Tree sender)
	{
		
		return BehaveResult.Failure;
	}
	
}

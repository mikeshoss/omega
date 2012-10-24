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
	
	//RayROD and ShossBOSS's work here.
	public BehaveResult TickRunCheckInputAction(Tree sender)
	{
		Debug.Log(mPlayer.RunCheckInput());
		if(mPlayer.RunCheckInput())
			return BehaveResult.Success;	
		
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickRunAction(Tree sender)
	{
//		if(mPlayer.JumpCheckInput()){
//			return BehaveResult.Success;
//		}
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickAnimateRunAction(Tree sender)
	{
		mPlayer.AnimateRun();
		return BehaveResult.Success;
	}
	
	public BehaveResult TickAnimateIdleAction(Tree sender)
	{
		mPlayer.AnimateIdle();
		return BehaveResult.Success;
	}
	
	public BehaveResult TickIdleAction(Tree sender)
	{
		return BehaveResult.Failure;
	}
		
}

using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private PlayerAI mAI;
	private PlayerData mPlayer;
	
	private Vector3 mMoveVelocity;
	private int mCurrentJumpNumber;
	
	private bool mJumpWait;
	private bool mJumpMax;
	private bool mAirborne;
	
	/*
	 * Checks player input
	 */
	private bool mJumpPressed;
	private bool mRunPressed;
	private bool mSkillPressed;
	private bool mAttackPressed;
	
	private bool mCurrentSkill;
	private bool mSkillActive;
	
	// Use this for initialization
	void Start ()
	{
		mMoveVelocity = new Vector3(0,0,0);
		mAirborne = false;
		mJumpWait = true;
		mAI = (PlayerAI)gameObject.AddComponent("PlayerAI");
	}
	
	// Update is called once per frame
	void Update ()
	{
		mRunPressed = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
	
		mJumpPressed = Input.GetKey(KeyCode.W);
		mSkillPressed = Input.GetKey(KeyCode.Alpha1) || 
						Input.GetKey(KeyCode.Alpha2) || 
						Input.GetKey(KeyCode.Alpha3) || 
						Input.GetKey(KeyCode.Alpha4);
		mAttackPressed = Input.GetKey(KeyCode.Space);
	}
	
	void FixedUpdate ()
	{
		ApplyGravity();
		Move ();
	}
	
	
	/*
	 * Attack Branch
	 */
	public bool CheckSkillInput ()
	{
		return mSkillPressed;
	}
	
	public bool CheckAttackInput ()
	{
		return mAttackPressed;
	}
	
	public void ActiveSkill () 
	{
	}
	
	public bool CheckSkillActive ()
	{
		/*
		 * Needs to check if the selected skill is currently in an active state
		 */
		mSkillActive = true;
		return mSkillActive;
	}
	
	public void Attack()
	{
	}
	
	/*
	 * Jump Branch
	 */
	public bool CheckJumpInput ()	
	{
		return mJumpPressed;
	}
	
	public bool CheckJumpWaitTime ()
	{
		return mJumpWait;	
	}
	
	private void SetJumpWait(bool val)
	{
		mJumpWait = val;
	}
	
	IEnumerator CoroutineJumpWaitTime ()
	{
		SetJumpWait(false);
		yield return new WaitForSeconds(mPlayer.JumpWaitTime);
		SetJumpWait(true);
	}
	
	public void JumpAction ()
	{
		mAirborne = true;
		StartCoroutine(CoroutineJumpWaitTime());
		mMoveVelocity.y = mPlayer.JumpVelocity;
	}
	
	/*
	 * Run Branch
	 */
	public bool CheckRunInput()
	{
		return mRunPressed;
	}
	
	public bool AnimateRun()
	{
		exSprite ex = GetComponent<exSprite>();
		if(!ex.spanim.IsPlaying("ninja_run"))
		ex.spanim.Play("ninja_run");
		return true;
	}
	
	public void RunAction ()
	{
		// apply running action
	}
	
	/*
	 * Idle Branch
	 */
	public bool AnimateIdle()
	{
		exSprite ex = GetComponent<exSprite>();
		if(!ex.spanim.IsPlaying("ninja_land"))
		ex.spanim.Play("ninja_land");
		return true;
	}
	
	public void IdleAction ()
	{
		// apply idle action	
	}
	
	
	public bool CheckAirborne ()
	{
		if (mAirborne)
		{
			return false;
		}
		return true;
	}
	
	public void ApplyGravity ()
	{
		if (mAirborne)
		{
			mMoveVelocity.y -= 9.8f * Time.deltaTime;
		}
		
		if (mMoveVelocity.y < 0)
		{
			mAirborne = false;
			mMoveVelocity.y = 0;
		}
	}
	
	public void Move()
	{
		gameObject.transform.Translate(mMoveVelocity);
	}
	
}

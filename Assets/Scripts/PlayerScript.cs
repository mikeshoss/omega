using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	
	private ImplBehaveExample mAI; 	// Reference to Players AI Behavior Tree
	
	private float mJumpWaitLength; 	// Delay between multiple air jumps
	private float mFriction;		// Intensity of friction
	private float mGravity;			// Intensity of gravity;
	private Vector3 mMoveVelocity; 	// Movement Velocity & Direction
	
	private int mJumpCurrentNumber; // Current amount of times player jumped
	private int mJumpMax;			// Maximum number of jumps before being grounded
	private int mRunDirection;		// Direction of travel for player
	
	private bool mIdleCheck;		// Condition: Idle Check
	private bool mFrictionEnabled;	// Enables/Disables Friction
	private bool mGravityEnabled;	// Enables/Disables Gravity
	private bool mJumpInput;		// Condition: Jump Input
	private bool mJumpWait;			// Condition: Jump Delay, cannot jump again till false
	private bool mRunInput;			// Condition: Run Input

	
	/**
	 * MonoBehaviour Functions
	 */
	
	void Start ()
	{
		mMoveVelocity = new Vector3(0,0,0);
		mJumpMax = 2;
		mJumpCurrentNumber = 0;
		mJumpWaitLength = 0.3f;
		mFriction = 10f;
		mGravity = 1000f;
		mAI = (ImplBehaveExample)gameObject.AddComponent("ImplBehaveExample");
	}
	
	void Update ()
	{
		mRunInput = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
		if (Input.GetKey(KeyCode.A))
		{
			mRunDirection = -1;	
		}
		
		if (Input.GetKey(KeyCode.D))
		{
			mRunDirection = 1;	
		}
		mIdleCheck = !mRunInput && !mJumpInput;
	}
	
	void FixedUpdate ()
	{
		IsGrounded();
		ApplyGravity();
		ApplyFriction();
		Move();
	}
	
	
	/**
	 * Behave: AI: PlayerJump
	 */
	
	public bool ConditionJumpMax ()
	{
		return mJumpCurrentNumber < mJumpMax;	
	}
	
	public bool ConditionJumpWait ()
	{
		if (mJumpCurrentNumber == 0) {
			mJumpWait = false;
			return true;
		}
		
		return mJumpWait;
	}
	
	IEnumerator CoroutineJumpWait ()
	{
		//Debug.Log ("******** coroutine START: " + mJumpWait);
		yield return new WaitForSeconds(mJumpWaitLength);
		mJumpWait = true;
		//Debug.Log ("******** coroutine END: " + mJumpWait);
	}
	
	public bool ConditionJumpInput ()
	{
		return mJumpInput = Input.GetKey(KeyCode.Space);	
	}
	
	public void AnimateJump ()
	{
		exSprite ex = (exSprite)GetComponent<exSprite>();
		ex.spanim.Play("ninja_jump", 0);
	}
	
	public void ActionJump ()
	{
		SetGravityEnabled(true);
		mMoveVelocity.y += 700;
		mJumpCurrentNumber++;
		StartCoroutine(CoroutineJumpWait());
	}
	
	
	/*
	 * Behave: AI: PlayerRun
	 */
	
	public bool ConditionRunInput ()
	{
		return mRunInput && IsGrounded();	
	}
	
	public void AnimateRun ()
	{
		exSprite ex = (exSprite)GetComponent<exSprite>();
		if (!ex.spanim.IsPlaying("ninja_run"))
			ex.spanim.Play("ninja_run");
	}
	
	public void ActionRun ()
	{
		SetFrictionEnabled(true);
		mMoveVelocity.x += 100;
	}
	
	
	/*
	 * Behave: AI: PlayerIdle
	 */
	
	public bool ConditionIdleCheck ()
	{
		return mIdleCheck && IsGrounded();	
	}
	
	public void AnimateIdle ()
	{
		exSprite ex = (exSprite)GetComponent<exSprite>();
		if (!ex.spanim.IsPlaying("ninja_land"))
			ex.spanim.Play("ninja_land");
	}
	
	public void ActionIdle ()
	{
		
	}
	
	/*
	 * Other: Flags
	 */
	bool IsGrounded ()
	{
		bool grounded = mMoveVelocity.y <= 0;
		
		if (grounded)
		{
			SetGravityEnabled(false);
			mMoveVelocity.y = 0;
			mJumpCurrentNumber = 0;
			return true;
		}
		return false;	
	}
	
	void ApplyGravity ()
	{
		if (mGravityEnabled)
			mMoveVelocity.y -= mGravity * Time.deltaTime;	
		
		//Debug.Log ("Velocity Y value: " + mMoveVelocity.y);
	}
	
	void SetGravityEnabled (bool enable)
	{
		mGravityEnabled = enable;
	}
	
	void ApplyFriction ()
	{
		if (mFrictionEnabled) {
			mMoveVelocity.x -= mFriction;
			if (mMoveVelocity.x < 0)
			{
				mMoveVelocity.x = 0;	
			}
		}
		
	}
	
	void SetFrictionEnabled (bool enable)
	{
		mFrictionEnabled = enable;	
	}
	
	void Move ()
	{
		CharacterController cc = (CharacterController)GetComponent<CharacterController>();
		
		cc.Move(mMoveVelocity * Time.deltaTime);
	}
}

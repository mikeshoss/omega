using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerScript : MonoBehaviour {

	private PlayerAI mAI;
	private exSprite mSprite;
	private CharacterController mCharacter;
	private PlayerData mPlayer;
	
	private Vector3 mMoveVelocity;
	
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
	private int  mCurrentJump;
	private int  mCurrentRunDir;
	
	private const float kGravity = 1700.0f;
	private const float kFriction = 4000.0f;
	
	// Use this for initialization
	void Start ()
	{
		mPlayer = new PlayerData(1, null, null);
		mCharacter = GetComponent<CharacterController>();
		mMoveVelocity = new Vector3(0,0,0);
		mAirborne = true;
		mJumpWait = true;
		mCurrentJump = 0;
		mCurrentRunDir = 1;
		mSprite = GetComponent<exSprite>();
		mAI = (PlayerAI)gameObject.AddComponent("PlayerAI");
	}
	
	// Update is called once per frame
	void Update ()
	{
		mRunPressed = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
		
		if (Input.GetKey(KeyCode.A))
			mCurrentRunDir = -1;
		
		if(Input.GetKey(KeyCode.D))
			mCurrentRunDir = 1;	
		
		mJumpPressed = Input.GetKey(KeyCode.W);
		
		mSkillPressed = Input.GetKey(KeyCode.Alpha1) || 
						Input.GetKey(KeyCode.Alpha2) || 
						Input.GetKey(KeyCode.Alpha3) || 
						Input.GetKey(KeyCode.Alpha4);
		
		mAttackPressed = Input.GetKey(KeyCode.Space);
	}
	
	void FixedUpdate ()
	{
		CheckGrounded();
		ApplyGravity();
		ApplyFriction();
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
	
	public bool CheckAttackCooldown ()
	{
		return true;	
	}
	
	public void ActiveSkill () 
	{
	}
	
	public bool CheckSkillExists ()
	{
		return true;
	}
	
	public void AnimateAttack ()
	{
		
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
		StopCoroutine("CoroutineJumpWaitTime");
	}
	
	public bool CheckJumpMax ()
	{
		return mCurrentJump < mPlayer.MaxJump;
	}
	
	public void AnimateJump ()
	{
		mSprite.spanim.Play ("ninja_jump", 0);
	}
	
	public void JumpAction ()
	{
		mAirborne = true;
		StartCoroutine(CoroutineJumpWaitTime());
		mMoveVelocity.y = mPlayer.JumpVelocity;
		mCurrentJump++;
	}
	
	/*
	 * Run Branch
	 */
	public bool CheckRunInput ()
	{
		return mRunPressed;
	}
	
	public void AnimateRun ()
	{
		if(!mSprite.spanim.IsPlaying("ninja_run"))
			mSprite.spanim.Play("ninja_run");
	}
	
	public void RunAction ()
	{
		if (mCurrentRunDir > 0)
			mMoveVelocity.x += mPlayer.RunAcceleration * Time.deltaTime;
			
		else if (mCurrentRunDir < 0)
			mMoveVelocity.x -= mPlayer.RunAcceleration * Time.deltaTime;
		
		if ((mMoveVelocity.x > mPlayer.MaxRunSpeed && mCurrentRunDir == 1) || (mMoveVelocity.x < -mPlayer.MaxRunSpeed && mCurrentRunDir == -1))
		{
			mMoveVelocity.x = mPlayer.MaxRunSpeed * mCurrentRunDir;	
		}
		
		Vector3 scale = gameObject.transform.localScale;
		scale.x = Mathf.Abs(scale.x);
		
		if (mMoveVelocity.x > 0)
		{
			mCurrentRunDir = 1;
			scale.x *= 1;
		}
		else if (mMoveVelocity.x < 0)
		{
			mCurrentRunDir = -1;
			scale.x *= -1;
		}
		gameObject.transform.localScale = scale;
	}
	
	/*
	 * Idle Branch
	 */
	public void AnimateIdle ()
	{
		if(!mSprite.spanim.IsPlaying("ninja_land"))
			mSprite.spanim.Play("ninja_land");
	}
	
	public void IdleAction ()
	{
		// apply idle action	
	}
	
	public bool IsAirborne ()
	{
		return mAirborne;
	}
	
	private void ApplyGravity ()
	{
		if (mAirborne)
		{
			mMoveVelocity.y -= kGravity * Time.deltaTime;
		}
	}
	
	private void ApplyFriction ()
	{
		if (!mAirborne && !mRunPressed)
		{
			if (mCurrentRunDir > 0)
			{
				mMoveVelocity.x -= kFriction * Time.deltaTime;
				if (mMoveVelocity.x < 0)
					mMoveVelocity.x = 0;	
			}
			
			if (mCurrentRunDir < 0)
			{
				mMoveVelocity.x += kFriction * Time.deltaTime;
				if (mMoveVelocity.x > 0)
					mMoveVelocity.x = 0;
			}
		}
	}
	
	public void Move ()
	{
		mCharacter.Move(mMoveVelocity * Time.deltaTime);
	}
	
	private void CheckGrounded ()
	{
		RaycastHit hit;
		Vector3 down = transform.TransformDirection(-Vector3.up);
		float height = mCharacter.height;

		Vector3 pos = transform.position;

		pos.y -= (height / 2.0f) * transform.localScale.y - 5;
		pos.x -= mCharacter.radius;

		RaycastHit hit2;

		Vector3 pos2 = transform.position;

		pos2.y -= (height / 2.0f) * transform.localScale.y - 5;
		pos2.x += mCharacter.radius;

        mAirborne = !Physics.Raycast(pos2, down, out hit2, mMoveVelocity.y * Time.deltaTime + 10);
		
		Vector3 debugVector = mMoveVelocity;
		debugVector.y = debugVector.y * Time.deltaTime + 10;
		
		Debug.DrawRay(pos, debugVector * Time.deltaTime, Color.red);
		Debug.DrawRay(pos2, debugVector * Time.deltaTime, Color.red);
	}
	
	public void OnControllerColliderHit (ControllerColliderHit hit)
	{
		int terrainLayer = LayerMask.NameToLayer("Terrain");
		if (hit.gameObject.layer == terrainLayer && mCharacter.collisionFlags == CollisionFlags.Below)
		{
			mMoveVelocity.y = 0;
			mAirborne = false;
			mCurrentJump = 0;
		}

		// If player hits a wall, set x velocity to 0.
		if (hit.gameObject.layer == terrainLayer && mCharacter.collisionFlags == CollisionFlags.Sides)
		{
			mMoveVelocity.x = 0;
		}
		// If head hits the top of a terrain, bounce back down
		if (hit.gameObject.layer == terrainLayer && mCharacter.collisionFlags == CollisionFlags.CollidedAbove)
		{
			mMoveVelocity.y = 0;	
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : CombatantScript {

	private PlayerData mPlayer;

	private int  mRequestedSkill;
	private int  mJumpCount;
	
	private bool mCanJump;
	private bool mJumpPressed;
	private bool mRunPressed;
	private bool mSkillPressed;
	private bool mAttackPressed;
	
	// Use this for initialization
	void Start ()
	{
		mCharacter = 		GetComponent<CharacterController>();
		mSprite = 			GetComponent<exSprite>();
		mLearnedSkills = 	new List<Skill>();
		mSelectedSkills = 	new List<Skill>();
		mPlayer = 			new PlayerData(1, mLearnedSkills, mSelectedSkills);
		mHealth = 			mPlayer.MaxHealth;
		mEnergy	=			mPlayer.MaxEnergy;
		mCombatantType = 	CombatantType.PLAYER;
		mMoveVelocity = 	new Vector3(0,0,0);
		mCurrentSkill = 	0;
		mRequestedSkill = 	0;
		mJumpCount = 		0;
		mDirection = 		1;
		mIsAirborne = 		true;
		mCanJump = 			true;
		Skill fireball = 	new Skill(0, "Fireball", "Description", 40.0f, this, 0.3f, 0.5f, 0.3f, 3.0f, 0.7f, "Fireball");
		
		mLearnedSkills.Add(fireball);
		mSelectedSkills.Add(fireball);

		gameObject.AddComponent("PlayerAI");
	}
	
	void Update ()
	{
		mJumpPressed = Input.GetKey(KeyCode.W);
		mAttackPressed = Input.GetKey(KeyCode.Space);

		if(mRunPressed = Input.GetKey(KeyCode.D))
			mDirection = 1;
		else if (mRunPressed = Input.GetKey(KeyCode.A))
			mDirection = -1;
		
		if (mSkillPressed = Input.GetKey(KeyCode.Alpha1))
			mRequestedSkill = 0;
		else if (mSkillPressed = Input.GetKey(KeyCode.Alpha2))
			mRequestedSkill = 1;
		else if (mSkillPressed = Input.GetKey(KeyCode.Alpha3))
			mRequestedSkill = 2;
		else if (mSkillPressed = Input.GetKey(KeyCode.Alpha4))
			mRequestedSkill = 3;

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
		return !mIsSkillCooling[mCurrentSkill];	
	}
	
	public void ActiveSkill () 
	{
		mCurrentSkill = mRequestedSkill;
		// do whatever to the action bar
		// ActionBar.HighlighSkill(mCurrentSkill);
	}
	
	public bool CheckSkillExists ()
	{
		return mRequestedSkill < mSelectedSkills.Count;
	}
	
	public void AnimateAttack ()
	{
		StartCoroutine("CoroutineAttack");
	}
	
	public void AttackAction()
	{

	}
	
	IEnumerator CoroutineAttack ()
	{
		mIsSkillCooling[mCurrentSkill] = true;
		float delay = mSelectedSkills[mCurrentSkill].OriginDelay;
		
		GameObject go = mSelectedSkills[mCurrentSkill].InstantiateObject();
		SkillScript sc = (SkillScript)go.GetComponent<SkillScript>();
		sc.Skill = mSelectedSkills[mCurrentSkill];
		sc.Origin = this;
		
		MeshRenderer mr = (MeshRenderer)go.GetComponent<MeshRenderer>();
		mr.enabled = false;
		
		yield return new WaitForSeconds(delay);
		mIsSkillCooling[mCurrentSkill] = false;
	}
	/*
	 * Jump Branch
	 */
	public bool CheckJumpInput ()	
	{
		return mJumpPressed;
	}
	
	public bool CanJump ()
	{
		return mCanJump;	
	}
	
	IEnumerator CoroutineJumpWaitTime ()
	{
		mCanJump = false;
		yield return new WaitForSeconds(mPlayer.JumpWaitTime);
		mCanJump = true;
	}
	
	public bool CheckJumpMax ()
	{
		return mJumpCount < mPlayer.MaxJump;
	}
	
	public void AnimateJump ()
	{
		mSprite.spanim.Play ("ninja_jump", 0);
	}
	
	public void JumpAction ()
	{
		mIsAirborne = true;
		StartCoroutine(CoroutineJumpWaitTime());
		mMoveVelocity.y = mPlayer.JumpVelocity;
		mJumpCount++;
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
		if (mDirection > 0)
			mMoveVelocity.x += mPlayer.RunAcceleration * Time.deltaTime;
			
		else if (mDirection < 0)
			mMoveVelocity.x -= mPlayer.RunAcceleration * Time.deltaTime;
		
		if ((mMoveVelocity.x > mPlayer.MaxRunSpeed && mDirection == 1) || (mMoveVelocity.x < -mPlayer.MaxRunSpeed && mDirection == -1))
		{
			mMoveVelocity.x = mPlayer.MaxRunSpeed * mDirection;	
		}
		
		Vector3 scale = gameObject.transform.localScale;
		scale.x = Mathf.Abs(scale.x);
		
		if (mMoveVelocity.x > 0)
		{
			mDirection = 1;
			scale.x *= 1;
		}
		else if (mMoveVelocity.x < 0)
		{
			mDirection = -1;
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
		return mIsAirborne;
	}
	
	private void ApplyGravity ()
	{
		if (mIsAirborne)
		{
			mMoveVelocity.y -= kGravity * Time.deltaTime;
		}
	}
	
	private void ApplyFriction ()
	{
		if (!mIsAirborne && !mRunPressed)
		{
			if (mDirection > 0)
			{
				mMoveVelocity.x -= kFriction * Time.deltaTime;
				if (mMoveVelocity.x < 0)
					mMoveVelocity.x = 0;	
			}
			
			if (mDirection < 0)
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
		pos.x -= mCharacter.radius / 5.0f;

		RaycastHit hit2;

		Vector3 pos2 = transform.position;

		pos2.y -= (height / 2.0f) * transform.localScale.y - 5;
		pos2.x += mCharacter.radius/ 5.0f;

        mIsAirborne = (!Physics.Raycast(pos, down, out hit, mMoveVelocity.y * Time.deltaTime + 10) && 
					!Physics.Raycast(pos2, down, out hit2, mMoveVelocity.y * Time.deltaTime + 10));
		
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
			mIsAirborne = false;
			mJumpCount = 0;
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
	
	public void OnTriggerEnter (Collider other)
	{
		// determine if it was a skill
		if (other.tag == "Skill")
		{
			//determine if it was an enemies skill
			SkillScript ss = (SkillScript)other.gameObject.GetComponent<SkillScript>();
			
			if (ss != null)
			{
				if (ss.Origin.GetCombatantType != mCombatantType)	
				{
					ss.Target = this;
					ss.ShouldEnd();
				}
			} else
			{
				Debug.Log ("Something went wrong");	
			}
		}
	}
}

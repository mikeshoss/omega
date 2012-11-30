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
	
	private bool mJumpChecked;
	private bool mAttackChecked;
	private bool mRunChecked;
	private bool mSkillChecked;
	
	public AudioClip mJumpSound;
	// Use this for initialization
	
	public PlayerData Player
	{
		get
		{
			return mPlayer;	
		}
	}
	
	public float MaxHealth
	{
		get
		{				
			return mPlayer.MaxHealth;	
		}
	}
	
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
		mJumpChecked = 		true;
		mAttackChecked =	true;
		mRunChecked = 		true;
		mSkillChecked = 	true;
		Skill fireball = 	new Skill(0, "Fireball", "Description", 40.0f, this, 1.0f, 0.5f, 0.3f, 3.0f, 0.7f, "Fireball");
		Skill icepick = 	new Skill(1, "Icepick", "D", 30.0f, this, 1.0f, 0.5f, 0.3f, 3.0f, 0.7f, "Icepick");
		mLearnedSkills.Add(fireball);
		mLearnedSkills.Add (icepick);
		mSelectedSkills.Add(fireball);
		mSelectedSkills.Add (icepick);
		
		GUITexture guit = (GUITexture)GameObject.FindGameObjectWithTag("SkillSetIcon1").GetComponent<GUITexture>();
		guit.texture = (Texture)Resources.Load("skill-fireball");
		
		guit = (GUITexture)GameObject.FindGameObjectWithTag("SkillSetIcon2").GetComponent<GUITexture>();
		guit.texture = (Texture)Resources.Load("skill-icepick");
		gameObject.AddComponent("PlayerAI");
		
		
	}
	
	void Update ()
	{
		if (Time.timeScale != 0)
		{
			if (Input.GetKeyDown(KeyCode.W) && mJumpChecked)
			{
				mJumpPressed = true;
				mJumpChecked = false;
			} else if (mJumpChecked)
			{
				mJumpPressed = Input.GetKeyDown(KeyCode.W);	
			}
			
			
			if (Input.GetKeyDown(KeyCode.Space) && mAttackChecked)
			{
				mAttackPressed = true;
				mAttackChecked = false;
			} else if (mAttackChecked)
			{
				mAttackPressed = Input.GetKeyDown(KeyCode.Space);
			}
			
			if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && mRunChecked)
			{
				if (Input.GetKey(KeyCode.D))
					mDirection = 1;
				else if (Input.GetKey(KeyCode.A))
					mDirection = -1;
				
				mRunPressed = true;
				mRunChecked = false;
			} else if (mRunChecked)
			{
				if(mRunPressed = Input.GetKey(KeyCode.D))
				{
					mDirection = 1;
				}
				else if (mRunPressed = Input.GetKey(KeyCode.A))
				{
					mDirection = -1;
				}
			}
			
			if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha1)) && mSkillChecked)
			{
				if (Input.GetKey(KeyCode.Alpha1))
					mRequestedSkill = 0;
				else if (Input.GetKey(KeyCode.Alpha2))
					mRequestedSkill = 1;
				else if (Input.GetKey(KeyCode.Alpha3))
					mRequestedSkill = 2;
				else if (Input.GetKey(KeyCode.Alpha4))
					mRequestedSkill = 3;
				
				mSkillPressed = true;
				mSkillChecked = false;
			} else if (mSkillChecked)
			{
				if (mSkillPressed = Input.GetKey(KeyCode.Alpha1))
					mRequestedSkill = 0;
				else if (mSkillPressed = Input.GetKey(KeyCode.Alpha2))
					mRequestedSkill = 1;
				else if (mSkillPressed = Input.GetKey(KeyCode.Alpha3))
					mRequestedSkill = 2;
				else if (mSkillPressed = Input.GetKey(KeyCode.Alpha4))
					mRequestedSkill = 3;
			}
		}
		if (Input.GetKeyDown(KeyCode.Return) && Time.timeScale != 0) {
			Time.timeScale = 0;
		} else if (Input.GetKeyDown(KeyCode.Return) && Time.timeScale == 0) {
			Time.timeScale = 1;
		}
		
		
	}
	
	void FixedUpdate ()
	{
		HealthRegen();
		EnergyRegen();
		CheckGrounded();
		ApplyGravity();
		ApplyFriction();
		Move ();
		UpdateVelocity ();
	}
	
	void UpdateVelocity ()
	{
		mPrevVelocity = mMoveVelocity;	
	}
	
	void HealthRegen ()
	{
		float recovery = 2 * Time.deltaTime;
		mHealth += recovery;
		if (mHealth > MaxHealth)
		{
			mHealth = MaxHealth;	
		}
	}
	
	void EnergyRegen ()
	{
		float recovery = 4 * Time.deltaTime;
		mEnergy += recovery;
		if (mEnergy > mPlayer.MaxEnergy)
		{
			mEnergy = mPlayer.MaxEnergy;	
		}
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
	}
	
	public bool CheckSkillExists ()
	{
		return mRequestedSkill < mSelectedSkills.Count;
	}
	
	public void AnimateAttack ()
	{
		if (mEnergy < 20)
			return;
		mEnergy -= 10;
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
	
	public void SetJumpChecked(bool val)
	{
		mJumpChecked = val;	
	}
	
	public void SetAttackChecked(bool val)
	{
		mAttackChecked = val;	
	}
	public void SetRunChecked(bool val)
	{
		mRunChecked = val;	
	}
	public void SetSkillChecked(bool val)
	{
		mSkillChecked = val;	
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
		AudioSource.PlayClipAtPoint(mJumpSound, transform.position, 0.5f);
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
		float accel = 0;
		
		if (mIsAirborne)
		{
			accel = mPlayer.AirRunAcceleration;	
		} else
		{
			accel = mPlayer.RunAcceleration;	
		}
		
		if (mDirection == 1) {
			mMoveVelocity.x += accel * Time.deltaTime;
		}
		else if (mDirection == -1) {
			mMoveVelocity.x -= accel * Time.deltaTime;
		}
		if ((mMoveVelocity.x > mPlayer.MaxRunSpeed && mDirection == 1) || 
			(mMoveVelocity.x < -mPlayer.MaxRunSpeed && mDirection == -1))
		{
			mMoveVelocity.x = mPlayer.MaxRunSpeed * mDirection;	
		}
		
		Vector3 scale = gameObject.transform.localScale;
		scale.x = Mathf.Abs(scale.x);
		
		if (mMoveVelocity.x > 0)
		{
			scale.x *= 1;
		}
		else if (mMoveVelocity.x < 0)
		{
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
		
		if (mHealth <= 0) {
			mHealth = MaxHealth;
			transform.position = new Vector3(0,0,0);
		}
	}
	
	private void CheckGrounded ()
	{
		RaycastHit hit;
		Vector3 down = transform.TransformDirection(-Vector3.up);
		float height = mCharacter.height;

		Vector3 pos = transform.position;

		pos.y -= (height / 2.0f) * transform.localScale.y - 25;

		pos.x -= mCharacter.radius / 2.0f;

		RaycastHit hit2;

		Vector3 pos2 = transform.position;

		pos2.y -= (height / 2.0f) * transform.localScale.y - 25;

		pos2.x += mCharacter.radius/ 2.0f;


        mIsAirborne = (!Physics.Raycast(pos, down, out hit, mMoveVelocity.y * Time.deltaTime + 30) && 
					!Physics.Raycast(pos2, down, out hit2, mMoveVelocity.y * Time.deltaTime + 30));
		
		Vector3 debugVector = mMoveVelocity;
		debugVector.y = debugVector.y * Time.deltaTime + 30;
		
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
			}
		} else if (other.tag == "Pickup")
		{
			PickupScript ps = (PickupScript)other.gameObject.GetComponent<PickupScript>();
			
			if (ps != null)
			{
				Skill s = ps.GetSkill();
				int foundIndex = -1;
				
				for(int i = 0; i < mLearnedSkills.Count; i++)
				{
					if (s.Name == mLearnedSkills[i].Name)
					{
						foundIndex = i;
						break;
					}
				}
				
				if (foundIndex >= 0)
				{
					mLearnedSkills[foundIndex].LevelUp();
					Debug.Log ("leveled up " + mSelectedSkills[1].Level);
				} else {
					mLearnedSkills.Add(s);
					
					for (int i = 0; i < 4; i++)
					{
						if (mSelectedSkills[i] == null)
						{
							mSelectedSkills[i] = s;
							break;
						}
					}
				}
			}
			
			Destroy (other.gameObject);
		} else if (other.tag == "Respawn")
		{
			transform.position = new Vector3(0,0,0);
		}
	}
}

	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(CharacterController))]
public class EnemyScript : CombatantScript {
	
	private EnemyData mEnemy;
	public List<GameObject> mPath;
	
	public GameObject mHealthBar;
	public GameObject mHealthBarParent;
	
	private float mXViewRange;
	private float mYViewRange; 
	
	private int mNextNode;
	
	private bool mIsDead; 
	private bool mIsHostile;
	private bool mIsWandering;
	private bool mIsIdle;
	
	private const float kMinNodeDistance = 	200.0f;
	private const float kNodeWaitTime = 	0.2f;
	
	void Start () {
		mCharacter = 		(CharacterController)GetComponent<CharacterController>();
		mSprite = 			(exSprite)GetComponent<exSprite>();
		mSelectedSkills = 	new List<Skill>();
		mEnemy = 			new EnemyData(1, mSelectedSkills);
		mCombatantType = 	CombatantType.ENEMY;
		mMoveVelocity = 	new Vector3(0,0,0);
		mHealth = 			mEnemy.MaxHealth;
		mEnergy = 			mEnemy.MaxEnergy;
		mXViewRange = 		700.0f;
		mYViewRange = 		300.0f;
		mCurrentSkill = 	0;
		mIsDead = 			false;
		mIsHostile = 		false;
		mIsWandering = 		false;
		mIsIdle = 			true;
		mIsAirborne = 		true;
		
		if (mPath.Count > 1) {
			mNextNode = 1;
		} else {
			mNextNode = 0;
		}
		
		Skill icepick = new Skill(1, "Icepick", "Description", 20.0f, this, 1.5f, 0.5f, 0.3f, 3.0f, 1.0f, "Icepick");
		mSelectedSkills.Add(icepick);
		gameObject.AddComponent("EnemyAI");
	}
	
	void Update () {
	
	}
	
	void OnGUI ()
	{
		
	}
	
	void FixedUpdate () {
		if (!mIsDead)
		{
			UpdateGUI();
			CheckGrounded ();
			ApplyGravity ();
			CheckHostile ();
			Move ();
		}
	}
	
	void UpdateGUI ()
	{
		if (mHealthBar != null && mHealthBarParent != null)
		{
			Vector3 p = transform.position;
			p.y += mCharacter.height + mCharacter.radius * 4;
			
			mHealthBarParent.transform.position = p;
			
			Vector3 v = mHealthBar.transform.localScale;
			v.x = 1 * (mHealth / mEnemy.MaxHealth);
			if (v.x < 0)
			{
				v.x = 0;	
			}
			mHealthBar.transform.localScale = v;
		}
	}
	
	/*
	 * DEAD
	 */
	public bool IsDead ()
	{
		return mIsDead;
	}
	public void DeadAction ()
	{
		//do nothing
		Destroy(mHealthBarParent);
		Destroy(gameObject);
	}
	
	/*
	 * DIE
	 */
	public bool IsDying ()
	{
		return mHealth <= 0;	
	}
	public void AnimateDie ()
	{
		// set die animation
		// start coroutine die
		mSprite.spanim.Play("dance_back");
		GameObject go = (GameObject)Instantiate(Resources.Load("Pickup"));
		go.transform.position = transform.position;
		
		PickupScript ps = (PickupScript)go.GetComponent<PickupScript>();
		
		ps.SetSkill(mSelectedSkills[0]);
		
		StartCoroutine("CoroutineDieTime");
	}
	IEnumerator CoroutineDieTime ()
	{
		
		AudioSource.PlayClipAtPoint((AudioClip)Resources.Load ("enemy-die"), new Vector3(0,0,0), 3);
		yield return new WaitForSeconds(1.0f); // Animation die time
		mIsDead = true;
		StopCoroutine("CoroutineDieTime");
	}
	public void DieAction ()
	{
		// some sort of dying action
	}

	/*
	 * ALIVE
	 */
	public bool IsAlive ()
	{
		return mHealth > 0;	
	}
	
	/*
	 * HOSTILE
	 */
	void CheckHostile ()
	{
		
		GameObject player = GameObject.FindGameObjectWithTag("Character");
		mIsHostile = 	((player.transform.position.x - mXViewRange <= transform.position.x && 
						transform.position.x <= player.transform.position.x + mXViewRange) &&
						(player.transform.position.y - mYViewRange <= transform.position.y &&
						transform.position.y <= player.transform.position.y + mYViewRange));
		
		if (mIsHostile)
		{
			float dirCheck = player.transform.position.x - transform.position.x;
			Vector3 scale = gameObject.transform.localScale;
			scale.x = Mathf.Abs(scale.x);
			
			if ( dirCheck >= 0) {
				mDirection = 1;
				scale.x *= 1;
			}
			else {
				mDirection = -1;
				scale.x *= -1;
			}
			
			gameObject.transform.localScale = scale;
			
		}
		
	}
	public bool IsHostile ()
	{
		return mIsHostile;	
	}
	
	/*
	 * ATTACK
	 */
	public bool IsSkillSelected ()
	{
		//TODO
		return true;
	}
	
	public bool IsAttackCooling ()
	{
		Debug.Log ("Check Attack cooling: " + mIsSkillCooling[mCurrentSkill]);
		return mIsSkillCooling[mCurrentSkill];	
	}
	
	public void AnimateAttack ()
	{
		//TODO
		StartCoroutine("CoroutineAttack");
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
	public void AttackAction ()
	{
		//TODO
	}
	
	/*
	 * SKILL
	 */
	public void SelectSkill ()
	{
		//TODO
		// Depending on the enemies stats, select the best possible skill
	}
	
	/*
	 * WANDER
	 */
	public bool IsWandering ()
	{
		return mIsWandering;	
	}
	public void SetWanderDirection ()
	{
		float dirCheck = transform.position.x - mPath[mNextNode].transform.position.x;
		
		Vector3 scale = gameObject.transform.localScale;
		scale.x = Mathf.Abs(scale.x);
		
		if ( dirCheck >= 0) {
			mDirection = -1;
			scale.x *= -1;
		}
		else {
			mDirection = 1;
			scale.x *= 1;
		}
		
		gameObject.transform.localScale = scale;
	}
	public void AnimateWander ()
	{
	}
	
	public void WanderAction ()
	{
		mIsWandering = true;
		if (Mathf.Abs(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), 
						new Vector2(mPath[mNextNode].transform.position.x, mPath[mNextNode].transform.position.y))) < kMinNodeDistance)
		{
			mIsWandering = false;
			if (++mNextNode >= mPath.Count) {
				mNextNode = 0;
				mMoveVelocity.x = 0;
			}
		}
		if (mIsWandering)
			mMoveVelocity.x = mDirection * mEnemy.MaxWanderSpeed;
		
		mCharacter.Move (mMoveVelocity * Time.deltaTime);
	}

	/*
	 * IDLE
	 */
	public bool IsIdle ()
	{
		return mIsIdle;	
	}
	public void AnimateIdle ()
	{	
	}
	IEnumerator CoroutineIdleTime ()
	{
		mIsIdle = true;
		yield return new WaitForSeconds(kNodeWaitTime);
		mIsIdle = false;
	}
	public void IdleInit ()
	{
		StartCoroutine("CoroutineIdleTime");	
	}
	public void IdleAction ()
	{
		mMoveVelocity.x = 0;
	}
	
	private void ApplyGravity ()
	{
		if (mIsAirborne)
		{
			mMoveVelocity.y -= kGravity * Time.deltaTime;
		}
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
		
		if (!mIsAirborne)
        mIsAirborne = (	!Physics.Raycast(pos, down, out hit, mMoveVelocity.y * Time.deltaTime + 10) && 
						!Physics.Raycast(pos2, down, out hit2, mMoveVelocity.y * Time.deltaTime + 10));
		
		Vector3 debugVector = mMoveVelocity;
		debugVector.y = debugVector.y * Time.deltaTime + 10;
		
		//Debug.DrawRay(pos, debugVector * Time.deltaTime, Color.red);
		//Debug.DrawRay(pos2, debugVector * Time.deltaTime, Color.red);
	}
	
	public void OnControllerColliderHit (ControllerColliderHit hit)
	{
		int terrainLayer = LayerMask.NameToLayer("Terrain");
		
		if (hit.gameObject.layer == terrainLayer && mCharacter.collisionFlags == CollisionFlags.Below)
		{
			mMoveVelocity.y = 0;
			mIsAirborne = false;
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
	
	void Move()
	{
		//mCharacter.Move (mMoveVelocity * Time.deltaTime);
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
					AudioSource.PlayClipAtPoint((AudioClip)Resources.Load ("enemy-hit"), new Vector3(0,0,0), 8);
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

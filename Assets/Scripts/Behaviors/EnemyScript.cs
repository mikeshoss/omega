using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class EnemyScript : MonoBehaviour {
	
	private EnemyAI mAI;
	private EnemyData mEnemy;
	private exSprite mSprite;
	private CharacterController mCharacter;
	
	private Skill mSelectedSkill;
	private float mCurrentHealth;
	private float mCurrentEnergy;
	private bool mIsDead;
	private bool mIsHostile;
	private bool mIsEnemyInRange;
	private bool mIsSeeking;
	private bool mIsWandering;
	private bool mIsIdle;
	private bool mIsAttacking;
	private bool mIsAirborne;
	
	private bool mWanderSucceeded;
	private bool mIdleSucceeded;
	
	private int mDirection;
	private int mDesiredDirection;
	private float mHorizontalSeeingRange;
	private float mVerticalSeeingRange;
	
	private Vector3 mDesiredLocation;
	private Vector3 mMoveVelocity;
	
	private const float kGravity = 1700.0f;
	
	// Use this for initialization
	void Start () {
		mEnemy = new EnemyData(1, null);
		mSelectedSkill = null;
		mCurrentHealth = mEnemy.MaxHealth;
		mCurrentEnergy = mEnemy.MaxEnergy;
		mIsDead = false;
		mIsHostile = false;
		mIsEnemyInRange = false;
		mIsSeeking = false;
		mIsWandering = false;
		mIsIdle = true;
		mIsAttacking = true;
		mIsAirborne = true;
		
		mWanderSucceeded = false;
		mIdleSucceeded = false;
		
		mDirection = 1;
		mHorizontalSeeingRange = 700.0f;
		mVerticalSeeingRange = 200.0f;
		
		mMoveVelocity = new Vector3(0,0,0);
		mDesiredLocation = transform.position;
		mSprite = (exSprite)GetComponent<exSprite>();
		mCharacter = (CharacterController)GetComponent<CharacterController>();
		mAI = (EnemyAI)gameObject.AddComponent("EnemyAI");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate () {
		if (!mIsDead)
		{
			CheckGrounded ();
			ApplyGravity ();
			CheckHostile ();
			CheckEnemyInRange ();
			Move ();
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
	}
	
	/*
	 * DIE
	 */
	public bool IsDying ()
	{
		return mCurrentHealth <= 0;	
	}
	public void AnimateDie ()
	{
		// set die animation
		// start coroutine die
		StartCoroutine("CoroutineDieTime");
	}
	IEnumerator CoroutineDieTime ()
	{
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
		return !mIsDead;	
	}
	
	/*
	 * HOSTILE
	 */
	void CheckHostile ()
	{
		
		GameObject player = GameObject.FindGameObjectWithTag("Character");
		
		mIsHostile = false;
		
		if (player.transform.position.x - mHorizontalSeeingRange <= transform.position.x && transform.position.x <= player.transform.position.x + mHorizontalSeeingRange)
		{
			mIsHostile = true;
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
		//if (mSelectedSkill == null)
		//	return false;
		
		return true;
	}
	void CheckEnemyInRange ()
	{
		mIsEnemyInRange = false;
		if (mIsHostile)
		{
			//TODO:
			// use skills attack range to decide if attack will attack
			mIsEnemyInRange = true;
		}
	}
	public bool IsEnemyInRange ()
	{
		return mIsEnemyInRange;	
	}
	public void AnimateAttack ()
	{
	}
	IEnumerator CoroutineAttack ()
	{
		mIsAttacking = true;
		yield return new WaitForSeconds(1.0f);
		mIsAttacking = false;
		StopCoroutine("CoroutineAttack");
	}
	public void AttackAction ()
	{
		
	}
	
	/*
	 * SKILL
	 */
	public void SelectSkill ()
	{
		// Depending on the enemies stats, select the best possible skill
		mSelectedSkill = null;
	}

	/*
	 * SEEK
	 */
	public bool IsSeeking ()
	{
		return mIsSeeking;
	}
	public void SetDesiredSeekLocation ()
	{
		// Find the players location and set that to the desired location
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		mDesiredLocation = player.transform.position;
	}
	public void FindPath ()
	{
		//astar pathfinding	
	}
	public void AnimateSeek ()
	{
		// seek animation	
	}
	public void SeekAction ()
	{
		// seek action
		// character controller, move to desired location
	}

	/*
	 * WANDER
	 */
	public bool IsWandering ()
	{
		return mIsWandering;	
	}
	public void SetDesiredWanderLocation ()
	{
		// random path node in the area of the enemy
		float x = Random.Range(-5, 5);
		if (x <= 0) {
			mDesiredDirection = -1;
		} else {
			mDesiredDirection = 1;
		}
	}
	public void AnimateWander ()
	{
	}
	IEnumerator CoroutineWanderTime ()
	{
		mIsWandering = true;
		yield return new WaitForSeconds(2.0f);
		mIsWandering = false;
		StopCoroutine("CoroutineWanderTime");
	}
	public void WanderInit ()
	{
		StartCoroutine("CoroutineWanderTime");	
	}
	
	public void WanderAction ()
	{
		if (IsEndOfPlatform())
		{
			Debug.Log ("End of Platform");
			mIsWandering = false;
			mMoveVelocity.x = 0;
			return;
		}
		
		// Move along path to the node
		Vector3 scale = gameObject.transform.localScale;
		scale.x = Mathf.Abs(scale.x);
		
		mMoveVelocity.x = mDesiredDirection * mEnemy.MaxWanderSpeed;
		
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
		yield return new WaitForSeconds(3.0f);
		mIsIdle = false;
		StopCoroutine("CoroutineIdleTime");
	}
	public void IdleInit ()
	{
		StartCoroutine("CoroutineIdleTime");	
	}
	public void IdleAction ()
	{
		mMoveVelocity = new Vector3(0,0,0);
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
		pos.x -= mCharacter.radius;

		RaycastHit hit2;

		Vector3 pos2 = transform.position;

		pos2.y -= (height / 2.0f) * transform.localScale.y - 5;
		pos2.x += mCharacter.radius;

        mIsAirborne = (!Physics.Raycast(pos2, down, out hit2, mMoveVelocity.y * Time.deltaTime + 10) && !Physics.Raycast(pos, down, out hit2, mMoveVelocity.y * Time.deltaTime + 10));
		
		Vector3 debugVector = mMoveVelocity;
		debugVector.y = debugVector.y * Time.deltaTime + 10;
		
		Debug.DrawRay(pos, debugVector * Time.deltaTime, Color.red);
		Debug.DrawRay(pos2, debugVector * Time.deltaTime, Color.red);
	}
	
	private bool IsEndOfPlatform ()
	{
		RaycastHit hit;
		Vector3 down = transform.TransformDirection(-Vector3.up);
		float height = mCharacter.height;

		Vector3 pos = transform.position;

		pos.y -= (height / 2.0f) * transform.localScale.y - 5;
		pos.x -= mCharacter.radius * 1.5f;

		RaycastHit hit2;

		Vector3 pos2 = transform.position;

		pos2.y -= (height / 2.0f) * transform.localScale.y - 5;
		pos2.x += mCharacter.radius * 1.5f;
		
		Vector3 debugVector = mMoveVelocity;
		debugVector.y = debugVector.y * Time.deltaTime + 10;
		
		Debug.DrawRay(pos, new Vector3(0, -100, 0), Color.yellow);
		Debug.DrawRay(pos2, new Vector3(0, -100, 0), Color.yellow);
		
        return (!Physics.Raycast(pos2, down, out hit2, 100) || !Physics.Raycast(pos, down, out hit2, 100));
		
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
		mCharacter.Move (mMoveVelocity * Time.deltaTime);
	}
}

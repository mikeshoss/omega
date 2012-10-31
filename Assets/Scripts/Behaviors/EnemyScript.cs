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
	
	private bool mWanderSucceeded;
	private bool mIdleSucceeded;
	
	private float mHorizontalSeeingRange;
	private float mVerticalSeeingRange;
	
	private Vector3 mDesiredLocation;
	private Vector3 mMoveVelocity;
	
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
		
		mWanderSucceeded = false;
		mIdleSucceeded = false;
		
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
		//Debug.Log ("Players X coordinate" + player.transform.position.x);
		
		mIsHostile = false;
		
		if (player.transform.position.x - mHorizontalSeeingRange <= transform.position.x && transform.position.x <= player.transform.position.x + mHorizontalSeeingRange)
		{
			mIsHostile = true;
		}
	}
	public bool IsHostile ()
	{
		Debug.Log ("Hostile?: " + mIsHostile);
		return mIsHostile;	
	}
	
	/*
	 * ATTACK
	 */
	public bool IsSkillSelected ()
	{
		if (mSelectedSkill == null)
			return false;
		
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
	public void AttackAction ()
	{
		
	}
	
	/*
	 * SKILL
	 */
	public void SelectSkill ()
	{
		// Depending on the enemies stats, select the best possible skill
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
		
		if (x <= 0)
		{
			mMoveVelocity.x = -300.0f;	
		} else {
			mMoveVelocity.x = 300.0f;	
		}
	}
	public void AnimateWander ()
	{
		StartCoroutine("CoroutineWanderTime");
	}
	IEnumerator CoroutineWanderTime ()
	{
		mIsWandering = true;
		yield return new WaitForSeconds(2.0f);
		mIsWandering = false;
		StopCoroutine("CoroutineWanderTime");
	}
	public void WanderAction ()
	{
		Debug.Log("Wander Action");
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
		StartCoroutine("CoroutineIdleTime");
	}
	IEnumerator CoroutineIdleTime ()
	{
		mIsIdle = true;
		yield return new WaitForSeconds(3.0f);
		mIsIdle = false;
		StopCoroutine("CoroutineIdleTime");
	}
	public void IdleAction ()
	{
		Debug.Log("Idle Action");
		mMoveVelocity = new Vector3(0,0,0);
	}
	
	void Move()
	{
		mCharacter.Move (mMoveVelocity * Time.deltaTime);
	}
	
}

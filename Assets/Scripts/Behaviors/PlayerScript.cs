using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private PlayerAI mAI;
	private PlayerData mPlayer;
	
	private Vector3 mMoveVelocity;
	private int mCurrentJumpNumber;
	
	private bool mJumpPressed;
	private bool mAirborne;
	private bool mSkillPressed;
	private bool mAttackPressed;
	private bool mCurrentSkill;
	
	// Use this for initialization
	void Start () {
		mMoveVelocity = new Vector3(0,0,0);
		mAirborne = false;
		mAI = (PlayerAI)gameObject.AddComponent("PlayerAI");
	}
	
	// Update is called once per frame
	void Update () {
		mJumpPressed = Input.GetKey(KeyCode.W);
		mSkillPressed = Input.GetKey(KeyCode.Alpha1) || 
						Input.GetKey(KeyCode.Alpha2) || 
						Input.GetKey(KeyCode.Alpha3) || 
						Input.GetKey(KeyCode.Alpha4);
		mAttackPressed = Input.GetKey(KeyCode.Space);
	}
	
	
	
	void FixedUpdate () {
		ApplyGravity();
		Move ();
	}
	
	public bool SkillCheckInput ()
	{
		return mSkillPressed;
	}
	
	public bool AttackCheckInput ()
	{
		return mAttackPressed;
	}
	
	public void ActiveSkill () 
	{
	}
	
	public bool CheckSkillActive ()
	{
		return true;
	}
	
	
	public bool JumpCheckInput ()	
	{
		// if player hit jump key, return true
		return mJumpPressed;
		
	}
	
	public bool JumpCheckAirborne ()
	{
		// if he is not airborne, allow a jump
		if (mAirborne)
		{
			// if he is airborne, check for air jump
			return false;
		}
		return true;
	}
	
	public void JumpAction ()
	{
		mAirborne = true;
		mMoveVelocity.y += 10.0f;
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
	
	public void Attack()
	{
	}
}

using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private PlayerAI mAI;
	private PlayerData mPlayer;
	
	private Vector3 mMoveVelocity;
	private int mCurrentJumpNumber;
	
	private bool mJumpPressed;
	private bool mJumpWait;
	private bool mJumpMax;
	private bool mAirborne;
	private bool mSkillPressed;
	private bool mAttackPressed;
	private bool mCurrentSkill;
	
	// Use this for initialization
	void Start () {
		mMoveVelocity = new Vector3(0,0,0);
		mAirborne = false;
		mJumpWait = true;
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
	
	public bool CheckAirborne ()
	{
		if (mAirborne)
		{
			return false;
		}
		return true;
	}
	
	public void JumpAction ()
	{
		mAirborne = true;
		StartCoroutine(CoroutineJumpWaitTime());
		mMoveVelocity.y = mPlayer.JumpVelocity;
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

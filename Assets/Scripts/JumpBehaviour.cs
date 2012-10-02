using UnityEngine;
using System.Collections;

public class JumpBehaviour : BaseBehaviour {
	
	
	protected int 		mBaseMaxAirJump;
	protected int 		mCurrentAirJump;
	
	protected float		mGravity;
	protected float		mJumpSpeed;
	
	protected bool		mEnableGravity;
	protected bool		mEnableWallJumping;
	protected Vector3	mJumpVelocity;
	
	void Jump ()
	{
		float deltaT = Time.deltaTime;	
	}
	
	void AirJump ()
	{
		
	}
	
	void WallJump ()
	{
		
	}
	
	public void EnableGravity ()
	{
		mEnableGravity = true;	
	}
	
	public void DisableGravity ()
	{
		mEnableGravity = false;	
	}
	
	public void EnableWallJumping ()
	{
		mEnableWallJumping = true;	
	}
	
	public void DisableWallJumping ()
	{
		mEnableWallJumping = false;	
	}
	
}

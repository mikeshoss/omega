using UnityEngine;
using System.Collections;

public abstract class MoveBehaviour : BaseBehaviour {
	
	// Flags to handle functionality
	protected bool 		mMove;
	
	protected float 	mBaseMoveSpeed;
	protected float 	mCalcMoveSpeed;
	protected float		mBaseMoveAccel;
	protected float		mCalcMoveAccel;
	
	protected Vector3 	mMoveDirection;
	protected Vector3	mMoveVelocity;
	
	public MoveBehaviour (ICombatant origin)
		: base (origin)
	{
		mMove = true;
		mMoveDirection = new Vector3(0, 0, 0);
		mMoveVelocity = new Vector3(0, 0, 0);
		mBaseMoveSpeed = 500;
		mCalcMoveSpeed = mBaseMoveSpeed;
		mBaseMoveAccel = 90;
		mCalcMoveAccel = mBaseMoveAccel;
	}
	
	public MoveBehaviour (ICombatant origin, float baseMoveSpeed, float baseMoveAccel)
		: base (origin)
	{
		mMove = true;
		mMoveDirection = new Vector3(0, 0, 0);
		mMoveVelocity = new Vector3(0, 0, 0);
		mBaseMoveSpeed = baseMoveSpeed;
		mCalcMoveSpeed = mBaseMoveSpeed;
		mBaseMoveAccel = baseMoveAccel;
		mCalcMoveAccel = mBaseMoveAccel;
	}

	public void EnableMove ()
	{
		mMove = true;
	}
	
	public void DisableMove ()
	{
		mMove = false;
	}
	
	public abstract void Left ();
	
	public abstract void Up ();
	
	public abstract void Down ();
	
	public abstract void Right ();
	
	protected abstract void Move ();
	
	public abstract void OnControllerColliderHit (ControllerColliderHit hit);
}

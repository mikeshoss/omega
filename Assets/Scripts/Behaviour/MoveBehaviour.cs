using UnityEngine;
using System.Collections;

public abstract class MoveBehaviour : BaseBehaviour {
	
	protected float 	mBaseMoveSpeed;
	protected float 	mCalcMoveSpeed;
	protected float		mBaseMoveAccel;
	protected float		mCalcMoveAccel;
	
	protected Vector3 	mMoveDirection;
	protected Vector3	mMoveVelocity;
	protected int 		mDirection = 1;
	
	public int direction
	{
		get
		{
			return mDirection;	
		}
	}
	
	public MoveBehaviour (ICombatant origin)
		: base (origin)
	{
		mMoveDirection = new Vector3(0, 0, 0);
		mMoveVelocity = new Vector3(0, 0, 0);
		mBaseMoveSpeed = 500;
		mCalcMoveSpeed = mBaseMoveSpeed;
		mBaseMoveAccel = 90;
		mCalcMoveAccel = mBaseMoveAccel;
	}
	
	public MoveBehaviour (ICombatant origin, float baseMoveSpeed, float baseMoveAccel)
		: this (origin)
	{
		mBaseMoveSpeed = baseMoveSpeed;
		mCalcMoveSpeed = mBaseMoveSpeed;
		mBaseMoveAccel = baseMoveAccel;
		mCalcMoveAccel = mBaseMoveAccel;
	}
	
	public abstract void Left ();
	
	public abstract void Up ();
	
	public abstract void Down ();
	
	public abstract void Right ();
	
	protected abstract void Move ();
	
	public abstract void OnControllerColliderHit (ControllerColliderHit hit);
}

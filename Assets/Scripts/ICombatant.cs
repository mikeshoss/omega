using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public abstract class ICombatant : MonoBehaviour {
	
	protected MoveBehaviour mMovement;
	protected HitFlag mHitFlag;
	
	public enum HitFlag
	{
		GROUNDED,
		AERIAL
	}
	
	void Start ()
	{
		mMovement = null;
		mHitFlag = HitFlag.GROUNDED;
	}
	
	public void SetMovementBehaviour (MoveBehaviour behaviour)
	{
		mMovement = behaviour;	
	}
	
	public HitFlag GetHitFlag ()
	{
		return mHitFlag;	
	}
	
	public void SetHitFlag (HitFlag hitFlag)
	{
		mHitFlag = hitFlag;
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		mMovement.OnControllerColliderHit(hit);
	}
}

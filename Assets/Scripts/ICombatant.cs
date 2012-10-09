using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public abstract class ICombatant : MonoBehaviour {
	
	protected MoveBehaviour mMovement;
	protected HitFlag mHitFlag;
	
	public MoveBehaviour movement
	{
		get
		{
			return mMovement;	
		}
		set
		{
			mMovement = value;	
		}
	}
	
	public HitFlag hitFlag
	{
		get
		{
			return mHitFlag;	
		}
		set
		{
			mHitFlag = value;	
		}
	}
	
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
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		mMovement.OnControllerColliderHit(hit);
	}
}

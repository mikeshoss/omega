using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public abstract class ICombatant : MonoBehaviour {
	
	protected MoveBehaviour mMovement;
	
	void Start ()
	{
		mMovement = null;
	}
	
	public void SetMovementBehaviour (MoveBehaviour behaviour)
	{
		mMovement = behaviour;	
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		mMovement.OnControllerColliderHit(hit);
	}
}

using UnityEngine;
using System.Collections;

public class PlayerScript : ICombatant {
	
	void Start ()
	{
		mMovement = new GroundMoveBehaviour(this,550,70,600,2);
	}
	
	void Update ()
	{
		CheckInput ();
		
		if (mMovement != null)
		{
			mMovement.Update();
		}
	}

	private void CheckInput ()
	{
		// Movement
		if (Input.GetKey(KeyCode.D))
		{
			mMovement.Right ();
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			mMovement.Up ();
		}
		if (Input.GetKey(KeyCode.A))
		{
			mMovement.Left ();
		}
		
		// Game State Changes
		
		// Action Bar
		if (Input.GetKey(KeyCode.Alpha1))
		{
			
		}
		if (Input.GetKey(KeyCode.Alpha2))
		{
			
		}
		if (Input.GetKey(KeyCode.Alpha3))
		{
			
		}
		if (Input.GetKey(KeyCode.Alpha4))
		{
			
		}
		
		// Action
		 
	}
	
	void OnTriggerEnter (Collider collider)
	{
	
	}

}
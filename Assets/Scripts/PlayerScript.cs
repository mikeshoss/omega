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
		if (Input.GetKey(KeyCode.Q))
		{
			
		}
		if (Input.GetKey(KeyCode.E))
		{
			
		}
		
		// Action
		if (Input.GetKey(KeyCode.Space))
		{
			
		}
	}
	
	void OnTriggerEnter (Collider collider)
	{
	
	}
}
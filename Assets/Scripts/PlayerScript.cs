using UnityEngine;
using System.Collections;

public class PlayerScript : ICombatant {
	
	void Start ()
	{
		mMovement = new GroundMoveBehaviour(this,500,70,600,2);
	}
	
	void Update ()
	{
		if (mMovement != null)
		{
			mMovement.Update();
		}
	}

	void OnTriggerEnter (Collider collider)
	{
	
	}

}
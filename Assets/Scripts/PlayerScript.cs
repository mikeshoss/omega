using UnityEngine;
using System.Collections;

public class PlayerScript : ICombatant {
	
	void Start ()
	{
		mMovement = new GroundMoveBehaviour(this,500,50,800,2);
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
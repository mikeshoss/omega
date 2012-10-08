using UnityEngine;
using System.Collections;

public class AirMoveBehaviour : MoveBehaviour {
	
	public AirMoveBehaviour (ICombatant origin)
		: base(origin)
	{
		mBehaviourType = BType.AIR_MOVE_BEHAVIOUR;
	}
	
	public override void Update ()
	{
		if (mIsEnabled)
		{
			
			if (Input.GetKey(KeyCode.D))
			{
				Right ();
			}
			if (Input.GetKey(KeyCode.W))
			{
				Up ();
			}
			if (Input.GetKey(KeyCode.A))
			{
				Down ();
			}
			if (Input.GetKey(KeyCode.S))
			{
				Left ();
			}
			if (Input.GetKey(KeyCode.Alpha2))
			{
				mOrigin.SetMovementBehaviour(new GroundMoveBehaviour(mOrigin));	
			}
		}
	}
	
	public override void Left ()
	{
		mOrigin.transform.Translate(new Vector3(0, -500 * Time.deltaTime, 0));
	}
	
	public override void Up ()
	{
		mOrigin.transform.Translate(new Vector3(0, 500 * Time.deltaTime, 0));
	}
	
	public override void Right ()
	{
		mOrigin.transform.Translate(new Vector3(500 * Time.deltaTime, 0,0));
	}
	
	public override void Down ()
	{
		mOrigin.transform.Translate(new Vector3(-500 * Time.deltaTime, 0,0));
	}
	
	protected override void Move ()
	{
		
	}
	
	public override void OnControllerColliderHit (ControllerColliderHit hit)
	{
		
	}
}

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

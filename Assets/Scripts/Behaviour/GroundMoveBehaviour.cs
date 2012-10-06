using UnityEngine;
using System.Collections;

public class GroundMoveBehaviour : MoveBehaviour {
	
	
	// Flags used to toggle functionality
	private bool mJump = true;
	private bool mGravity = true;
	
	// Flags to handle states
	private bool mAirborne = false;
	
	// -1 = left
	// 1 = right
	private int mOrientation = 1;
	
	// Friction
	private float mGroundFriction = 45;
	private float mWindFriction = 1;
	
	private float mBaseJumpSpeed = 600;
	private float mCalcJumpSpeed = 600;
	private float mGravitySpeed = 30;
	
	private int mMaxAirJump = 1;
	private int mCurrentAirJump = 0;
	
	public GroundMoveBehaviour (ICombatant origin)
		: base(origin)
	{
		mBehaviourType = BType.GROUND_MOVE_BEHAVIOUR;
	}
	
	public GroundMoveBehaviour (ICombatant origin, float baseMoveSpeed, float baseMoveAccel)
		: base(origin, baseMoveSpeed, baseMoveAccel)
	{
		mBehaviourType = BType.GROUND_MOVE_BEHAVIOUR;
	}
	
	public GroundMoveBehaviour (ICombatant origin, float baseMoveSpeed, float baseMoveAccel, float baseJumpSpeed, int maxAirJump)
		: base(origin, baseMoveSpeed, baseMoveAccel)
	{
		mBehaviourType = BType.GROUND_MOVE_BEHAVIOUR;
		mBaseJumpSpeed = baseJumpSpeed;
		mCalcJumpSpeed = mBaseJumpSpeed;
		mMaxAirJump = maxAirJump;
		mCurrentAirJump = 0;
	}
	
	public override void Update ()
	{
		if (mIsEnabled)
		{
			CheckInput ();
			
			CheckGrounded ();
			CheckOrientation ();
			if (mMove)
			{
				if (mAirborne)
				{	
					ApplyWindFriction ();
					ApplyGravity ();
				}
				else
				{
					ApplyGroundFriction ();
				}
				Move ();
			}
		}
	}
	
	private void CheckInput ()
	{
		if (Input.GetKey(KeyCode.D))
		{
			Right ();
		}
		if (mJump && Input.GetKeyDown(KeyCode.W))
		{
			Up ();
		}
		if (Input.GetKey(KeyCode.A))
		{
			Left ();
		}
		if (Input.GetKey(KeyCode.Alpha1))
		{
			mOrigin.SetMovementBehaviour(new AirMoveBehaviour(mOrigin));	
		}
	}
	
	protected override void Left ()
	{
		// Apply acceleration
		mMoveVelocity.x -= mCalcMoveAccel;
		
		if (mMoveVelocity.x < -mCalcMoveSpeed)
		{
			mMoveVelocity.x = -mCalcMoveSpeed;	
		}
		
	}
	
	protected override void Up ()
	{
		// Apply initial jump velocity
		if (!mAirborne || mCurrentAirJump < mMaxAirJump)
		{
			mAirborne = true;
			mCurrentAirJump++;
			mMoveVelocity.y = mCalcJumpSpeed;
		}
	}
	
	protected override void Down ()
	{
		//No-op
	}	
	
	protected override void Right ()
	{
		mMoveVelocity.x += mCalcMoveAccel;
		
		if (mMoveVelocity.x > mCalcMoveSpeed)
		{
			mMoveVelocity.x = mCalcMoveSpeed;	
		}
	}
	
	protected override void Move ()
	{

		CharacterController cc = (CharacterController)mOrigin.GetComponent<CharacterController>();
		cc.Move(mMoveVelocity * Time.deltaTime);
	}
	
	public override void OnControllerColliderHit (ControllerColliderHit hit)
	{
		Debug.Log(hit.gameObject.tag);
		CharacterController cc = (CharacterController)mOrigin.GetComponent<CharacterController>();
		
		if (hit.gameObject.tag == "Ground" && cc.collisionFlags == CollisionFlags.Below)
		{
			
			mMoveVelocity.y = 0;
			mAirborne = false;
			mCurrentAirJump = 0;
		}
		
		// If player hits a wall, set x velocity to 0.
		if (hit.gameObject.tag == "Ground" && cc.collisionFlags == CollisionFlags.Sides)
		{
			mMoveVelocity.x = 0;
			
			// if wall jumping enabled
			mCurrentAirJump = 0;
		}
		// If head hits the top of a terrain, bounce back down
		if (hit.gameObject.tag == "Ground" && cc.collisionFlags == CollisionFlags.CollidedAbove)
		{
			mMoveVelocity.y = 0f;	
		}
		if (hit.gameObject.tag == "Fireball")
		{
			hit.gameObject.SendMessage("ApplyDamage");	
		}
	}
	
	public void EnableJump ()
	{
		mJump = true;	
	}
	
	public void DisableJump ()
	{
		mJump = false;	
	}
	
	private void ApplyGroundFriction ()
	{
		// If moving right
		if (mMoveVelocity.x > 0)
		{
			mMoveVelocity.x -= mGroundFriction;
			
			// If friction depletes velocity
			if (mMoveVelocity.x < 0)
			{
				mMoveVelocity.x = 0;	
			}
		}
		// If moving left
		if (mMoveVelocity.x < 0)
		{
			mMoveVelocity.x += mGroundFriction;
			
			// If friction depletes velocity
			if (mMoveVelocity.x > 0)
			{
				mMoveVelocity.x = 0;	
			}
		}
	}
	
	private void ApplyWindFriction ()
	{
		// If moving right
		if (mMoveVelocity.x > 0)
		{
			mMoveVelocity.x -= mWindFriction;
			
			// If friction depletes velocity
			if (mMoveVelocity.x < 0)
			{
				mMoveVelocity.x = 0;	
			}
		}
		// If moving left
		if (mMoveVelocity.x < 0)
		{
			mMoveVelocity.x += mWindFriction;
			
			// If friction depletes velocity
			if (mMoveVelocity.x > 0)
			{
				mMoveVelocity.x = 0;	
			}
		}
	}
	
	private void ApplyGravity ()
	{
		mMoveVelocity.y -= mGravitySpeed;
		
	}
	
	private void CheckGrounded ()
	{
		RaycastHit hit;
		Vector3 down = mOrigin.transform.TransformDirection(-Vector3.up);
		CharacterController cc = (CharacterController)mOrigin.GetComponent<CharacterController>();
		float height = cc.height;

		Vector3 pos = mOrigin.transform.position;
		
		pos.y -= (height / 2) * mOrigin.transform.localScale.y;
		pos.x -= cc.radius;
		
		RaycastHit hit2;

		Vector3 pos2 = mOrigin.transform.position;
		
		pos2.y -= (height / 2) * mOrigin.transform.localScale.y;
		pos2.x += cc.radius;
		
        mAirborne = (!Physics.Raycast(pos2, down, out hit2, mMoveVelocity.y * Time.deltaTime + 1) && !Physics.Raycast(pos, down, out hit, mMoveVelocity.y * Time.deltaTime + 1));
		

		Debug.DrawRay(pos, mMoveVelocity * Time.deltaTime, Color.red);
		Debug.DrawRay(pos2, mMoveVelocity * Time.deltaTime, Color.red);
	}
	
	private void CheckOrientation ()
	{
		if (mMoveVelocity.x > 0)
		{
			mOrientation = 1;
			
			
			Vector3 localScale = mOrigin.transform.localScale;
			localScale.x = Mathf.Abs(mOrigin.transform.localScale.x);
			mOrigin.transform.localScale = localScale;
		}
		if (mMoveVelocity.x < 0)
		{
			mOrientation = -1;
			
			Vector3 localScale = mOrigin.transform.localScale;
			localScale.x = -Mathf.Abs(mOrigin.transform.localScale.x);
			mOrigin.transform.localScale = localScale;
		}
	}
}

//http://www.youtube.com/watch?v=zyWp68-fJVU&feature=relmfu
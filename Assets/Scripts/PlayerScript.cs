using UnityEngine;
using System.Collections;

public class PlayerScript : ICombatant {
	
	public Vector3 moveDirection = new Vector3(0,0,0);
	private bool jumping = false;
	private float gravity = 10.0f;
	
	void Start () {
		mActiveSkills = new Skill[4];
		mActiveSkills[0] = new Fireball(this);
	}
	
	void Update ()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			GameObject go = (GameObject)Instantiate(Resources.Load("Fireball"));
			SkillScript ss = go.GetComponent<SkillScript>();
			ss.SetSkill(mActiveSkills[0]);
			jumping = false;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		
		
		if (Input.GetKey(KeyCode.D))
		{
			mOrientation = "right";
			RaycastHit hitRight;
			int moveRight;
			
			BoxCollider bRight = (BoxCollider)gameObject.GetComponent("BoxCollider");
			float width = bRight.size.x;
			Vector3 posRight = transform.position;
			
			posRight.x += (width / 2);
	        if (Physics.Raycast(posRight, Vector3.right, out hitRight, 500.00f * Time.deltaTime + moveDirection.x)){
	        	moveRight = 0;
				jumping = false;
			}
			else
			{	
				moveRight = 500;
			}
	
			transform.Translate(new Vector3(moveRight * Time.deltaTime, 0, 0));	
		}
		
		if (Input.GetKey(KeyCode.A))
		{
			mOrientation = "left";
			RaycastHit hitLeft;
			int moveLeft;
			
			BoxCollider bLeft = (BoxCollider)gameObject.GetComponent("BoxCollider");
			float widthl = bLeft.size.x;
			Vector3 posLeft = transform.position;
			
		posLeft.x -= (widthl / 2);
	        if (Physics.Raycast(posLeft, Vector3.left, out hitLeft, 500.00f * Time.deltaTime - moveDirection.x)){
	        	moveLeft = 0;
				jumping = false;
			}
			else
			{	
				moveLeft = -500;
			}
			
			transform.Translate(new Vector3(moveLeft * Time.deltaTime, 0, 0));	
		}
		
		if (Input.GetKey (KeyCode.W))
		{
			if (!jumping)
			{
				moveDirection.y = 15.0f;	
				gravity = 20.0f;
			}
			jumping = true;
		}
		
		RaycastHit hit;
		Vector3 myDown = transform.TransformDirection(-Vector3.up);
		BoxCollider b = (BoxCollider)gameObject.GetComponent("BoxCollider");
		float height = b.size.y;
		
		
		Vector3 pos = transform.position;
		
		pos.y -= (height/2);
		pos.x -= (b.size.x / 2);
        if (Physics.Raycast(pos, myDown, out hit, 30.00f * Time.deltaTime - moveDirection.y)){
        	gravity = 0.0f;
			moveDirection.y = 0;
		}
		else
		{	
			gravity = 30.0f;
		}
		
		RaycastHit hit2;
		Vector3 pos2 = pos;
		pos2.x += b.size.x;
		if (Physics.Raycast(pos2, myDown, out hit2, 30.00f * Time.deltaTime - moveDirection.y)){
        	gravity = 0.0f;
			moveDirection.y = 0;
		}
		else
		{	
			gravity = 30.0f;
		}
		Debug.DrawRay(pos,myDown * 30 * Time.deltaTime, Color.red);
		Debug.DrawRay(pos2,myDown * 30 * Time.deltaTime, Color.red);
	
		
		moveDirection.y -= (gravity * Time.deltaTime);
		
		transform.Translate(moveDirection);
		
	}

	void OnTriggerEnter (Collider collider) {
		jumping = false;
	}

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : ICombatant {
	
	private PlayerData mPlayerData;
	
	void Start ()
	{
		mMovement = new GroundMoveBehaviour(this,550,70,600,2);
		mPlayerData = new PlayerData(this);
		// Animations
		mSprite = (exSprite)GetComponent<exSprite>();
		
		
		// Skills
		mSkillSets = mPlayerData.skillSets;
		mSelectedSkillSet = mPlayerData.selectedSkillSet;
		mSelectedSkill = mSelectedSkillSet.GetSkill(0);
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
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			mSelectedSkill = mSelectedSkillSet.GetSkill(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			mSelectedSkill = mSelectedSkillSet.GetSkill(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			
		}
		
		// Action
		if (Input.GetKeyDown(KeyCode.Space))
		{
			mSelectedSkill.Execute();
		}
	}
	
	public override void UpdateAnimation ()
	{
		
	}
	
	void OnTriggerEnter (Collider collider)
	{
	
	}
}
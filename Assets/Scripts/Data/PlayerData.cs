using UnityEngine;
using System.Collections;

public class PlayerData {
	
	// NOT USING
	private int mHealth;
	private int mEnergy;
	
	
	private int mMaxJump;
	
	public PlayerData (int health, int energy, int maxJump)
	{
		mHealth = health;
		mEnergy = energy;
		mMaxJump = maxJump;
	}
	
}

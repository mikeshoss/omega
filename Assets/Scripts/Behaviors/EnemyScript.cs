using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	
	private EnemyAI mAI;
	private EnemyData mEnemy;
	private exSprite mSprite;
	
	private float mCurrentHealth;
	private float mCurrentEnergy;
	private bool mDead;
	// Use this for initialization
	void Start () {
		mEnemy = new EnemyData(1, null);
		mCurrentHealth = mEnemy.MaxHealth;
		mCurrentEnergy = mEnemy.MaxEnergy;
		mSprite = (exSprite)GetComponent<exSprite>();
		mAI = (EnemyAI)gameObject.AddComponent("EnemyAI");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpate () {
		
	}
	
	/*
	 * DEAD
	 */
	public bool IsDead ()
	{
		return mDead;
	}
	public void DeadAction ()
	{
		//do nothing	
	}
	
	/*
	 * DIE
	 */
	public bool IsDying ()
	{
		return mCurrentHealth <= 0;	
	}
	public void AnimateDie ()
	{
		// set die animation
		// start coroutine die
		StartCoroutine("CoroutineDieTime");
	}
	IEnumerator CoroutineDieTime ()
	{
		yield return new WaitForSeconds(1.0f); // Animation die time
		mDead = true;
		StopCoroutine("CoroutineDieTime");
	}
	public void DieAction ()
	{
		// some sort of dying action	
	}


}

using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour {
	
	private Skill mSkill;
	// Use this for initialization
	void Start () {
	}
	
	public void SetSkill (Skill skill)
	{
		mSkill = skill;
	}
	
	public Skill GetSkill ()
	{
		return mSkill;	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 0, -90 * Time.deltaTime));
	}
}

using UnityEngine;
using System.Collections;

public class FireballScript : SkillScript {
	
	public override void Startup ()
	{
		new WaitForSeconds(mSkill.StartupTime);
		transform.position = mOrigin.transform.position;
		Vector3 scale = transform.localScale;
		scale.x *= mOrigin.Direction;
		transform.localScale = scale;
		MeshRenderer mr = (MeshRenderer)GetComponent<MeshRenderer>();
		mr.enabled = true;
	}
	
	public override void Active ()
	{
		
	}
	
	public override void End ()
	{
		mTarget.ApplyDamage(mSkill.Magnitude);
		new WaitForSeconds(mSkill.EndTime);
		Destroy(gameObject);
	}
	
	public void OnTriggerEnter (Collider other)
	{
	}
}

using UnityEngine;
using System.Collections;

public class IcepickScript : SkillScript {
	
	
	public override void Startup ()
	{
		new WaitForSeconds(mSkill.StartupTime);
		transform.position = mOrigin.transform.position;
		Vector3 scale = transform.localScale;
		scale.x *= mOrigin.Direction;
		transform.localScale = scale;
		MeshRenderer mr = (MeshRenderer)GetComponent<MeshRenderer>();
		mr.enabled = true;
		mIsActive = true;
	}
	
	public override void Active ()
	{
		transform.Translate(new Vector3(2000, 0, 0) * Time.deltaTime * (transform.localScale.x / Mathf.Abs(transform.localScale.x)));
	}
	
	public override void End ()
	{
		mTarget.ApplyDamage(mSkill.Magnitude);
		Debug.Log ("Health of target: " + mTarget.Health);
		Debug.Log ("yello" + mTarget.Health);
		new WaitForSeconds(mSkill.EndTime);
		Debug.Log ("hello" + mTarget.Health);
		Destroy(gameObject);
	}
}

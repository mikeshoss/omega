using UnityEngine;
using System.Collections;

public class IcepickScript : SkillScript {
	
	
	public override void Startup ()
	{
		new WaitForSeconds(mSkill.StartupTime);
		transform.position = mOrigin.transform.position;
		Vector3 scale = transform.localScale;
		scale.x *= (mOrigin.transform.localScale.x / Mathf.Abs(mOrigin.transform.localScale.x));
		transform.localScale = scale;
		MeshRenderer mr = (MeshRenderer)GetComponent<MeshRenderer>();
		mr.enabled = true;
		mIsActive = true;
		AudioSource.PlayClipAtPoint((AudioClip)Resources.Load ("ice"), new Vector3(0,0,0), 0.4f);
	}
	
	public override void Active ()
	{
		transform.Translate(new Vector3(2000, 0, 0) * Time.deltaTime * (transform.localScale.x / Mathf.Abs(transform.localScale.x)));
	}
	
	public override void End ()
	{
		mTarget.ApplyDamage(mSkill.Magnitude);
		new WaitForSeconds(mSkill.EndTime);
		GameObject go = (GameObject)Instantiate(Resources.Load ("DamageText"), transform.position, transform.localRotation);
		DamageTextScript dts = go.GetComponent<DamageTextScript>();
		dts.SetDamageText(mSkill.Magnitude.ToString());
		Destroy(gameObject);
	}
}

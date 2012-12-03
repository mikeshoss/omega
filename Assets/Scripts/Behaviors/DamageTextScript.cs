using UnityEngine;
using System.Collections;

public class DamageTextScript : MonoBehaviour {
	
	private Vector3 velocity;
	private string damage;
	private exSpriteFont font;
	
	// Use this for initialization
	void Start () {
		transform.Translate(new Vector3(0,50,0));
		velocity = new Vector3(50, 500, 0);
		font = (exSpriteFont)GetComponent<exSpriteFont>();
		font.text = damage;
		Destroy(gameObject, 1);
	}
	
	// Update is called once per frame
	void Update () {
		font.text = damage;
		transform.position += velocity * Time.deltaTime;
		
		velocity.y -= 500 * Time.deltaTime;
		
		velocity *= 0.9f;
	}
	
	public void SetDamageText(string text)
	{
		damage = text;	
	}
}

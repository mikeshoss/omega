using UnityEngine;
using System.Collections;

public class MenuOption : MonoBehaviour {
	
	private exSprite mSprite;
	
	// Use this for initialization
	void Start () {
		mSprite = (exSprite)GetComponent<exSprite>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetTexture (int index)
	{
		if (mSprite == null)
			mSprite = (exSprite)GetComponent<exSprite>();
		
		Debug.Log(mSprite);
		Debug.Log (mSprite.atlas);
		mSprite.SetSprite(mSprite.atlas, index, false);
	}
}

using UnityEngine;
using System.Collections;

public class SkillLevelScript : MonoBehaviour {
	
	public PlayerScript player;
	public int	skillIndex;
	
	private exSpriteFont font;
	// Use this for initialization
	void Start () {
		font = (exSpriteFont)GetComponent<exSpriteFont>();
		font.text = player.SelectedSkills[skillIndex].Level.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		font.text = player.SelectedSkills[skillIndex].Level.ToString();
	}
}

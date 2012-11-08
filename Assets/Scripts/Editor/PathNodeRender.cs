using UnityEditor;
using UnityEngine;

public class PathNodeRender : ScriptableWizard {
    
    [MenuItem ("Omega/Toggle PathNode Visibility")]
    static void CreateWizard () 
	{
        ScriptableWizard.DisplayWizard<PathNodeRender>("Hide Show PathNodes", "Show All", "Hide All");
    }
    void OnWizardCreate () 
	{
        
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Node");
		
		foreach (GameObject go in gos)
		{
			MeshRenderer mesh = (MeshRenderer)go.GetComponent<MeshRenderer>();	
			mesh.enabled = true;
		}
		
    }
	
	void OnWizardOtherButton ()
	{
		
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Node");
		
		foreach (GameObject go in gos)
		{
			MeshRenderer mesh = (MeshRenderer)go.GetComponent<MeshRenderer>();	
			mesh.enabled = false;
		}
		
	}
}

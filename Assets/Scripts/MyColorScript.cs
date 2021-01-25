using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyColorScript : MonoBehaviour
{
	public GameObject plane;
	private MeshRenderer myRenderer;

	// Start is called before the first frame update
	private void Start() {
		Debug.Log("233");
	}

	public void OnClickColor() {
		myRenderer = plane.GetComponent<MeshRenderer>();
		myRenderer.enabled = !myRenderer.enabled;
	}

}

using UnityEngine;
using System.Collections;

public class lamp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	bool show = false;
	IEnumerator wait()
	{
		show = true;
		gameObject.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(Random.Range(0f,2));
		gameObject.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.5f);
		show = false;

	}
	// Update is called once per frame
	void Update () {

		if (show == false)
		{
			show = true;
			StartCoroutine(wait());
		}
	}
}

using UnityEngine;
using System.Collections;

public class car_controller : MonoBehaviour {

    public AnimationClip clip;
    Animation anim;
	// Use this for initialization
	void Start () {
        anim = (Animation)GetComponent("Animation");
        anim.AddClip(clip,"main");
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

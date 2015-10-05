using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour 
{
    public int index;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if ( index == 1 )
        transform.Rotate(0, 0, (control.power + control.AD) * 3, Space.Self);
        if (index == 2)
            transform.Rotate(0, 0, (control.power + control.BD) * 3, Space.Self);
        if (index == 3)
            transform.Rotate(0, 0, (control.power + control.CD) * 3, Space.Self);
        if (index == 4)
            transform.Rotate(0, 0, (control.power + control.DD) * 3, Space.Self);
	}
}

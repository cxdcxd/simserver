using UnityEngine;
using System.Collections;
using System;


public class vib : MonoBehaviour 
{
    bool can = false;
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.3f);
        can = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (can == false)
        {
            can = true;
            StartCoroutine(wait());
            drop_box.VIB("100");
        }
    }
    

   
   
}
	


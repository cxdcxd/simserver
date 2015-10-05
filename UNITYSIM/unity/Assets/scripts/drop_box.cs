using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class drop_box : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    IEnumerator wait()
    {  
        gameObject.AddComponent<BoxCollider>();
        gameObject.AddComponent<Rigidbody>();
        gameObject.transform.parent = null;
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
      
    }

    public static void VIB(string time)
    {
           // Process P = new Process();
           // P.StartInfo.CreateNoWindow = true;
           // P.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
           // P.StartInfo.Arguments = time;
           // P.StartInfo.FileName = "C:\\xboxc.exe";
           // P.Start();

    }

	void Update () 
    {

        if ( Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.O))
        {

           
            StartCoroutine(wait());
            
            
        
        }

	
	}
}
 
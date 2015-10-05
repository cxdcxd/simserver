using UnityEngine;
using System.Collections;

public class force_subtest : MonoBehaviour
{

    float a = 0;
    public int index;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = new UnityEngine.Quaternion(0, 0, 0, 0);

    }

    void FixedUpdate()
    {
            if (index == 1)
            {
                a = control2.input_force;
            }
            if (index == 2)
            {
                a = -control2.input_force;
            }
          
            GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, a , 0 ), ForceMode.Force);
            a = 0;
        }
    }

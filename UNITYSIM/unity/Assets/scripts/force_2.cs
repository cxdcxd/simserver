using UnityEngine;
using System.Collections;

public class force_2 : MonoBehaviour
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
                 a = control.AR;
            }
            if (index == 2)
            {
                 a = control.BR;
                 a = 10;
            }

            if (index == 3)
            {
                a = control.CR;
            }

            if (index == 4)
            {
                a = control.DR;
                a = 10;
            }

            GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, a), ForceMode.Force);
            a = 0;
        }
    }

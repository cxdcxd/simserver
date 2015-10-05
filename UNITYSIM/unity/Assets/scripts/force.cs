using UnityEngine;
using System.Collections;

public class force : MonoBehaviour
{

       private float a;
    private float b;
    public int index;

    private void FixedUpdate()
    {
        if (this.index == 1)
        {
            this.a = control.AD + control.ADD;
            this.b = control.AR;
        }
        if (this.index == 2)
        {
            this.a = control.BD + control.BDD;
            this.b = control.BR;
        }
        if (this.index == 3)
        {
            this.a = control.CD + control.CDD;
            this.b = control.CR;
        }
        if (this.index == 4)
        {
            this.a = control.DD + control.DDD;
            this.b = control.DR;
        }
        base.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0f, (control.power * 2f) + this.a, 0f), ForceMode.Force);
        this.a = 0f;
    }

    private void Start()
    {
    }

    private void Update()
    {
        base.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
    }
    }

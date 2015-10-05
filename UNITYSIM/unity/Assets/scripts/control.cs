using UnityEngine;
using System.Collections;
using System;


public class control : MonoBehaviour
{
    public static float AD;
    public static float ADD;
    public static float AR;
    public static float BD;
    public static float BDD;
    public static float BR;
    public static float CD;
    public static float CDD;
    float control_speed = 4f;
    int control_speed2 = 20;
    public static float CR;
    public float[] curve_points = new float[300];
    public GUITexture curve_show;
    public Texture2D curve_text;
    public static float DD;
    public static float DDD;
    public static float DR;
    private bool enable_curve;
    private float g = 9.8f;
    public GUISkin gskin;
    private float H_SENSOR;
    private float H_TARGET = 20f;
    private int ic;
    float K = 0.1f;
    float k_d_h = 0.5f;
    float k_d_r = 1f;
    float k_i_h = 0.1f;
    float k_i_r;
    float k_p_h = 0.15f;
    float k_p_r = 0.1f;
    public Texture2D logo;
    private float M = 2f;
    float max_power = 70f;
    private PID pid = new PID(0f, 0f, 0f);
    private PID pid2 = new PID(0f, 0f, 0f);
    private PID pid3 = new PID(0f, 0f, 0f);
    public static float power = 25f;
    public Texture2D Q_text;
    public static float stable_power;
    private bool start;
    private float T;
    private float tf_1;
    private float tf_2;
    private float tf_3;
    private float XR_SENSOR;
    private float YR_SENSOR;
    private float ZR_SENSOR;


    void Start()
    {
        Color[] colors = new Color[0x8ca0];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].a = 0.6f;
        }
        this.curve_text.SetPixels(colors);
        this.curve_text.Apply();
    }

  
    void stable()
    {
        this.pid.updata_k(3,0, 0);
        //this.pid2.updata_k(this.k_p_r, 0, 0);
        //this.pid3.updata_k(this.k_p_r, 0, 0);

        float num = this.pid.Update(this.H_TARGET, this.H_SENSOR, Time.deltaTime);
        print("error : " + num);

        if (power < 20f)
        {
            power = 20f;
        }

        if (power > 30f)
        {
            power = 30f;
        }

        
        power = 28.5f + num / 10; 
       


       // float num2 = this.pid2.Update(0f, this.XR_SENSOR, Time.deltaTime);
       // float num3 = this.pid3.Update(0f, this.ZR_SENSOR, Time.deltaTime);
        //AD = -num2 / 30f;
        //CD = num2 / 30f;
        //BD = num3 / 30f;
        //DD = -num3 / 30f;

    }


    // Update is called once per frame
 
    void OnTriggerEnter(Collider other)
    {

    }

    Vector3 last_position = Vector3.zero;
    void Update()
    {


        if (Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.Z))
        {
            BR = this.control_speed2;
            DR = -this.control_speed2;
            AR = -10f;
            CR = 10f;
        }
        else if (Input.GetKey(KeyCode.JoystickButton5) || Input.GetKey(KeyCode.C))
        {
            AR = -this.control_speed2;
            CR = this.control_speed2;
            BR = 10f;
            DR = -10f;
        }
        else
        {
            AR = 0f;
            BR = 0f;
            CR = 0f;
            DR = 0f;
        }
        ADD = this.control_speed * -Input.GetAxis("Vertical");
        CDD = this.control_speed * Input.GetAxis("Vertical");
        BDD = this.control_speed * -Input.GetAxis("Horizontal");
        DDD = this.control_speed * Input.GetAxis("Horizontal");
        if (((Input.GetAxis("Vertical") <= 0.1f) && (Input.GetAxis("Vertical") >= -0.1f)) && ((Input.GetAxis("Horizontal") <= 0.1f) && (Input.GetAxis("Horizontal") >= -0.1f)))
        {
            ADD = 0f;
            BDD = 0f;
            CDD = 0f;
            DDD = 0f;
        }
        if (Input.GetKey(KeyCode.N))
        {
            this.H_TARGET += Time.deltaTime * 20f;
        }
        if (Input.GetKey(KeyCode.M))
        {
            this.H_TARGET -= Time.deltaTime * 20f;
        }
        this.H_TARGET = (float)Math.Round((double)this.H_TARGET, 2);
        if (this.H_TARGET < 0f)
        {
            this.H_TARGET = 0f;
        }
        if (this.H_TARGET > 50f)
        {
            this.H_TARGET = 50f;
        }
        if (power < 0f)
        {
            power = 0f;
        }
        if (power > this.max_power)
        {
            power = this.max_power;
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.E))
        {
            Application.LoadLevel(0);
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.P))
        {
            this.start = true;
        }
        if (this.start)
        {
            this.stable();
        }
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(((float)Screen.width) / 1024f, ((float)Screen.height) / 768f, 1f));
        GUI.skin = this.gskin;
        GUI.Label(new Rect(10f, 10f, 200f, 100f), "Total power :" + power.ToString());
        this.H_SENSOR = base.transform.position.y - 13.64f;
        this.H_SENSOR = (float)Math.Round((double)this.H_SENSOR, 2);
        this.ZR_SENSOR = (float)Math.Round((double)base.transform.rotation.eulerAngles.z, 2);
        this.YR_SENSOR = (float)Math.Round((double)base.transform.rotation.eulerAngles.y, 2);
        this.XR_SENSOR = (float)Math.Round((double)base.transform.rotation.eulerAngles.x, 2);
        if ((this.XR_SENSOR > 180f) && (this.XR_SENSOR <= 360f))
        {
            this.XR_SENSOR -= 360f;
        }
        if ((this.ZR_SENSOR > 180f) && (this.ZR_SENSOR <= 360f))
        {
            this.ZR_SENSOR -= 360f;
        }
        if ((this.YR_SENSOR > 180f) && (this.YR_SENSOR <= 360f))
        {
            this.YR_SENSOR -= 360f;
        }
        GUI.Label(new Rect(10f, 40f, 2000f, 100f), "[" + this.XR_SENSOR.ToString() + " , " + this.YR_SENSOR.ToString() + " , " + this.ZR_SENSOR.ToString() + "]");
        GUI.Label(new Rect(10f, 70f, 2000f, 100f), "[" + this.H_SENSOR.ToString() + "][" + this.H_TARGET.ToString() + "]");
        GUI.DrawTexture(new Rect(944f, 688f, 80f, 80f), this.logo);
        if (this.enable_curve)
        {
            GUI.DrawTexture(new Rect(0f, 678f, 225f, 90f), this.curve_text);
        }
        Matrix4x4 matrix = GUI.matrix;
        Vector2 pivotPoint = new Vector2(962f, 62f);
        GUIUtility.RotateAroundPivot(this.YR_SENSOR, pivotPoint);
        GUI.matrix = matrix;


    }




}



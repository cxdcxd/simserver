using UnityEngine;
using System.Collections;
using System;


public class control2 : MonoBehaviour 
{
    public static float input_force = 0;
    bool enable_curve = false;
    bool in_control = true;
    float max_power = 100;
    float H_TARGET = 20;

    public GUITexture curve_show;
    public Texture2D logo;
    public Texture2D curve_text;
    public Texture2D Q_text;

    public float[] curve_points = new float[300];
    public GUISkin gskin;
    public static float AD = 0;
    public static float BD = 0;
    public static float CD = 0;
    public static float DD = 0;

    public static float ADD = 0;
    public static float BDD = 0;
    public static float CDD = 0;
    public static float DDD = 0;

    public static float AR = 0;
    public static float BR = 0;
    public static float CR = 0;
    public static float DR = 0;

    public static float power = 7;
    public static float stable_power = 0;
	
    bool start = false;
    float H_SENSOR = 0;
    float XR_SENSOR = 0;
    float YR_SENSOR = 0;
    float ZR_SENSOR = 0;

    PID pid = new PID(1f, 0.1f, 0f);
    PID pid2 = new PID(1f, 0.01f, 1f);
    PID pid3 = new PID(1f, 0.01f, 1f);

    void Start () 
    {
        Color[] w_all = new Color[300 * 180];
        for (int i = 0; i < w_all.Length; i++)
        {
            w_all[i].a = 0.6f;
        }

        curve_text.SetPixels(w_all);

        curve_text.Apply();
       
	}

    int ic = 0;
    void update_curve()
    {
        Color[] w_all = new Color[300*180];
        for (int i = 0; i < w_all.Length; i++)
        {
            w_all[i].a = 0.6f;
        }

            curve_text.SetPixels(w_all);

            curve_text.Apply();

        for (int i = 0; i < 297; i++)
        {
            curve_points[i + 2] = curve_points[i + 3];
            curve_points[i + 1] = curve_points[i + 2];
            curve_points[i] = curve_points[i + 1];
           
        }

        for ( int i = 0 ; i < curve_text.width ; i++ )
        {
                curve_text.SetPixel(i, (int)curve_points[i]*2, Color.red);
        }
      
        curve_text.Apply();
        //curve_show.texture = curve_text;
        print("CCC");
       
    }

    bool wait_curve = false;
    IEnumerator wait_draw()
    {
        yield return new WaitForSeconds(0.01f);
        curve_points[299] = XR_SENSOR ;
        curve_points[298] = XR_SENSOR ;
        curve_points[297] = XR_SENSOR ;
        update_curve();
        wait_curve = false;
    }


		void Heave_stable()
		{
			float q = pid.Update(H_TARGET, H_SENSOR, Time.deltaTime);

			//power += q ;
		
			if (power < 7) power = 7;
			//if (power > max_power ) power = max_power;

		}

    void stable()
    {
			
			Heave_stable();

			return;
        //============================
        if (true )
        {

        float qx = pid2.Update(0, XR_SENSOR, Time.deltaTime);
        if (qx > 10) qx = 10;
        if (qx < -10) qx = -10;
        qx = qx * 10;

       
            if (qx > 0)
            {
                //back
                CD = qx / 100 * ( control_speed);
                DD = qx / 100 * ( control_speed);

            }
            if (qx < 0)
            {
                //front

                AD = -qx / 100 * ( control_speed);
                BD = -qx / 100 * ( control_speed);

            }

            //////////////////////

            float qz = pid3.Update(0, ZR_SENSOR, Time.deltaTime);
            if (qz > 10) qz = 10;
            if (qz < -10) qz = -10;
            qz = qz * 10;


            if (qz > 0)
            {
                //back
                AD = qz / 100 * ( control_speed);
                DD = qz / 100 * ( control_speed);

            }
            if (qz < 0)
            {
                //front

                CD = -qz / 100 * ( control_speed);
                BD = -qz / 100 * ( control_speed);

            }
        }


    }
	// Update is called once per frame
    public int control_speed = 10;
    public int control_speed2 = 20;

	void Update () 
    {

        if (wait_curve == false)
        {
            
            if (enable_curve)
            {
                wait_curve = true;
                StartCoroutine(wait_draw());
            }
        }
       // print(AD + " " + DD + " " + CD + " " + BD);

        if (Input.GetKey(KeyCode.JoystickButton4))
        {
            BR = control_speed2 ;
            DR = -control_speed2 ;
            AR = -10;
            CR = 10;
            

           
        }else
        if (Input.GetKey(KeyCode.JoystickButton5))
        {
            AR = -control_speed2 ;
            CR = control_speed2;
            BR = 10;
            DR = -10;
            
        }
        else
        {

            AR = -10;
            BR = 10;
            CR = 10;
            DR = -10;

        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
       
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            in_control = false;
            input_force = control_speed * Input.GetAxis("Horizontal");
        }
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            in_control = false;
            input_force = control_speed * Input.GetAxis("Horizontal");
        }
      
        if (Input.GetAxis("Vertical") <= 0.1f && Input.GetAxis("Vertical") >= -0.1f && Input.GetAxis("Horizontal") <= 0.1f && Input.GetAxis("Horizontal") >= -0.1f)
        {
            in_control = true;
            input_force = 0;
            
           
        }



				if (Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.Q))
            {
                Application.LoadLevel(0);
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                enable_curve = !enable_curve;
            }

						if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Space))
            {
							print("S");
                start = true;
            }
            if (start == true)
            {
                stable();
            }
          
    }

  void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3((float)Screen.width / 1024, (float)Screen.height / 768, 1));
        GUI.skin = gskin;
        GUI.Label(new Rect(10, 10, 200, 100), power.ToString());

        H_SENSOR = transform.position.y - 13.64f;
        H_SENSOR = (float)Math.Round(H_SENSOR, 2);

        ZR_SENSOR = (float)Math.Round(transform.rotation.eulerAngles.z, 2);
        YR_SENSOR = (float)Math.Round(transform.rotation.eulerAngles.y, 2);
        XR_SENSOR = (float)Math.Round(transform.rotation.eulerAngles.x, 2);


        if (XR_SENSOR > 180 && XR_SENSOR <= 360)
            XR_SENSOR -= 360;

        if (ZR_SENSOR > 180 && ZR_SENSOR <= 360)
            ZR_SENSOR -= 360;

        if (YR_SENSOR > 180 && YR_SENSOR <= 360)
            YR_SENSOR -= 360;

         XR_SENSOR = Math.Abs(XR_SENSOR);
        GUI.Label(new Rect(10, 40, 2000, 100), "[" + XR_SENSOR.ToString() + " , " + YR_SENSOR.ToString() + " , " +  ZR_SENSOR.ToString() + "]");
        GUI.Label(new Rect(10, 70, 2000, 100), "[" + input_force.ToString() + "]" );
        GUI.DrawTexture(new Rect(1024-80,688,80,80),logo);
       
        if ( enable_curve )
        GUI.DrawTexture(new Rect(0,768-90,225,90),curve_text);


        //Matrix4x4 matrixBackup = GUI.matrix;

        //Vector2 pivot = new Vector2(1024 - 125/2,  125/2);
        //GUIUtility.RotateAroundPivot(YR_SENSOR, pivot);

        //GUI.DrawTexture(new Rect(1024 - 125, 0, 125, 125), Q_text);
        

        //GUI.matrix = matrixBackup;

       

    }




}
	


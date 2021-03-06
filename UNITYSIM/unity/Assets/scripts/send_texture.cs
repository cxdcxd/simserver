﻿using UnityEngine;
using System.Collections;
using System;
using System.Text;
using UnityEngine.UI;
using System.IO;

public class send_texture : MonoBehaviour 
{
	// Use this for initialization
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    public int cam_code = 0;
    //==========================
    client_control client;

    public string ImageToBase64(byte[] imageBytes)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            // Convert byte[] to Base64 String
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }

	Texture2D screenShot;
RenderTexture rt;
    public void send_camera(int num)
    {
        Camera camera_target = null;

        if (num == 1)
            camera_target = camera1;
        if (num == 2)
            camera_target = camera2;
        if (num == 3)
            camera_target = camera3;

        
        camera_target.targetTexture = rt;
        camera_target.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 800, 600), 0, 0);
        camera_target.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors


        byte[] bytes = screenShot.EncodeToJPG();
        string string_image = ImageToBase64(bytes);
        

		if ( num == 1 )
		    client.write_send_cam1("CAM" + num + "," + string_image);
		if ( num == 2 )
			client.write_send_cam2("CAM" + num + "," + string_image);
		if ( num == 3 )
			client.write_send_cam3("CAM" + num + "," + string_image);

		//rt = null;
		//screenShot = null;
		//GC.Collect ();
		//GC.WaitForPendingFinalizers ();
        //==========================================

    }

   
	void Start () 
    {

        client = (client_control)GetComponent("client_control");
        rt = new RenderTexture(800, 600, 24);
        screenShot = new Texture2D(800, 600, TextureFormat.RGB24, false);


	}

    void mc_get_message(ClientEventArgs args)
    {
        print(args.message);
    }

    void mc_get_event(ClientEventArgs args)
    {
        print(args.message);
    }
	
	// Update is called once per frame
	void Update () 
    {
       
       

	}
}

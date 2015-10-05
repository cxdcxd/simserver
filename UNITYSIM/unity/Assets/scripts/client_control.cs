using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;


public class client_control : MonoBehaviour 
{
	public delegate void Del();
	public Del a;

	System.Timers.Timer cam1_timer;
    send_texture sendtexture;
    public GameObject quad;
	public GameObject Car;
	public AnimationClip anim_clip;

    control_cmd cmd;
    bool first_client = false;
    public static int connected = 0;

    public GUISkin englishskin;
    private string lastMessage;
    private bool isActive = false;
  
  
    bool reader_active = true;
    bool client_active = true;
    string log = "LGS\n";
    int line = 0;
    bool delayy = false;

    public string name2 = "CAR";
    public static TcpClient client;
	public static TcpClient client_cam1;
	public static TcpClient client_cam2;
	public static TcpClient client_cam3;

    string input_name = "";
    public  static string input_ip = "127.0.0.1";
    
	void call()
	{

	}

    void OnApplicationQuit()
    {
            //QUITING THE APPLICATION   
            client_active = false;
            client_control.connected = 0;
            if (client != null)
            {
                client.Close();
                client = null;
			    client_cam1 = null;
			    client_cam2 = null;
			    client_cam3 = null;
            }
    }

    IEnumerator login_wait()
    {
        yield return new WaitForSeconds(0.5f);


        if (name.Length < 10)
        {
            print("LOGIN SENDING DONE");
            write_send("LOGIN|" + name);
        }
        else
        {
            print("USER NAME IS LONG MAX IS 10 CHAR");
        }
    }



    bool in_login = false;
    
    public void manual_disconnect()
    {
        client_active = false;
        client_control.connected = 0;
        client.Close();
        client = null;
		client_cam1 = null;
		client_cam2 = null;
		client_cam3 = null;
        print("Disconnected...");
    }
	
	int counter = 0;
	int counter_connect = 0;
	void Update()
	{

		counter++;
		if (counter == 1)
		{
			sendtexture.send_camera (1);
		}
		if (counter == 2)
		{
			sendtexture.send_camera (2);
		}
		if (counter == 3)
		{
			sendtexture.send_camera (3);
		}
		if (counter == 4) {

            write_send("gps," + quad.transform.position.x + "," + quad.transform.position.z + "," + quad.transform.position.y + "," +
            quad.transform.eulerAngles.x + "," + quad.transform.eulerAngles.z + "," + quad.transform.eulerAngles.y);
            //print(quad.transform.eulerAngles.x + " " + quad.transform.eulerAngles.y + " " + quad.transform.eulerAngles.z);
			counter = 0;
		}
		counter_connect++;

		if (counter_connect > 60) {
			counter_connect = 0;
			if (connected == 0) {
				if (client == null) {
					button_connect ();
				}
			}
		}

	}
  
	void Start () 
    {
        input_ip = "127.0.0.1";
        input_name = "car";

        statics.read_object_list = new ArrayList();
        statics.read_object_byte_list = new ArrayList();
        StartCoroutine(fast_update());

        cmd = (control_cmd)quad.GetComponent("control_cmd");
        sendtexture = (send_texture)GetComponent("send_texture");

		a = new Del (call);

		Animation anim = (Animation)Car.GetComponent ("Animation");
		anim ["car"].speed = 1f;
		anim.Play ();



    }


  
    void read_recive(string msg)
    {
        print("TCP GET : " + msg);

        string[] command = msg.Split(',');
        if ( command[0] == "set_control")
        {
            float x = float.Parse(command[1]);
            float y = float.Parse(command[2]);
            float z = float.Parse(command[3]);
			float w = float.Parse(command[4]);
            cmd.cmd_x = x;
            cmd.cmd_y = y;
            cmd.cmd_z = z;
			cmd.cmd_w = w;
        }

		if ( command[0] == "play")
		{
			Animation anim = (Animation)Car.GetComponent ("Animation");
			anim.Play ();
		}

		if ( command[0] == "stop")
		{
			Animation anim = (Animation)Car.GetComponent ("Animation");
			anim.Stop();
		}

		if ( command[0] == "speed")
		{
			Animation anim = (Animation)Car.GetComponent ("Animation");
			float x = float.Parse(command[1]);
			anim["car"].speed = x;
		}

		if ( command[0] == "goto")
		{
			Animation anim = (Animation)Car.GetComponent ("Animation");
			float x = float.Parse(command[1]);
			anim["car"].time = x;

		}

       


      
    }

    public bool write_byte(byte[] buffer)
    {
        if (client == null) return false;
        try
        {

            NetworkStream clientStream = client.GetStream();
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
            //print("FLUSH");
            return true;
        }
        catch (System.Exception ex)
        {

            return false;
        }
    }
    
    public bool write_send(string msg)
    {
        msg = "%" + msg + "$";
        if (client == null) return false ;
        if (client_control.connected == 0) return false;
        try
        {
            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(msg);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
             //print("FLUSH pos");
            return true;
        }
        catch (System.Exception ex)
        {
          
            return false;
        }
       
    }

	public bool write_send_cam1(string msg)
	{
		msg = "%" + msg + "$";
		if (client_cam1 == null) return false ;
		if (client_control.connected == 0) return false;
		try
		{
			NetworkStream clientStream = client_cam1.GetStream();
			ASCIIEncoding encoder = new ASCIIEncoding();
			byte[] buffer = encoder.GetBytes(msg);
			clientStream.Write(buffer, 0, buffer.Length);
			clientStream.Flush();
			//print("FLUSH cam1");
			return true;
		}
		catch (System.Exception ex)
		{
			
			return false;
		}
		
	}

	public bool write_send_cam2(string msg)
	{
		msg = "%" + msg + "$";
		if (client_cam2 == null) return false ;
		if (client_control.connected == 0) return false;
		try
		{
			NetworkStream clientStream = client_cam2.GetStream();
			ASCIIEncoding encoder = new ASCIIEncoding();
			byte[] buffer = encoder.GetBytes(msg);
			clientStream.Write(buffer, 0, buffer.Length);
			clientStream.Flush();
			//print("FLUSH cam2");
			return true;
		}
		catch (System.Exception ex)
		{
			
			return false;
		}
		
	}

	public bool write_send_cam3(string msg)
	{
		msg = "%" + msg + "$";
		if (client_cam3 == null) return false ;
		if (client_control.connected == 0) return false;
		try
		{
			NetworkStream clientStream = client_cam3.GetStream();
			ASCIIEncoding encoder = new ASCIIEncoding();
			byte[] buffer = encoder.GetBytes(msg);
			clientStream.Write(buffer, 0, buffer.Length);
			clientStream.Flush();
		//	print("FLUSH cam3");
			return true;
		}
		catch (System.Exception ex)
		{
			
			return false;
		}
		
	}

    void button_connect()
    {
            try
            {

               
                client = new TcpClient();
                client.NoDelay = true;

			    client_cam1 = new TcpClient();
		     	client_cam2 = new TcpClient();
			    client_cam3 = new TcpClient();

                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(input_ip), int.Parse("4001"));
                client.Connect(serverEndPoint);
     			IPEndPoint serverEndPoint2 = new IPEndPoint(IPAddress.Parse(input_ip), int.Parse("4010"));
		    	client_cam1.Connect(serverEndPoint2);
				IPEndPoint serverEndPoint3 = new IPEndPoint(IPAddress.Parse(input_ip), int.Parse("4011"));
				client_cam2.Connect(serverEndPoint3);
				IPEndPoint serverEndPoint4 = new IPEndPoint(IPAddress.Parse(input_ip), int.Parse("4012"));
				client_cam3.Connect(serverEndPoint4);


            

                //START COMUNICATION THREAD FOR BLOCKING READ
                //////////////////

                Thread clientThread = new Thread(new ThreadStart(HandleClientComm));
                clientThread.Start();
                print("TCP CONNECTED DONE :)");
                client_control.connected = 1;
                client_active = true;
  
            }
            catch (System.Exception ex)
            {
                client = null;
                client_control.connected = 0;
                client_active = false;
                
                print("SERVER IS NOT ACTIVE...");

            }   

    }
	
    public void HandleClientComm()
    {  
           
            NetworkStream clientStream = client.GetStream();
            //*#*byte[] message = new byte[1030];
            byte[] message = new byte[1024];
            int bytesRead;

            while (true)
            {

                bytesRead = 0;

                try
                {
                    //blocks until a server sends a message
                    //BLOCKING READ
                    bytesRead = clientStream.Read(message, 0, message.Length);

                    //print((float)bytesRead / 1000);

                }
                catch (Exception e)
                {
                    //socket error has occured
                    //print("ERROR : " + e.Message + " " + e.Data + " " + e.StackTrace);
                    print(e.GetBaseException());
                    break;
                }

                if (bytesRead == 0)
                {
                    //Client is disconnected from server
                    //print("ERROR : READ = 0");
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                string read_str = encoder.GetString(message, 0, bytesRead);

                read_object ro = new read_object(read_str, bytesRead.ToString());
                statics.read_object_list.Add(ro);


            }

            client_control.connected = 0;
            client.Close();
            client = null;

            //ADD server message to static buffer
            read_object ro2 = new read_object("Disconnected...", "0");
            statics.read_object_list.Add(ro2);

    }

   
    IEnumerator fast_update()
    {
        while (true)
        {
            if (statics.read_object_list != null && statics.read_object_list.Count != 0)
            {

                while (statics.read_object_list.Count != 0)
                {
                    read_object ro = (read_object)statics.read_object_list[0];
                    read_recive(ro.message);
                    statics.read_object_list.RemoveAt(0);

                }
            }

            yield return new WaitForSeconds(0.001f);
        }

    }
	
}


using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using UnityEngine;

/////////////////////////////////////////////////////////////////////////////////////////////////////
// programed by edwin babayans                                                                     //
// for more info contact me                                                                        //
// edwinteen@yahoo.com                                                                             //
// 2012                                                                                            //
// Emenu Project                                                                                   //
/////////////////////////////////////////////////////////////////////////////////////////////////////
class read_object
{

    public string message;
    public string bytes;
  
    public read_object(string msg, string bytes_count)
    {
        bytes = bytes_count;
        message = msg;
    }

}

class read_object_byte
{

    public byte[] message;
    public int bytes;
  

    public read_object_byte(byte[] msg, int bytes_count)
    {
        bytes = bytes_count;
        message = msg;
    }

}

public class player
{
    public Texture2D image;
    public int score;
    public int number;
    public string name;
    public int enable = 1;
    public int lamp_mode = 0;

    public player(int number)
    {
        this.number = number;
      
    }



}
    class statics
    {



        
       

        public static int max_chop_size = 1024;
        
        public static ArrayList read_object_list;
        public static ArrayList read_object_byte_list;
        public static string FPS;

        public static ArrayList img_list;
        public static ArrayList imgrate_list;
        public static ArrayList string_list = new ArrayList();

        
        

    }


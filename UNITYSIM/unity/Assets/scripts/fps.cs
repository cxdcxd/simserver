using UnityEngine;
using System.Collections;

/////////////////////////////////////////////////////////////////////////////////////////////////////
// programed by edwin babayans                                                                     //
// for more info contact me                                                                        //
// edwinteen@yahoo.com                                                                             //
// 2012                                                                                            //
// Emenu Project                                                                                   //
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class fps : MonoBehaviour
{

  
    
    public float updateInterval = 1F;
    public bool static_frame_rate = false;
    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
    public Rect fps_rec = new Rect(10, 10, 100, 100);

    public GUISkin skin;

    void Start()
    {

        PlayerPrefs.SetString("cxdbook", "0912-6211659");

      
        timeleft = updateInterval;
    }
    void OnGUI()
    {
        GUI.skin = skin;
     
     //  GUI.Label(fps_rec, statics.FPS);
    }
    void Update()
    {
       
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            string format = System.String.Format("{0:F2}", fps);
            //statics.FPS = format;

            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}

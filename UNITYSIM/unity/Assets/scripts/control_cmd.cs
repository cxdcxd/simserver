using UnityEngine;
using System.Collections;

public class control_cmd : MonoBehaviour {

    public GUITexture curve_show;
    public Texture2D logo;
    public GUISkin gskin;

    public static float power = 25;
    public  float cmd_x = 0;
    public  float cmd_y = 0;
    public  float cmd_z = 0;
   float z_noise = 0;
   float x_noise = 0;
   float y_noise = 0;
    float alpha = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        z_noise = Mathf.Sin(alpha * Mathf.Deg2Rad) * 0.01f;
        x_noise = Mathf.Sin((alpha + 90) * Mathf.Deg2Rad) * 0.02f;
        y_noise = Mathf.Sin((alpha + 180) * Mathf.Deg2Rad) * 0.02f;

        this.transform.position = new Vector3(this.transform.position.x + cmd_x / 10 + x_noise, this.transform.position.y + cmd_z / 10 + z_noise, this.transform.position.z + cmd_y / 10 + y_noise);
        this.transform.rotation = Quaternion.Euler(cmd_y * 1.5f, 0, -cmd_x * 1.5f);

        alpha += 0.5f;
        if (alpha > 360) alpha = 0;

        //print(z_noise);
	}

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(((float)Screen.width) / 1024f, ((float)Screen.height) / 768f, 1f));
        GUI.skin = this.gskin;
        GUI.Label(new Rect(10f, 10f, 200f, 100f), "Total power :" + power.ToString());

    }
}

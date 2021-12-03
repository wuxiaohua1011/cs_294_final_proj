using System;
using System.Collections;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class UDP_receiver_with_gui : MonoBehaviour
{
    public GameObject obj_to_render;
    public GameObject steeringWheel;
    public GameObject indicator_rpm;
    public GameObject indicator_speed;

    public float throttle = 0;
    public float steering = 0;
    public String host = "127.0.0.1";
    public int port = 8009;

    VehicleStatemStreamer vehicle_state_streamer;
    RGBStreamer rgb_streamer; 
    // Start is called before the first frame update
    void Start()
    {
       /* vehicle_state_streamer = new VehicleStatemStreamer("192.168.1.11", 8003, 100);
        vehicle_state_streamer.Start();*/


        rgb_streamer = new RGBStreamer("192.168.1.11", 8001, 100);
        rgb_streamer.Start();

    }



    void Update()
    {
        parse_oculus_control();
        updateImage();

        float turning_angle = ExtensionMethods.Remap(steering, -1, 1, -160, 160);
        steeringWheel.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -turning_angle);

        // rpm 0 ==> z=30 | 8 ==> z=-210
        float rpm_angle = ExtensionMethods.Remap(Math.Abs(throttle), 0, 1, 30, -205);
        indicator_rpm.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rpm_angle);
    }
    void updateImage()
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(this.rgb_streamer.getRGBData());
        obj_to_render.GetComponent<Renderer>().material.mainTexture = tex;
    }
    void parse_oculus_control()
    {
        OVRInput.Update();

        float left_trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        float right_trigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        float right_joystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;

        throttle = left_trigger * 1 + right_trigger * -1;
        steering = right_joystick;
    }


    private void OnApplicationQuit()
    {
        /*vehicle_state_streamer.Stop();*/
        rgb_streamer.Stop();
    }

}
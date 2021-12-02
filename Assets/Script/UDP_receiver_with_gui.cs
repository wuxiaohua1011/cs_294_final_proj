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

    UDPStreamer udp_streamer;
    // Start is called before the first frame update
    void Start()
    {
        udp_streamer = new UDPStreamer("127.0.0.1", 20001);
        udp_streamer.Start();
    }

    

    void Update()
    {
        print(udp_streamer.GetLatestPacket());
        parse_oculus_control();

        float turning_angle = ExtensionMethods.Remap(steering, -1, 1, -160, 160);
        steeringWheel.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -turning_angle);


        // rpm 0 ==> z=30 | 8 ==> z=-210

        float rpm_angle = ExtensionMethods.Remap(Math.Abs(throttle), 0, 1, 30, -205);
        indicator_rpm.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rpm_angle);
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
        udp_streamer.Stop();
    }

}
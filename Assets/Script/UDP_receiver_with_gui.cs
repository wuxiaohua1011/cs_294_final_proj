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
    /*public String host = "10.0.0.29";*/

    public float MAX_FORWARD_THROTTLE = 1;
    public float MAX_REVERSE_THROTTLE = -1;
    public float MAX_STEERING = 1;
    public float STEERING_OFFSET = 0;


    private Texture2D tex;


    VehicleStatemStreamer vehicle_state_streamer;
    RGBStreamer rgb_streamer;
    ControlStreamer control_streamer;
    // Start is called before the first frame update
    void Start()
    {
        tex = new Texture2D(2, 2);
        vehicle_state_streamer = new VehicleStatemStreamer(host, 8003, 100);
        vehicle_state_streamer.Start();

        rgb_streamer = new RGBStreamer(host, 8001, 100);
        rgb_streamer.Start();

        control_streamer = new ControlStreamer(host, 8004, 100); //significant latency when interval is lower than 500
        control_streamer.Start();

    }



    void Update()
    {
        ParseOculusControl();
        UpdateImage();
        RegularizeControl();
        control_streamer.UpdateControl(throttle, steering);

        float turning_angle = ExtensionMethods.Remap(steering, -1, 1, -160, 160);
        steeringWheel.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -turning_angle);

        // rpm 0 ==> z=30 | 8 ==> z=-210
        float rpm_angle = ExtensionMethods.Remap(Math.Abs(throttle), 0, 1, 30, -205);
        indicator_rpm.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rpm_angle);

        float speed = vehicle_state_streamer.vehicleState.getSpeed();
        float speed_angle = ExtensionMethods.Remap(Math.Abs(speed), 0, 3, 30, -205);
        indicator_speed.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -20, speed_angle);
    }

    void RegularizeControl()
    {
        this.throttle = Math.Max(Math.Min(this.MAX_FORWARD_THROTTLE, this.throttle), this.MAX_REVERSE_THROTTLE);
        this.steering = Math.Max(Math.Min(this.MAX_STEERING, this.steering + this.STEERING_OFFSET), -this.MAX_STEERING);
      
    }

    void UpdateImage()
    {
        tex.LoadImage(this.rgb_streamer.getRGBData());
        obj_to_render.GetComponent<Renderer>().material.mainTexture = tex;
    }
    void ParseOculusControl()
    {
        OVRInput.Update();
        /*Debug.Log("rotation" + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) + "," + OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles);*/
        /*Debug.Log(OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles[2]);*/
        float controller_rotation_angle = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles[2];
        if (controller_rotation_angle <= 180)
        {
            steering = -controller_rotation_angle / 180.0f;
        }
        else
        {
            steering = -(controller_rotation_angle - 360) / 180.0f; ;
        }
        float left_trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        float right_trigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        float right_joystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;

        throttle = left_trigger * -1 + right_trigger * 1;
        /*steering = right_joystick;*/
    }


    private void OnApplicationQuit()
    {
        vehicle_state_streamer.Stop();
        rgb_streamer.Stop();
        control_streamer.Stop();
    }

}
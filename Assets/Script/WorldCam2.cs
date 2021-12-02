using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;

public class WorldCam2 : MonoBehaviour
{
    WebSocket websocket;
    public GameObject obj_to_render;
    public GameObject steeringWheel;
    public GameObject indicator_rpm;
    public GameObject indicator_speed;

    public float throttle = 0;
    public float steering = 0;
    public String host = "127.0.0.1";
    public int port = 8009;

    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket($"ws://{host}:{port}");
        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            // getting the message as a string
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            obj_to_render.GetComponent<Renderer>().material.mainTexture = tex;
            /*var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);*/
        };

        // Keep sending messages at every 0.1s
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.1f);

        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {

        parse_oculus_control();

        #if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
        #endif

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

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending plain text
            await websocket.SendText($"{throttle},{steering}");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}
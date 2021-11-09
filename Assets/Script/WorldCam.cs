using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using UnityEngine.InputSystem;


public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}

public class WorldCam : MonoBehaviour
{
    WebSocket websocket;
    GameObject obj_to_render;
    GameObject steeringWheel;

    public float throttle = 0;
    public float steering = 0;
    public String host = "127.0.0.1";
    public int port = 8009;
    private void OnThrottle(InputValue value)
    {
        throttle = value.Get<float>();
    }
    private void OnSteering(InputValue value)
    {
        steering = value.Get<Vector2>().x;

    }

    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket($"ws://{host}:{port}");
        obj_to_render = GameObject.Find("glass");
        steeringWheel = GameObject.Find("steering-wheel");
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
            print("frame parsed");
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
    #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
    #endif

    float turning_angle = ExtensionMethods.Remap(steering, -1, 1, -160, 160);
   // print(turning_angle * Time.deltaTime);
    steeringWheel.transform.rotation =  Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -turning_angle );



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
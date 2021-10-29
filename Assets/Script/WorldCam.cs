using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using UnityEngine.InputSystem;

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
            Texture2D tex = new Texture2D(480, 640);
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
    #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
        steeringWheel.transform.Rotate(new Vector3(0, 0, -steering));

    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending plain text
            await websocket.SendText($"({throttle},{steering})");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}
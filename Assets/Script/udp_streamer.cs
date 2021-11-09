using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class udp_streamer : MonoBehaviour
{
    byte[] udata;
    UdpClient newsock;
    IPEndPoint sender;
    // Start is called before the first frame update
    void Start()
    {
        udata = new byte[1024];
        newsock = new UdpClient("127.0.0.1", 9050);

        Debug.Log("Waiting for a client...");

        sender = new IPEndPoint(IPAddress.Any, 0);
    }

    // Update is called once per frame
    void Update()
    {
        print("here");
        udata = newsock.Receive(ref sender);

        Debug.Log("Message received from {0}:" + sender.ToString());
        /*Debug.Log(Encoding.ASCII.GetString(udata, 0, udata.Length));

        var welcome : String = "Welcome to my test server";
        udata = Encoding.ASCII.GetBytes(welcome);
        newsock.Send(udata, udata.Length, sender);*/
    }
}

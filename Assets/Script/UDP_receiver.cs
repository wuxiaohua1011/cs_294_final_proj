using System;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using System.Timers;
using Debug = UnityEngine.Debug;

public class UDPStreamer
{
    string address;
    int port;
    long interval; // milisecond
    int timeout;
    Thread readThread;
    UdpClient client;
    System.Timers.Timer timer;
    AutoResetEvent autoEvent = new AutoResetEvent(false);

    public string lastReceivedPacket = "";
    public string allReceivedPackets = ""; // this one has to be cleaned up from time to time

    public UDPStreamer(string address, int port, long interval = 500, int timeout = 1000)
    {
        this.address = address;
        this.port = port;
        this.interval = interval;
        this.timeout = timeout;

        this.InitializeClient();
    }
    public void Start()
    {
        timer = new System.Timers.Timer(500);
        timer.Elapsed += this.ReceiveData;
        timer.AutoReset = true;
        timer.Enabled = true;

    }

    private void InitializeClient()
    {
        client = new UdpClient(this.address, this.port);
        client.Client.Blocking = true;
        client.Client.ReceiveTimeout = this.timeout;

    }

    public void ReceiveData(System.Object source, ElapsedEventArgs e)
    {
        this.InitializeClient();
        try
        {
            Byte[] sendBytes = Encoding.ASCII.GetBytes("ack");

            client.Send(sendBytes, sendBytes.Length);

            // receive bytes
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = client.Receive(ref anyIP);
            // encode UTF8-coded bytes to text format
            string text = Encoding.UTF8.GetString(data);
            lastReceivedPacket = text;
            // update received messages
            /* allReceivedPackets = allReceivedPackets + text;*/
        }
        catch (Exception err)
        {
            Debug.Log(err);
        }
    }

    public string GetLatestPacket()
    {
        allReceivedPackets = "";
        return lastReceivedPacket;
    }

    // Stop reading UDP messages
    public void Stop()
    {
        timer.Stop();
        client.Close();
    }
}

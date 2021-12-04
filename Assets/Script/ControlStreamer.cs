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

public class ControlStreamer
{
    protected string address;
    protected int port;
    protected long interval; // milisecond
    protected int timeout;
    protected Thread readThread;
    protected UdpClient client;
    protected System.Timers.Timer timer;

    private float throttle = 0;
    private float steering = 0;

    private byte[] received_data; // this one has to be cleaned up from time to time

    public ControlStreamer(string address, int port, long interval = 500, int timeout = 1000)
    {
        this.address = address;
        this.port = port;
        this.interval = interval;
        this.timeout = timeout;

        this.InitializeClient();
    }
    public void Start()
    {
        timer = new System.Timers.Timer(this.interval); // milisecond
        timer.Elapsed += this.SendData;
        timer.AutoReset = true;
        timer.Enabled = true;

    }
    private void SendData(System.Object source, ElapsedEventArgs e)
    {
        Byte[] sendBytes = Encoding.UTF8.GetBytes(this.throttle + "," + this.steering);
        client.Send(sendBytes, sendBytes.Length);
    }
    protected void InitializeClient()
    {
        client = new UdpClient(this.address, this.port);
        client.Client.Blocking = true;
        client.Client.ReceiveTimeout = this.timeout;
    }

    // Stop reading UDP messages
    public void Stop()
    {
        Byte[] sendBytes = Encoding.UTF8.GetBytes(0 + "," + 0);
        for (int i = 0; i < 10; i ++)
        {
            client.Send(sendBytes, sendBytes.Length);
        }
        timer.Stop();
        client.Close();
    }

    public void UpdateControl(float throttle, float steering)
    {
        this.throttle = throttle;
        this.steering = steering;
    }
}
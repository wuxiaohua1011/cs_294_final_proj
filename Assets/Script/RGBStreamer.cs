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

public class RGBStreamer : UDPStreamer
{
    private byte[] rgb_data; 
    public RGBStreamer(string address, int port, long interval=500, int timeout=1000) :
        base(address, port, interval, timeout) { }

    public override void ReceiveData(object source, ElapsedEventArgs e)
    {
        base.ReceiveData(source, e);
        this.rgb_data = new byte[this.GetLatestData().Length - 16];
        Array.Copy(this.GetLatestData(), 16, this.rgb_data, 0, this.rgb_data.Length);
    }

    public byte[] getRGBData()
    {
        return rgb_data;
    }
}

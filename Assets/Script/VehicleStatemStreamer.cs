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

public class VehicleStatemStreamer : UDPStreamer
{

    public VehicleState vehicleState = new VehicleState();

    public VehicleStatemStreamer(string address, int port, long interval = 500, int timeout = 1000) : 
        base(address, port, interval, timeout) { }

    public override void ReceiveData(System.Object source, ElapsedEventArgs e)
    {
        base.ReceiveData(source, e);
        string msg = Encoding.UTF8.GetString(this.GetLatestData()); // parse the data to string
        string[] data = msg.Split(',');
        vehicleState.x = float.Parse(data[0]);
        vehicleState.y = float.Parse(data[1]);
        vehicleState.z = float.Parse(data[2]);
        vehicleState.roll = float.Parse(data[3]);
        vehicleState.pitch = float.Parse(data[4]);
        vehicleState.yaw = float.Parse(data[5]);
        vehicleState.vx = float.Parse(data[6]);
        vehicleState.vy = float.Parse(data[7]);
        vehicleState.vz = float.Parse(data[8]);
        vehicleState.gx = float.Parse(data[9]);
        vehicleState.gy = float.Parse(data[10]);
        vehicleState.gz = float.Parse(data[11]);
        vehicleState.received_time = float.Parse(data[12]);
    }

}

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
        vehicleState.y = float.Parse(data[0]);
        vehicleState.z = float.Parse(data[0]);
        vehicleState.roll = float.Parse(data[0]);
        vehicleState.pitch = float.Parse(data[0]);
        vehicleState.yaw = float.Parse(data[0]);
        vehicleState.vx = float.Parse(data[0]);
        vehicleState.vy = float.Parse(data[0]);
        vehicleState.vz = float.Parse(data[0]);
        vehicleState.gx = float.Parse(data[0]);
        vehicleState.gy = float.Parse(data[0]);
        vehicleState.gz = float.Parse(data[0]);
        vehicleState.received_time = float.Parse(data[0]);


        Debug.Log(this.vehicleState.ToString());
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleState
{
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public float roll = 0;
    public float pitch = 0;
    public float yaw = 0;
    public float vx = 0;
    public float vy = 0;
    public float vz = 0;
    public float ax = 0;
    public float ay = 0;
    public float az = 0;
    public float gx = 0;
    public float gy = 0;
    public float gz = 0;
    public float received_time = 0; 
    public VehicleState()
    {

    }

    public override string ToString()
    {
        return x + ", " + y + ", " + z + ", " +
               roll + ", " + pitch + "," + yaw + ", " +
               vx + ", " + vy + ", " + vz + ", " + ax + ", " + ay + ", " + az + ", " +
               gx + ", " + gy + ", " + gz + ", " + received_time;
    }
}

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
    protected string address;
    protected int port;
    protected long interval; // milisecond
    protected int timeout;
    protected Thread readThread;
    protected UdpClient client;
    protected System.Timers.Timer timer;
    private bool should_continue = true;
    private byte[] received_data; // this one has to be cleaned up from time to time

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
        timer = new System.Timers.Timer(this.interval); // milisecond
        timer.Elapsed += this.ReceiveData;
        timer.AutoReset = true;
        timer.Enabled = true;

    }

    protected void InitializeClient()
    {
        client = new UdpClient(this.address, this.port);
        client.Client.Blocking = true;
        client.Client.ReceiveTimeout = this.timeout;
    }

    public virtual void ReceiveData(System.Object source, ElapsedEventArgs e)
    {
        if (this.should_continue)
        {
            this.InitializeClient();
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                var buffer = new List<byte>();

                // send ack to let the other end know where I am
                Byte[] sendBytes = Encoding.ASCII.GetBytes("ack");
                client.Send(sendBytes, sendBytes.Length);
                bool has_received_full = false;
                while (has_received_full == false)
                {
                    byte[] raw_data = client.Receive(ref anyIP); // receive data

                    // decode meta data
                    byte[] meta_data = new byte[9];
                    Array.Copy(raw_data, 0, meta_data, 0, meta_data.Length);
                    string text = Encoding.UTF8.GetString(meta_data);
                    int prefix_num = int.Parse(text.Substring(0, 3));
                    int total_num = int.Parse(text.Substring(3, 3));


                    byte[] data = new byte[raw_data.Length - 9];
                    Array.Copy(raw_data, 9, data, 0, data.Length);
                    buffer.AddRange(data);
                    if (prefix_num == total_num)
                    {
                        received_data = buffer.ToArray();
                        buffer.Clear();
                        has_received_full = true;
                        break;
                    }
                }
            }
            catch (Exception err)
            {
                Debug.Log(err);
            }
        }
    }

    public byte[] GetLatestData()
    {
        return received_data;
    }

    // Stop reading UDP messages
    public void Stop()
    {
        this.should_continue = false;
        timer.Enabled = false;
        timer.Stop();
        timer.Dispose();
        client.Dispose();
        client.Close();
        
    }
}

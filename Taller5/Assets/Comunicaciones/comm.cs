using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System.Threading;
using System.IO.Ports;
using System.Runtime.ConstrainedExecution;

public class comm : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cube;
    public Vector3 transformacion;

    //Thread server;
    Thread ser;
    SerialPort serial;

    void Start()
    {
        cube = GameObject.Find("Cube");
        //server = new Thread(new ThreadStart(StartServer));
        //server.Start();
        try
        {
            serial = new SerialPort("COM8", 115200);
            if (serial != null)
            {
                ser = new Thread(new ThreadStart(StartSerial));
                ser.Start();
            }
        }
        catch (Exception e)
        {
            print("No serial port found");
            print(e.ToString());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (cube != null)
        {
            if (!transformacion.Equals(Vector3.zero))
            {
                print("transformando");
                cube.transform.Rotate(transformacion.x, transformacion.y, transformacion.z);
                transformacion.x = 0;
                transformacion.y = 0;
                transformacion.z = 0;
            }
        }
    }

    private void OnApplicationQuit()
    {
        print("Fin");
        ser.Abort();
        serial.Close();
  
       // server.Abort();
    }

    public void StartSerial()
    {
        byte data,Hx,Lx,Hy,Ly;
        serial.ReadTimeout = -1;
        try
        {
            serial.Open();
            print("abierto");
            while (true)
            {
                if (serial.IsOpen == true)
                {
                    do {
                        data = (byte)serial.ReadByte();
                    } while (data != 0x0a);
                    Hx = (byte)serial.ReadByte();
                    Lx = (byte)serial.ReadByte();
                    byte err = (byte)serial.ReadByte();
                    Hy = (byte)serial.ReadByte();
                    Ly = (byte)serial.ReadByte();
                    err = (byte)serial.ReadByte();

                    int X = (Hx<<8) + Lx;
                    int Y = (Hy<<8) + Ly;
                    int X2 = Hx * 256 + Lx;
                    int Y2 = Hy * 256 + Ly;

                    //0...1024  mas o menos 512
                    print(X);
                    print(Y);
                   if (X < 450)
                    {
                        transformacion.x = -10;
                        //transformacion.Set(-10, 0, 0);
                    } 
                   if (X> 600)
                    {
                        //transformacion.Set(10, 0, 0);
                        transformacion.x = 10;
                    }

                    if (Y < 450)
                    {
                        //transformacion.Set(0, -10, 0);
                        transformacion.y = -10;
                    }
                    if (Y > 600)
                    {
                        //transformacion.Set(0, +10, 0);
                        transformacion.y= 10;
                    }
                    /*
                   
                    if (data.Length >= 1)
                    {
                        if (data.StartsWith("X"))
                        {
                            transformacion.Set(10, 0, 0);
                        }
                        if (data.StartsWith("Y"))
                        {
                            transformacion.Set(0, 10, 0);
                        }
                        if (data.StartsWith("Z"))
                        {
                            transformacion.Set(0, 0, 10);
                        }
                    }*/
                }
            }
        }
        catch
        {
            print("error");
        }
    }

    public void StartServer()
    {
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress;
        ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 12345);
        try
        {

            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(1);
            print("esperando conexion");
            Socket handler = listener.Accept();
            print("conectado");
            string data = null;
            while (true)
            {

                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                print("dato recibido");

                if (data != null)
                {
                    if (data.Contains("X"))
                    {
                        transformacion.Set(10, 0, 0);
                    }
                    if (data.Contains("Y"))
                    {
                        transformacion.Set(0, 10, 0);
                    }
                    if (data.Contains("Z"))
                    {
                        transformacion.Set(0, 0, 10);
                    }
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    if (data.Length == 0)
                    {
                        break;
                    }
                    if (data.Length == 0 || data.Contains("FFF"))
                    {
                        break;
                    }
                }
            }
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }
}

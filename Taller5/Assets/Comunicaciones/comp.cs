using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.IO.Ports;
using System.Collections.Concurrent;

public class comp : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cube;
    public Vector3 transformacion;
    public Vector3 escala;
    public Vector3 desplazamiento;
    public Vector3 destino;
    Thread server;
    Thread ser;

    SerialPort serial;

    void Start()
    {
        cube = GameObject.Find("Cube");
        server = new Thread(new ThreadStart(StartServer));
        server.Start();
    }

    /*       
           try
           {
               serial = new SerialPort("COM5", 115200);
               if (serial != null)
               {
                   ser = new Thread(new ThreadStart(StartSerial));
                   ser.Start();
               }
           } catch (Exception e)
           {
               print("No serial port found");
               print(e.ToString());
           }

       }
    */
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
    /*
    if (!escala.Equals(Vector3.zero)) {

        Vector3 vector = cube.transform.localScale;
        vector = Vector3.Scale(vector, escala);
        escala = Vector3.zero;
        cube.transform.localScale = vector;
    }

    if (!desplazamiento.Equals(Vector3.zero))
    {
        cube.transform.Translate(desplazamiento * Time.deltaTime);
        desplazamiento = Vector3.zero;
    }
}
}
    */
    private void OnApplicationQuit()
    {
        print("Fin");
        server.Abort();
    }

    /*
            serial.Close();
            ser.Abort();
        }
    */

    /*
    public void StartSerial()
    {
        string data;
        serial.ReadTimeout = -1;
        try
        {
            serial.Open();
            while (true)
            {
                if (serial.IsOpen == true)
                {
                    data = serial.ReadLine();
                    if (data.Length > 2)
                    {
                        if (data.StartsWith("X"))
                        {
                            float move = float.Parse(data.Substring(1, 4));
                            move = move - 2047;
                            move = move / 1000;
                            desplazamiento.Set(move, 0, 0);
                        }
                        if (data.StartsWith("Y"))
                        {
                            float move = float.Parse(data.Substring(1, 4));
                            move = move - 2047;
                            move = move / 1000;
                            desplazamiento.Set(0, move, 0);
                        }
                        if (data.StartsWith("Z"))
                        {
                            float move = float.Parse(data.Substring(1, 4));
                            move = move - 2047;
                            move = move / 1000;
                            desplazamiento.Set(0, 0, move);
                        }

                    }
                }
            }
        } catch
        {
            print("error");
        }
    }

    */

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
    /*
                        if (data.Contains("xTO"))
                        {
                            float mov = float.Parse(data.Substring(3, 2));
                            destino.Set(mov, 0, 0);
                        }
                        if (data.Contains("yTO"))
                        {
                            float mov = float.Parse(data.Substring(3, 2));
                            destino.Set(0, mov, 0);
                        }
                        if (data.Contains("zTO"))
                        {
                            float mov = float.Parse(data.Substring(3, 2));
                            destino.Set(0, mov, 0);
                        }
                        if (data.Contains("+"))
                        {
                            escala.Set(2, 2, 2);
                        }
                        if (data.Contains("-"))
                        {
                            escala.Set(0.5f, 0.5f, 0.5f);
                        }
                    } else
                    {
                        break;
                    }*/


}
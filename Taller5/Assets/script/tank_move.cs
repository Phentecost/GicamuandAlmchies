using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class tank_move : MonoBehaviour
{


    SerialPort serial = new SerialPort("COM3", 115200);

    public float duration; 
    bool isMoving = false;
    private static readonly ConcurrentQueue<string> cola_datos= new ConcurrentQueue<string>();

    GameObject turret;

    float angulo;

    Thread ser;

    // Start is called before the first frame update
    void Start()
    {
        turret = GameObject.Find("PivotCan");
        ser = new Thread(new ThreadStart(StartSerial));
        ser.Start();
    }

    // Update is called once per frame
    void Update()
    {

        //delay(50);
        string dato;


        if (isMoving == false)
        {
            if (cola_datos.TryDequeue(out dato))
            {
                if (dato == "W")
                {
                    isMoving = true;
                    StartCoroutine(MoveFront());
                    duration = 0.1f;
                }
                if (dato == "S")
                {
                    isMoving = true;
                    StartCoroutine(MoveBack());
                    duration = 1.0f;
                }
                if (dato == "D")
                {
                    isMoving = true;
                    StartCoroutine(Rotateleft());
                    duration = 1.0f;
                }
                if (dato == "A")
                {
                    isMoving = true;
                    StartCoroutine(Rotateright());
                    duration = 1.0f;
                }
                if (dato == "Q")
                {
                    isMoving = true;
                    StartCoroutine(Turretup());
                    duration = 1.0f;
                }
                if (dato == "E")
                {
                    isMoving = true;
                    StartCoroutine(Turretdown());
                    duration = 1.0f;
                }
                if (dato == "C")
                {
                    isMoving = true;
                    StartCoroutine(Turretleft());
                    duration = 1.0f;
                }
                if (dato == "Z")
                {
                    isMoving = true;
                    StartCoroutine(Turretright());
                    duration = 1.0f;
                }

            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            cola_datos.Enqueue("W");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            cola_datos.Enqueue("S");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            cola_datos.Enqueue("D");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            cola_datos.Enqueue("A");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cola_datos.Enqueue("Q");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            cola_datos.Enqueue("E");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            cola_datos.Enqueue("Z");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            cola_datos.Enqueue("C");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (cola_datos.TryDequeue(out string any)) ;
        }

    }
    private void OnApplicationQuit()
    {
        serial.Close();
    }


    IEnumerator MoveFront()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Translate(Vector3.right * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator MoveBack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Translate(Vector3.left * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator Rotateleft()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 90);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator Rotateright()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * 90);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator Turretup()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (turret.transform.localRotation.eulerAngles.z >= 0.0f)
            {
                turret.transform.Rotate(Vector3.back * Time.deltaTime * 15);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator Turretdown()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (turret.transform.localRotation.eulerAngles.z <= 90.0f)
            {
                turret.transform.Rotate(Vector3.forward * Time.deltaTime * 15);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator Turretleft()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (turret.transform.localRotation.eulerAngles.y <= 270.0f)
            {
                turret.transform.Rotate(Vector3.right * Time.deltaTime * 15);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    IEnumerator Turretright()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (turret.transform.localRotation.eulerAngles.y >= 90.0f)
            {
                turret.transform.Rotate(Vector3.left * Time.deltaTime * 15);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    public void StartSerial()
    {
        byte data, XmaxMov, XminMov, YmaxMov, YminMov, turret;
        serial.ReadTimeout = -1;
        try
        {
            serial.Open();
            print("serial abierto");
            while (true)
            {
                if (serial.IsOpen == true)
                {
                    do
                    {
                        data = (byte)serial.ReadByte();
                    } while (data != 0x0a);
                    //packet[0] --> X
                    //packet[1] --> Y
                    //packet[2] -->pot
                    XmaxMov = (byte)serial.ReadByte();
                    XminMov = (byte)serial.ReadByte();
                    byte err = (byte)serial.ReadByte();
                    YmaxMov = (byte)serial.ReadByte();
                    YminMov = (byte)serial.ReadByte();
                    err = (byte)serial.ReadByte();
                    turret = (byte)serial.ReadByte();

                    int X = (XmaxMov << 8) + XminMov;
                    int Y = (YmaxMov << 8) + YminMov;

                    angulo = 10 * (turret - 512) / 1024;

                    if (turret == 1 && X > 1000)
                        cola_datos.Enqueue("W");
                    if (turret == 1 && X < 20)
                        cola_datos.Enqueue("S");
                    if (turret == 1 && Y > 1000)
                        cola_datos.Enqueue("D");
                    if (turret == 1 && Y < 20)
                        cola_datos.Enqueue("A");
                    if (turret == 0 && X > 1000)
                        cola_datos.Enqueue("E");
                    if (turret == 0 && X < 20)
                        cola_datos.Enqueue("Q");
                    if (turret == 0 && Y > 1000)
                        cola_datos.Enqueue("C");
                    if (turret == 0 && Y < 20)
                        cola_datos.Enqueue("Z");


                    //if (data.Length > 2)
                    //{
                    //    /*
                    //    if (data.Contains("T"))
                    //    {
                    //        cola_datos.Enqueue("E");
                    //    }
                    //    if (data.Contains("Y"))
                    //    {
                    //        cola_datos.Enqueue("Q");
                    //    }
                    //    */
                    //    print(data);

                    //}
                }
            }
        }
        catch
        {
            print("error");
        }
    }

}

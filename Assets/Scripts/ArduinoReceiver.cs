using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using Microsoft.Win32;
using System.Text.RegularExpressions;
public class ArduinoReceiver : MonoBehaviour
{
    static SerialPort stream;

    // SELECT YOUR COM PORT AND BAUDRATE
    public string port = "COM8";
    int baudrate = 9600;
    int readTimeout = 10;

    private string[] dataRaw;
    private string[] datafinal;
    public static float xaxis;
    public static float zaxis;
    public static float referencevalue;


    public bool eshtablished = false;
    public string dataString;


    //  public int PortsPastLength = 0;

    void Start()
    {
        //  port = PlayerPrefs.GetString("portName","COM3");

        //DetectPort();
        connectionEstablish();

        //setreferencevalue();
    }

   private void setreferencevalue()
   {
        if (eshtablished)
        {
            dataString = "null received";


            if (stream.IsOpen)
            {
                try
                {
                    dataString = stream.ReadLine();
                    //Debug.Log("data String : " + dataString);
                }
                catch (System.Exception ioe)
                {
                    // Debug.Log("IOException: " + ioe.Message);
                }

            }
            else
            {
                dataString = "NOT OPEN";
                eshtablished = false;
            }

            if (!dataString.Equals("NOT OPEN"))
            {
                if (!dataString.Equals("null received"))
                {
                    char splitChar = '|';
                    dataRaw = dataString.Split(splitChar);
                    datafinal = dataRaw[2].Split('=');
                    referencevalue = float.Parse(datafinal[1]);
                    referencevalue = (referencevalue / 1023) * 2 - 1;
                }
            }
        }
   }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
           // DetectPort();
            connectionEstablish();
        }

        if (port == null)
            return;

        if (eshtablished)
        {
            dataString = "null received";


            if (stream.IsOpen)
            {
                //Debug.Log("WTF");
                try
                {
                    dataString = stream.ReadLine();
                    Debug.Log("data String : " + dataString);
                }
                catch (System.Exception ioe)
                {
                    // Debug.Log("IOException: " + ioe.Message);
                }

            }
            else
            {
                dataString = "NOT OPEN";
                eshtablished = false;
            }

            if (!dataString.Equals("NOT OPEN"))
            {
                if (!dataString.Equals("null received"))
                {
                    char splitChar = ',';
                    dataRaw = dataString.Split(splitChar);
                    if (dataRaw.Length == 2 && dataRaw[0] != "")
                    {
                        xaxis = float.Parse(dataRaw[0]);
                        zaxis = float.Parse(dataRaw[1]);
                    }
                }
            }


            //stream.Close();
        }
    }

    void DetectPort()
    {
        //List<string> names = ComPortNames("2341", "0043"); // UNO CODE _ arduino
        List<string> names = ComPortNames("1A86", "7523");  // MEGA CODE_arduino
        if (names.Count > 0)
        {
            foreach (String s in SerialPort.GetPortNames())
            {
                if (names.Contains(s))
                {
                    Console.WriteLine("My Arduino port is " + s);
                    port = s;
                }
            }
        }
        else
            Console.WriteLine("No COM ports found");
    }

    void connectionEstablish()
    {
        if (stream != null)
        {
            stream.Close(); stream.Dispose();
        }

        stream = new SerialPort("\\\\.\\" + port, baudrate);

        try
        {
            stream.ReadTimeout = readTimeout;
        }

        catch (System.IO.IOException ioe)
        {
            Debug.Log("IOException: " + ioe.Message);
        }
        try
        {
            stream.Open();

        }
        catch (System.Exception e)
        {
            Debug.Log("IOExc: " + e.Message);
        }

        finally
        {
            eshtablished = true;
            // port = portTemp;
        }
    }

    //map changes the range of a1,a2 to b1,b1
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return (s - a1) * (b2 - b1) / (a2 - a1) + b1;
    }

    public void ResetHardware()
    {
        DetectPort();
        connectionEstablish();
    }

    List<string> ComPortNames(string VID, string PID)
    {
        string pattern = string.Format("^VID_{0}.PID_{1}", VID, PID);
        Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
        List<string> comports = new List<string>();
        RegistryKey rk1 = Registry.LocalMachine;
        RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
        foreach (string s3 in rk2.GetSubKeyNames())
        {
            RegistryKey rk3 = rk2.OpenSubKey(s3);
            foreach (string s in rk3.GetSubKeyNames())
            {
                if (_rx.Match(s).Success)
                {
                    RegistryKey rk4 = rk3.OpenSubKey(s);
                    foreach (string s2 in rk4.GetSubKeyNames())
                    {
                        RegistryKey rk5 = rk4.OpenSubKey(s2);
                        RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                        comports.Add((string)rk6.GetValue("PortName"));
                    }
                }
            }
        }
        return comports;
    }
}


#region OldCode


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO.Ports;

//public class ArduinoReceiver : MonoBehaviour
//{
//    static SerialPort stream;

//    // SELECT YOUR COM PORT AND BAUDRATE
//    public string port = "COM3";
//    int baudrate = 57600;
//    int readTimeout = 10;

//    private string[] dataRaw;
//    public static float axis1;
//    public static float axis2;
//    public static float axis3;
//    public static float axis4;

//    public float _axis1;

//    public static float BoptSteering;
//    public static int StopUp = +1;
//    public static int StopDown = +1;
//    public static int PadL = 0;
//    public static int PadR = 0;
//    public static int PalletLock = 0;

//    public static int direction = +1;  // 0 represents backward, 1 represents forward
//    public static int horn = 1;         // 0 represents horn, 1 represents no horn
//    public static int engine = 1;       // 0 represents on, 1 represents off

//    public static bool lever3 = true;
//    public bool eshtablished = false;
//    public string dataString;



//    void Start()
//    {
//      //  port = PlayerPrefs.GetString("portName","COM3");
//         connectionEstablish();
//    }

//    void Update()
//    {
//        if(Input.GetKeyDown(KeyCode.R))
//        {
//            connectionEstablish();
//        }

//        //if (SerialPort.GetPortNames().Length != PortsPastLength)
//        //{
//        //    port = null;

//        //    try
//        //    {
//        //        stream.Close();
//        //    }
//        //    catch (System.Exception e)
//        //    {
//        //        Debug.Log("IOExc: " + e.Message);
//        //    }
//        //    eshtablished = false;
//        //    connectionEstablish();
//        //}

//        //if (port == null && SerialPort.GetPortNames().Length > 0)
//        //{

//        //    port = SerialPort.GetPortNames()[0]; connectionEstablish();
//        //}


//        if (port == null)
//            return;

//        // Debug.Log(SerialPort.GetPortNames()[0] + "   " + SerialPort.GetPortNames().Length);
//        if (eshtablished)
//        {
//             dataString = "null received";

//            if (stream.IsOpen)
//            {
//                try
//                {
//                    dataString = stream.ReadLine();
//                    // Debug.Log("RCV_ : " + dataString);
//                }
//                catch (System.Exception ioe)
//                {
//                    //  Debug.Log("IOException: " + ioe.Message);
//                }

//            }
//            else
//            {
//                dataString = "NOT OPEN";
//                eshtablished = false;
//            }

//            if (!dataString.Equals("NOT OPEN"))
//            {
//                char splitChar = ',';
//                dataRaw = dataString.Split(splitChar);
//                if (dataRaw.Length == 7 && dataRaw[0] != "")
//                {
//                    axis1 = (float.Parse(dataRaw[0]) / 1023) * 2 - 1;

//                    _axis1 =(float.Parse(dataRaw[0]) / 1023) * 2 - 1;

//                    axis2 = (float.Parse(dataRaw[1]) / 1023) * 2 - 1;
//                    axis3 = (float.Parse(dataRaw[2]) / 1023) * 2 - 1;
//                    axis4 = (float.Parse(dataRaw[3]) / 1023) * 2 - 1;

//                    direction = int.Parse(dataRaw[4]);
//                    horn = int.Parse(dataRaw[5]);
//                    engine = int.Parse(dataRaw[6]);


//                    BoptSteering = float.Parse(dataRaw[0]);
//                    StopUp = int.Parse(dataRaw[1]);
//                    StopDown = int.Parse(dataRaw[2]);
//                    PadL = int.Parse(dataRaw[3]);
//                    PalletLock = int.Parse(dataRaw[4]);
//                    PadR = int.Parse(dataRaw[5]);
//                }
//            }

//            try
//            {
//                stream.BaseStream.FlushAsync();
//            }
//            catch (System.Exception ioe)
//            {
//                Debug.Log("IOException: " + ioe.Message);
//            }

//            //stream.Close();
//        }
//    }

//    void connectionEstablish()
//    {
//        if (stream != null)
//        {
//            stream.Close(); stream.Dispose();
//        }

//            stream = new SerialPort("\\\\.\\" + port, baudrate);

//            try
//            {
//                stream.ReadTimeout = readTimeout;
//            }

//            catch (System.IO.IOException ioe)
//            {
//                Debug.Log("IOException: " + ioe.Message);
//            }
//            try
//            {
//                stream.Open();
//            }
//            catch (System.Exception e)
//            {
//                Debug.Log("IOExc: " + e.Message); 
//            }

//            finally
//            {             
//                eshtablished = true;
//               // port = portTemp;
//            }         


///*
//        foreach ( string portTemp in SerialPort.GetPortNames())
//        {
//            // open port. Be sure in unity edit > project settings > player is NET2.0 and not NET2.0Subset
//            stream = new SerialPort("\\\\.\\" + portTemp, baudrate);

//            try
//            {
//                stream.ReadTimeout = readTimeout;
//            }

//            catch (System.IO.IOException ioe)
//            {
//                Debug.Log("IOException: " + ioe.Message);
//            }
//            try
//            {
//                stream.Open();
//            }
//            catch (System.Exception e)
//            {
//                Debug.Log("IOExc: " + e.Message); 
//            }

//            finally
//            {             
//                eshtablished = true;
//                port = portTemp;
//            }         

//        }
//    */
//}

//    //map changes the range of a1,a2 to b1,b1
//    float map(float s, float a1, float a2, float b1, float b2)
//    {
//        return (s - a1) * (b2 - b1) / (a2 - a1) + b1;
//    }

//    public void ResetHardware()
//    {
//        stream.Close();
//        connectionEstablish();
//    }
//}




#endregion
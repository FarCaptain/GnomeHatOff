using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using Microsoft.Win32;
using System.Text.RegularExpressions;
public class ArduinoReceiver : MonoBehaviour
{
    public MainGameController Game;
    static List<SerialPort> streams = new List<SerialPort>();
     
    
    int baudrate = 9600;
    int readTimeout = 10;
    [Header("SELECT YOUR COM PORT AND BAUDRATE")]
    public string[] ports;
    private int playerAmount = 0;
    private string[] dataRaw;
    private string[] datafinal;
    

    private bool established = false;
    private string dataString;       // string output of arduino transmission

    //  public int PortsPastLength = 0;

    void Start()
    {
        //  port = PlayerPrefs.GetString("portName","COM3");
        playerAmount = ports.Length;
        //DetectPort();
        for(int i=0; i < playerAmount; i++)
        {
            connectionEstablish(i);
        }
        established = true;

    }

    private void OnDestroy()
    {
        for(int i =0; i < playerAmount; i++)
        {
            streams[i].Close();
            streams[i].Dispose();
        }
        
    }

    /// <summary>
    /// Ensures Serial Ports are connected
    /// </summary>
    void connectionEstablish(int playerIndex)
    {

        SerialPort stream = new SerialPort("\\\\.\\" + ports[playerIndex], baudrate);
        streams.Add(stream);
        try
        {
            stream.ReadTimeout = readTimeout;
        }

        catch (System.IO.IOException ioe)
        {
            Debug.Log(playerIndex+":IOException: " + ioe.Message);
        }
        try
        {
            stream.Open();

        }
        catch (System.Exception e)
        {
            Debug.Log(playerIndex + ":IOExc: " + e.Message);
        }

    }

    void Update()
    {


        if (established)
        {
            dataString = "null received";
            for(int i=0; i < playerAmount; i++)
            {
                if (streams[i].IsOpen)
                {
                    try
                    {
                        dataString = streams[i].ReadLine();
                        char splitChar = ',';
                        dataRaw = dataString.Split(splitChar);
                        if (dataRaw.Length == 2 && dataRaw[0] != "")
                        {
                            float xaxis = float.Parse(dataRaw[1]);
                            float zaxis = float.Parse(dataRaw[2]);
                            Game.RecieveSignal(i, xaxis, 0, zaxis);
                        }
                    }
                    catch (System.Exception ioe)
                    {
                        Debug.Log("IOException: " + ioe.Message);
                    }
                }
                else
                {
                    dataString = "NOT OPEN";
                    established = false;
                }
            }
        }
    }
}

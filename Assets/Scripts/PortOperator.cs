using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Management;
using System;

public class PortOperator : MonoBehaviour
{
    static List<SerialPort> streams = new List<SerialPort>();
    private string[] ports;
    private List<string> arduinoPorts = new List<string>();
    private int baudrate = 9600;

    public GameObject gnomePurple;
    public GameObject gnomeRed;
    public GameObject gnomeYellow;
    public GameObject gnomeBlue;

    // Start is called before the first frame update
    void Start()
    {
        ports = SerialPort.GetPortNames();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO. dynamically update the connection state
        for (int i = 0; i < ports.Length; i++)
        {
            if (ports[i] == "Connected")
                continue;

            if (connectionEstablish(i))
            {
                string dataString = "null received";
                try
                {
                    dataString = streams[i].ReadLine();

                    if (dataString != "null received")
                        print("");

                    char splitChar = ',';
                    string[] dataRaw = dataString.Split(splitChar);
                    if (dataRaw.Length == 3 && dataRaw[0] != "")
                    {
                        arduinoPorts.Add(ports[i]);
                        print("IIIII" + ports[i]);

                        streams[i].Close();
                        streams[i].Dispose();
                        ports[i] = "Connected";

                        //TODO. Needs the value from the arduino to identify the hat
                        // Now just doing it in some order
                        distributeCharacter(i);
                    }

                    // and then we need to give the COM numbers to Ardity
                }
                catch (System.Exception ioe)
                {
                    Debug.Log("IOException: " + ioe.Message);
                }
            }
        }
    }

    void distributeCharacter(int temp)
    {
        Vector3 pos = new Vector3(0f, 20f, 0f);
        GameObject gnomePrefab;

        switch(temp)
        {
            case 0:
                gnomePrefab = gnomePurple;
                break;
            case 1:
                gnomePrefab = gnomeRed;
                break;
            case 2:
                gnomePrefab = gnomeYellow;
                break;
            default:
                gnomePrefab = gnomeBlue;
                break;
        }
        GameObject gnome = Instantiate(gnomePrefab, pos, Quaternion.identity);
    }

    bool connectionEstablish(int portIndex)
    {

        SerialPort stream = new SerialPort("\\\\.\\" + ports[portIndex], baudrate);
        streams.Add(stream);
        try
        {
            stream.ReadTimeout = 10;
        }

        catch (System.IO.IOException ioe)
        {
            Debug.Log(portIndex + ":IOException: " + ioe.Message);
            return false;
        }
        try
        {
            stream.Open();
        }
        catch (System.Exception e)
        {
            Debug.Log(portIndex + ":IOExc: " + e.Message);
            return false;
        }

        return true;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Management;
using System;

public class PortManager : MonoBehaviour
{
    static List<SerialPort> streams = new List<SerialPort>();
    private string[] ports;
    private List<string> connectedPorts;

    private List<string> arduinoPorts = new List<string>();
    private int baudrate = 9600;

    private NewTimer connectComTimer;
    public MainGameController gameController;

    public GameObject gnomePurple;
    public GameObject gnomeRed;
    public GameObject gnomeYellow;
    public GameObject gnomeBlue;

    public GameObject arduino;

    // Start is called before the first frame update
    void Start()
    {
        ports = SerialPort.GetPortNames();
        connectComTimer = gameObject.AddComponent<NewTimer>();
        connectComTimer.MaxTime = 10f;
        connectComTimer.TimerStart = true;

        getConnectedPorts();

        gameController = GetComponent<MainGameController>();
        gameController.Arduino = arduino;
        gameController.COM.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO. dynamically update the connection state
        //if (connectComTimer.TimerStart)
        //{
        for (int i = 0; i < streams.Count; i++)
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
                    gameController.COM.Add(streams[i].PortName);

                    //streams[i].Close();
                    //streams[i].Dispose();
                    //TODO. Needs the value from the arduino to identify the hat
                    // Now just doing it in some order
                    distributeCharacter(i);

                    streams.RemoveAt(i--);
                }

                // and then we need to give the COM numbers to Ardity
            }
            catch (System.Exception ioe)
            {
                Debug.Log("IOException: " + ioe.Message);
            }
        }
    }

    void getConnectedPorts()
    {
        for (int i = 0; i < ports.Length; i++)
        {
            SerialPort port = connectionEstablish(i);
            if (port != null)
            {
                streams.Add(port);
            }
        }
    }

    void distributeCharacter(int temp)
    {
        // TODO. uses the respawn effect
        Vector3 pos = new Vector3(0f, 1f, 0f);
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
        gameController.players.Add(gnome);
        gameController.RegisterPlayerController(gameController.players.Count - 1);

    }

    SerialPort connectionEstablish(int portIndex)
    {
        SerialPort stream = new SerialPort(ports[portIndex], baudrate);
        try
        {
            stream.ReadTimeout = 10;
        }

        catch (System.IO.IOException ioe)
        {
            Debug.Log(portIndex + ":IOException: " + ioe.Message);
            return null;
        }
        try
        {
            stream.Open();
        }
        catch (System.Exception e)
        {
            Debug.Log(portIndex + ":IOExc: " + e.Message);
            return null;
        }

        return stream;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
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

    public VisualEffect poof;

    // Start is called before the first frame update
    void Start()
    {
        ports = SerialPort.GetPortNames();
        connectComTimer = gameObject.AddComponent<NewTimer>();
        connectComTimer.MaxTime = 10f;
        connectComTimer.TimerStart = true;

        getConnectedPorts();

        gameController = GameObject.Find("GameManager").GetComponent<MainGameController>();
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
        Vector3 pos = gnomePrefab.transform.position;
        poof.transform.position = pos;
        poof.Play();
        gnomePrefab.SetActive(true);

        gameController.players.Add(gnomePrefab);
        gameController.RegisterPlayerController(gameController.players.Count - 1);

        //Destroy(poofEffect);

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

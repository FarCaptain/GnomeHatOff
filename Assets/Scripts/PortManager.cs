using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.IO.Ports;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.Management;
using System;

public class PortManager : MonoBehaviour
{
    static List<SerialPort> streams = new List<SerialPort>();
    private string[] ports;
    private List<string> connectedPorts;

    private List<string> arduinoPorts = new List<string>();
    private int baudrate = 9600;
    private int maxPlayerCount = 0;

    private GameObject gnomeSpawners;

    public MainGameController gameController;

    public GameObject gnomeRedPrefab;
    public GameObject gnomeYellowPrefab;
    public GameObject gnomeBluePrefab;
    public GameObject gnomePurplePrefab;

    //public GameObject arduino;

    public VisualEffect poofPrefab;

    // Start is called before the first frame update
    void Start()
    {
        BluetoothClient client = new BluetoothClient();

        ports = SerialPort.GetPortNames();

        getConnectedPorts();

        //gameController = GameObject.Find("GameManager").GetComponent<MainGameController>();
        gameController = MainGameController.instance;
        //gameController.Arduino = arduino;
        gameController.COM.Clear();

        // all connected devices
        var devices = client.DiscoverDevices();
        foreach (BluetoothDeviceInfo device in devices)
        {
            if (device.DeviceName == "BLUEHAT" || device.DeviceName == "YELLOWHAT" || device.DeviceName == "GREENHAT" ||
                (device.DeviceName == "HC-05" && device.DeviceAddress == BluetoothAddress.Parse("98D371FDB05D")))
                maxPlayerCount++;
            print(device.DeviceName + "::");
        }

        gnomeSpawners = GameObject.Find("GnomeSpawners");
    }

    // Update is called once per frame
    void Update()
    {
        // TODO. dynamically update the connection state
        //if (connectComTimer.TimerStart)
        //{
        if (gameController.COM.Count < maxPlayerCount)
        {
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
                        distributeCharacter(dataRaw[0]);

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

    void distributeCharacter(string temp)
    {
        // TODO. uses the respawn effect
        GameObject gnomePrefab;

        switch(temp)
        {
            case "G":
                gnomePrefab = gnomePurplePrefab;
                gnomePurplePrefab = null;
                break;
            case "R":
                gnomePrefab = gnomeRedPrefab;
                gnomeRedPrefab = null;
                break;
            case "Y":
                gnomePrefab = gnomeYellowPrefab;
                gnomeYellowPrefab = null;
                break;
            default:
                gnomePrefab = gnomeBluePrefab;
                gnomeBluePrefab = null;
                break;
        }

        if (gnomePrefab == null) return;
        
        Vector3 pos = gnomeSpawners.transform.GetChild(gameController.players.Count).position;
        pos.y = 0f;
        VisualEffect poo = Instantiate(poofPrefab, pos, Quaternion.identity);
        poo.Play();
        //gnomePrefab.SetActive(true);

        gameController.players.Add(Instantiate(gnomePrefab, pos, Quaternion.identity));
        DontDestroyOnLoad(gameController.players[gameController.players.Count - 1].transform.gameObject);
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

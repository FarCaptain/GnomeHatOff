/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * When creating your message listeners you need to implement these two methods:
 *  - OnMessageArrived
 *  - OnConnectionEvent
 */

public class SampleMessageListener : MonoBehaviour
{
    public MainGameController Game;
    private string[] dataRaw;
    public int Playerindex;
    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        //Debug.Log("Message arrived: " + msg);
       
        char splitChar = ',';
        dataRaw = msg.Split(splitChar);
        if (dataRaw.Length == 3 && dataRaw[0] != " ")
        {
            float xaxis = float.Parse(dataRaw[0]);
            float zaxis = float.Parse(dataRaw[1]);
            string jump = dataRaw[2];
            Game.RecieveSignal(Playerindex, xaxis, 0, zaxis, jump);
            Debug.Log("Player 1: " + xaxis + "," + zaxis);
        }
        if (dataRaw.Length == 2 && dataRaw[0] != " ")
        {
            float xaxis = float.Parse(dataRaw[0]);
            float zaxis = float.Parse(dataRaw[1]);
            //string jump = dataRaw[2];
            Game.RecieveSignal(Playerindex, xaxis, 0, zaxis, "True");
            Debug.Log("Player 2: " + xaxis + "," + zaxis);
        }
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }
}

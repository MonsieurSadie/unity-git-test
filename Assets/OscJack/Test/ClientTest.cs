// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using System.Collections;
using OscJack;

class ClientTest : MonoBehaviour
{
  OscClient client;
    void Start()
    {
        // IP address, port number
        client = new OscClient("127.0.0.1", 9000);
        InvokeRepeating("Send", 1, 1);
    }


    void Send()
    {
      client.Send("/test",       // OSC address
                        Time.time,     // First element
                        Random.value); // Second element
    }

    void OnDestroy()
    {
      client.Dispose();
    }
}

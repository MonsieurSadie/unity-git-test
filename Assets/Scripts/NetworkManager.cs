// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using OSC.NET;

using Console = RuntimeTools.Console;

class NetworkManager : MonoBehaviour
{

  public int clientID = 0;
  public string targetIp = "255.255.255.255";
  public int port = 9000;

  string localIpAdress = "";

  static OSCTransmitter sender;
  static OSCReceiverAsync receiver;

  void Start()
  {
    localIpAdress = GetLocalIPAddress();

    sender = new OSCTransmitter(targetIp, port);
    receiver = new OSCReceiverAsync(port);

    sender.Connect();
    receiver.Connect();
    receiver.Start(OnReceivedMessage);
  }

  public static string GetLocalIPAddress()
  {
      var host = Dns.GetHostEntry(Dns.GetHostName());
      foreach (var ip in host.AddressList)
      {
          if (ip.AddressFamily == AddressFamily.InterNetwork)
          {
              return ip.ToString();
          }
      }
      return "";
  }

  
  void LogOscMessage(OSCPacket msg)
  {
    string s = "";
    // message sender ip is stored in last message values
    s += "received message " + msg.Address + " from " + msg.Values[msg.Values.Count-1] + " with content";
    foreach (var item in msg.Values)
    {
      s += " " + item.ToString();
    }
    Console.Log(s);
  }



  void OnReceivedMessage(OSCPacket msg)
  {
    LogOscMessage(msg);
    switch(msg.Address)
    {
      case "/whosthere":
        string msgSenderIp = msg.Values[0].ToString();
      
        OSCMessage message = new OSCMessage("/here", GetLocalIPAddress());
        SendMessageTo(msgSenderIp, message);
      break;

      case "/here":
      break;
    }
  }


#region UI
  public void PingButton()
  {
    PingForPlayers();
  }
#endregion


#region SEND
  public void SendMessageTo(string ip, OSCMessage msg)
  {
    OSCTransmitter senderTo = new OSCTransmitter(ip, port);
    senderTo.Send(msg);
  }

  public void BroadcastMessage(OSCMessage msg)
  {
    msg.Append(localIpAdress);
    sender.Send(msg);
  }

  public void PingForPlayers()
  {
    Console.Log("[Me] ping ther players");
    OSCMessage message = new OSCMessage("/whosthere", localIpAdress);
    sender.Send(message);
  }

#endregion


}

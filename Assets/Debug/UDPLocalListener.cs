using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Tests
{
    public class UDPLocalListener : MonoBehaviour
    {
        [SerializeField]
        ScriptableString serverIpAdress;

        int port = 9995;

        int messageCount = 0;

        void Start()
        {
            UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);

            Debug.Log("Waiting for broadcast");
            byte[] bytes = listener.Receive(ref groupEP);//Lock execution but there is a Async alternative

            messageCount++;

            Debug.Log(Encoding.ASCII.GetString(bytes, 0, bytes.Length) + " | MessageCount : " + messageCount);

            listener.Close();
        }
    }
}

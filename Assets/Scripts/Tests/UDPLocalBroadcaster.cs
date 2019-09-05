using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace UdeS.Promoscience.Tests
{

    public class UDPLocalBroadcaster : MonoBehaviour
    {
        int port = 9995;

        float timer = 0;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= 2)
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress broadcastAddress = IPAddress.Parse("192.168.0.255");//SocketOptionName.Broadcast is equivalent to 255.255.255.255

                byte[] message = Encoding.ASCII.GetBytes("Hello");
                IPEndPoint endPoint = new IPEndPoint(broadcastAddress, port);

                socket.SendTo(message, endPoint);

                timer = 0;
            }
        }

    }
}

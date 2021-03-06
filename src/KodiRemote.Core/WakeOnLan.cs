﻿using System;
using System.Net;
using System.Net.Sockets;
#if SILVERLIGHT
using Microsoft.Phone.Net.NetworkInformation;
#endif

namespace KodiRemote.Core
{
    public static class WakeOnLan
    {
        public static void WakeUp(this Connection cnx)
        {
            byte[] datagram = new byte[102];

            for (int i = 0; i <= 5; i++)
            {
                datagram[i] = 0xff;
            }

            string macAddress = cnx.MacAddress;
            string[] macDigits = macAddress.Split(macAddress.Contains("-") ? '-' : ':');
            if (macDigits.Length != 6)
                throw new ArgumentException("Incorrect MAC address supplied!");

            const int start = 6;
            for (int i = 0; i < 16; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                }
            }

            Send(datagram);
        }

        private static void Send(byte[] payload)
        {
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs
            {
                RemoteEndPoint = new IPEndPoint(IPAddress.Broadcast, 7)
            };

            socketEventArg.Completed += SocketEventArg_Completed;

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
#if SILVERLIGHT
            socket.SetNetworkRequirement(NetworkSelectionCharacteristics.NonCellular);
#endif
            socketEventArg.SetBuffer(payload, 0, payload.Length);
            socket.ConnectAsync(socketEventArg);
        }

        private static void SocketEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket socket = sender as Socket;
            socket?.Dispose();
        }
    }
}

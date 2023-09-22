﻿using H00N.Network;
using Packets;
using System;

namespace TestClient
{
    public class PacketManager
    {
        private static PacketManager instance = null;
        public static PacketManager Instance {
            get {
                if (instance == null)
                    instance = new PacketManager();

                return instance;
            }
        }

        private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new Dictionary<ushort, Func<ArraySegment<byte>, Packet>>();
        private Dictionary<ushort, Action<Packet>> packetHandlers = new Dictionary<ushort, Action<Packet>>();

        private PacketManager()
        {
            packetFactories.Clear();
            packetHandlers.Clear();

            RegisterHandler();
        }

        private void RegisterHandler()
        {
            packetFactories.Add((ushort)PacketID.S_ChatPacket, PacketUtility.CreatePacket<S_ChatPacket>);
            packetHandlers.Add((ushort)PacketID.S_ChatPacket, PacketHandler.S_ChatPacketHandler);
        }

        public void HandlePacket(ArraySegment<byte> buffer)
        {
            ushort packetID = PacketUtility.ReadPacketID(buffer);

            if (packetFactories.ContainsKey(packetID))
            {
                Packet packet = packetFactories[packetID]?.Invoke(buffer);
                if (packetHandlers.ContainsKey(packetID))
                    packetHandlers[packetID]?.Invoke(packet);
            }
        }
    }
}

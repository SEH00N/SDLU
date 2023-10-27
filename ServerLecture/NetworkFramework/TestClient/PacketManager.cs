using H00N.Network;
using Packets;
using System;
using System.Collections.Generic;

namespace TestClient
{
    public class PacketManager
    {
        // 싱글톤패턴
        private static PacketManager instance = null;
        public static PacketManager Instance {
            get {
                if (instance == null)
                    instance = new PacketManager();

                return instance;
            }
        }
        
        private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new Dictionary<ushort, Func<ArraySegment<byte>, Packet>>(); // 패킷 생성자
        private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new Dictionary<ushort, Action<Session, Packet>>(); // 패킷 핸들러

        private PacketManager()
        {
            // 전역변수 초기화
            packetFactories.Clear();
            packetHandlers.Clear();

            RegisterHandler(); // 핸들러 구독
        }

        private void RegisterHandler() // 핸들러 구독
        {
            packetFactories.Add((ushort)PacketID.S_ChatPacket, PacketUtility.CreatePacket<S_ChatPacket>);
            packetHandlers.Add((ushort)PacketID.S_ChatPacket, PacketHandler.S_ChatPacket);

            packetFactories.Add((ushort)PacketID.S_EnterPacket, PacketUtility.CreatePacket<S_EnterPacket>);
            packetHandlers.Add((ushort)PacketID.S_EnterPacket, PacketHandler.S_EnterPacket);

            packetFactories.Add((ushort)PacketID.S_LeavePacket, PacketUtility.CreatePacket<S_LeavePacket>);
            packetHandlers.Add((ushort)PacketID.S_LeavePacket, PacketHandler.S_LeavePacket);
        }

        public void HandlePacket(Session session, ArraySegment<byte> buffer) // 패킷을 핸들링
        {
            ushort packetID = PacketUtility.ReadPacketID(buffer); // ID 추출

            if (packetFactories.ContainsKey(packetID)) // 패킷 팩토리가 존재하면
            {
                Packet packet = packetFactories[packetID]?.Invoke(buffer); // 패킷 생성하고
                if (packetHandlers.ContainsKey(packetID)) // 핸들러가 존재하면
                    packetHandlers[packetID]?.Invoke(session, packet); // 패킷 핸들링
            }
        }
    }
}

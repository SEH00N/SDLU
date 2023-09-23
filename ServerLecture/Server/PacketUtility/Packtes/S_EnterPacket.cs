﻿using H00N.Network;
using System;

namespace Packets
{
    public class S_EnterPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_EnterPacket;

        public string sender;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort); // 패킷 사이즈
            process += sizeof(ushort); // 패킷 아이디

            process += PacketUtility.TranslateString(buffer, process, out this.sender);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;
            process += sizeof(ushort); // 패킷의 사이즈를 넣을 공간 미리 확보

            process += PacketUtility.AppendUShortData(this.ID, buffer, process); // ID 할당
            process += PacketUtility.AppendStringData(this.sender, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0); // 아까 확보해둔 그 공간에 사이즈 할당

            return UniqueBuffer.Close(process);
        }
    }
}

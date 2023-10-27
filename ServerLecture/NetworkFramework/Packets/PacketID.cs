namespace Packets
{
    public enum PacketID
    {
        C_ChatPacket, // Client가 보내는 ChatPacket
        S_ChatPacket, // Server가 보내는 ChatPacket
        S_EnterPacket, // Server가 보내는 EnterPacket
        S_LeavePacket, // Server가 보내는 LeavePacket
    }
}
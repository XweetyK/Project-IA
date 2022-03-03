using System.Net;
using System.IO;

public enum PacketType
{
    ConnectionRequest,
    ChallengeRequest,
    ChallengeResponse,
    Connected,
    User
}

public class PacketHeader: ISerialize {
    public ushort protocolID;
    public PacketType packetType { get; set; }

    public void Serialize(Stream stream) {
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(protocolID);
        writer.Write((byte)packetType);
    }
    public void Deserialize(Stream stream) {
        BinaryReader reader = new BinaryReader(stream);
        protocolID = reader.ReadUInt16();
        packetType = (PacketType)reader.ReadByte();
    }
}

public class UserHeader: ISerialize {

    public ulong sendID;
    public uint ID;
    public uint objectID;
    public ushort userPacketType { get; set; }

    public void Serialize(Stream stream) {
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(ID);
        writer.Write(sendID);
        writer.Write(objectID);

        OnSerialize(stream);
    }
    public void Deserialize(Stream stream) {
        BinaryReader reader = new BinaryReader(stream);
        ID = reader.ReadUInt32();
        sendID = reader.ReadUInt64();
        objectID = reader.ReadUInt32();

        OnDeserialize(stream);
    }
    protected void OnSerialize(Stream stream) { }
    protected void OnDeserialize(Stream stream) { }
}
public abstract class NetworkPacket<P> 
{

    public PacketType type;
    public int clientId;
    public IPEndPoint ipEndPoint;
    public float timeStamp;
    public byte[] payload;

    public NetworkPacket(PacketType type, byte[] data, float timeStamp, int clientId = -1, IPEndPoint ipEndPoint = null)
    {
        this.type = type;
        this.timeStamp = timeStamp;
        this.clientId = clientId;
        this.ipEndPoint = ipEndPoint;
        this.payload = data;
    }
}
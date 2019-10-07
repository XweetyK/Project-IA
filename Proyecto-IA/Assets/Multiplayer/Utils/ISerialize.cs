using System.IO;

public interface ISerialize 
{
    void Serialize(Stream stream);
    void Deserialize(Stream stream);
}

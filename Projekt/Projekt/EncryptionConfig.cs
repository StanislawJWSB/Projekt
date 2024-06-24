using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class EncryptionConfig
{
    public string EncryptionMethod { get; set; }
    public string Key { get; set; }
    public string IV { get; set; }

    public void SaveConfig(string filePath)
    {
        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            formatter.Serialize(stream, this);
        }
    }

    public static EncryptionConfig LoadConfig(string filePath)
    {
        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            return (EncryptionConfig)formatter.Deserialize(stream);
        }
    }
}

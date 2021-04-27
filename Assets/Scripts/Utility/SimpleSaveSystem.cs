using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SimpleSaveSystem
{    
    public static void SaveObjectToDisk<T>(string filename, T targetObject)
    {
        var path = $"{Application.persistentDataPath}/{filename}.ssave";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.OpenWrite(path);
        binaryFormatter.Serialize(fileStream, targetObject);

        fileStream.Close();
    }

    public static T LoadFromDrive<T>(string filename)
    {
        var path = $"{Application.persistentDataPath}/{filename}.ssave";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.OpenRead(path);
            T deserializedObject = (T)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();

            return deserializedObject;
        }
        else
        {
            return default;
        }
    }
}

using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static void SaveGame(GameManager gm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.lol";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveFile file = new SaveFile(gm);

        formatter.Serialize(stream, file);
        stream.Close();
    }

    public static SaveFile loadGame()
    {
        string path = Application.persistentDataPath + "/save.lol";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveFile data = formatter.Deserialize(stream) as SaveFile;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("No save found");
            return null;
        }
    }
}

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad {

    public static bool SaveFile (GameData data) {
        string destination = Application.persistentDataPath + "/savedCheckPoint.dat";
        FileStream file;
        try {
            if (File.Exists (destination)) {
                file = File.OpenWrite (destination);
            } else {
                clearSavedFilesFolder ();
                file = File.Create (destination);
            }
            BinaryFormatter bf = new BinaryFormatter ();
            bf.Serialize (file, data);
            file.Close ();
        }
        catch (IOException e){
            Debug.Log(e);
            return false;
        }
        return true; 
    }

    public static GameData LoadFile () {
        GameData data;
        DirectoryInfo dir = new DirectoryInfo (Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles ("*.dat");
        if (info.Length > 0) {
            string destination = info[0].ToString ();
            FileStream file;
            file = File.OpenRead (destination);

            BinaryFormatter bf = new BinaryFormatter ();
           data = (GameData) bf.Deserialize (file);
            file.Close ();

            
        } else {
            return null;
            
        }
        return data;

    }

    public static void clearSavedFilesFolder () {
        if (Directory.Exists (Application.persistentDataPath)) {
            var hi = Directory.GetFiles (Application.persistentDataPath);

            for (int i = 0; i < hi.Length; i++) {
                File.Delete (hi[i]);
            }
        }
    }
    public static void UpdateSavedFile(int life)
    {
        string destination = Application.persistentDataPath + "/savedCheckPoint.dat";
        FileStream file;
        try
        {
            if (File.Exists(destination))
            {
                file = File.OpenRead(destination);
                BinaryFormatter bf = new BinaryFormatter();
                GameData  data = (GameData)bf.Deserialize(file);
                data.setLife(life);
                file.Close();

                file = File.OpenWrite(destination);

                bf.Serialize(file, data);
                file.Close();
            }
        }
        catch (IOException e)
        {
            Debug.Log(e);

        }
       
    }

}
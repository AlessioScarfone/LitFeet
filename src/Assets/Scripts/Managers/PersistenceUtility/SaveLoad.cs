using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad {

    // private static string destination = Path.Combine(Application.persistentDataPath,"savedCheckPoint.dat");
    // private static string destinationFolder = Application.persistentDataPath;
    private static string destinationFolder = Application.streamingAssetsPath;
    private static string destination = Path.Combine(Application.streamingAssetsPath,"savedCheckPoint.dat");

    public static bool Save(GameData data) {
        FileStream file;
        try {
            if (File.Exists (destination)) {
                file = File.OpenWrite (destination);
            } else {
                // clearSavedFilesFolder ();
                file = File.Create (destination);
            }
            BinaryFormatter bf = new BinaryFormatter ();
            bf.Serialize (file, data);
            file.Close ();
        } catch (IOException e) {
            Debug.Log (e);
            return false;
        }
        return true;
    }

    public static GameData LoadFile () {
        GameData data;
        // DirectoryInfo dir = new DirectoryInfo (destinationFolder);
        // FileInfo[] info = dir.GetFiles ("*.dat");
        // if (info.Length > 0) {
        if (File.Exists (destination)) {
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
        if (Directory.Exists (destinationFolder)) {
            var hi = Directory.GetFiles (destinationFolder);

            for (int i = 0; i < hi.Length; i++) {
                File.Delete (hi[i]);
            }
        }
    }
    public static void UpdateSavedFile (int life) {
        FileStream file;
        try {
            if (File.Exists (destination)) {
                file = File.OpenRead (destination);
                BinaryFormatter bf = new BinaryFormatter ();
                GameData data = (GameData) bf.Deserialize (file);
                data.setLife (life);
                file.Close ();

                file = File.OpenWrite (destination);

                bf.Serialize (file, data);

                file.Close ();
            }
        } catch (IOException e) {
            Debug.Log (e.Message);
        }
    }

    public static string getSavePath(){
        return destination;
    }

}
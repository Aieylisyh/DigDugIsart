using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager {

    [System.Serializable]
    public struct Score {

        public string name;
        public uint value;

        public Score (string name, uint score) {
            this.name  = name;
            this.value = score;
        }
    }

    const string SaveName = "save.ddm";

    static List<Score> m_save;

	public static void SaveScore (string playerName, uint score) {
        m_save.Add( new Score(playerName, score) );
        WriteSave();
    }

    public static List<Score> GetScores () {
        if (m_save == null) {
            LoadSave();
        }
        return m_save;
    }

    static void WriteSave () {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream f = File.Open( Application.persistentDataPath + "/" + SaveName, FileMode.OpenOrCreate );
        
        bf.Serialize( f, m_save );
        f.Close();
    }

    static void LoadSave () {

        if (File.Exists( Application.persistentDataPath + "/" + SaveName )) {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream f = File.Open( Application.persistentDataPath + "/" + SaveName, FileMode.Open );

            m_save = (List<Score>)bf.Deserialize( f );
            f.Close();

        } else {
            m_save = new List<Score>();
        }
    }
}

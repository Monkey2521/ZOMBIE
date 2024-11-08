using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace ZombieSurvival.General
{
    public static class GameData
    {
        #region Scene IDs
        public static readonly int LoadingScene = 0;
        public static readonly int MainMenuScene = 1;
        public static readonly int MainGameScene = 2;
        public static readonly int FirstTutorialScene = 3;
        #endregion

        #region Serialization
        public static readonly string DefaultPath = Application.persistentDataPath + "/";

        public static bool Save(string path, SerializableData data)
        {
            if (data == null) return false;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);

            bf.Serialize(file, data);
            file.Close();

            return true;
        }

        public static SerializableData Load(string path)
        {
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);

                SerializableData data = (SerializableData)bf.Deserialize(file);

                file.Close();

                return data;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
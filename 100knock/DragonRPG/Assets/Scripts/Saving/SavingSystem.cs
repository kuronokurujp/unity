using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private readonly string lastSceneLoadBuildIndexKeyName = "lastSceneLoadBuildIndex";
        public string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, string.Format("{0}.sav", fileName));
        }
        public void Save(string fileName)
        {
            // 元のデータから更に追加する
            var dic = this.LoadFile(fileName);
            this.SaveFile(fileName, this.Capture(dic));
        }
        public IEnumerator LoadLastScene(string fileName)
        {
            var dic = this.LoadFile(fileName);
            if (dic.ContainsKey(this.lastSceneLoadBuildIndexKeyName))
            {
                int buildIndex = (int)dic[this.lastSceneLoadBuildIndexKeyName];
                yield return SceneManager.LoadSceneAsync(buildIndex);
            }

            this.Restore(dic);
        }
        public void Load(string fileName)
        {
            this.Restore(this.LoadFile(fileName));
        }
        public void Delete(string fileName)
        {
            this.DeleteFile(fileName);
        }
        private void DeleteFile(string fileName)
        {
            string filePath = this.GetPath(fileName);
            if (!File.Exists(filePath)) return;

            Debug.LogFormat("Delete to {0}", filePath);
            File.Delete(filePath);
        }
        private void SaveFile(string fileName, object state)
        {
            string filePath = this.GetPath(fileName);
            Debug.LogFormat("save to {0}", filePath);

            using (var stream = File.Open(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }
        private Dictionary<string, object> LoadFile(string fileName)
        {
            string filePath = this.GetPath(fileName);
            if (!File.Exists(filePath)) 
            {
                Debug.LogFormat("it is not loading from {0}", filePath);
                return new Dictionary<string, object>();
            }

            Debug.LogFormat("loading from {0}", filePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }
        private Dictionary<string, object> Capture(Dictionary<string, object> dic)
        {
            foreach (SaveableEntity entity in GameObject.FindObjectsOfType(typeof(SaveableEntity)))
            {
                dic[entity.GetUniqueIdentifier()] = entity.CaptureState();
            }

            dic[this.lastSceneLoadBuildIndexKeyName] = SceneManager.GetActiveScene().buildIndex;

            return dic;
        }
        private void Restore(Dictionary<string, object> dic)
        {
            foreach (SaveableEntity entity in GameObject.FindObjectsOfType(typeof(SaveableEntity)))
            {
                string key = entity.GetUniqueIdentifier();
                if (dic.ContainsKey(key))
                {
                    entity.RestoreState(dic[key]);
                }
            }
        }
    }
}
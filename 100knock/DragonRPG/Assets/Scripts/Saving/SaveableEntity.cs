using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField]
        private string uniqueIdentifier = string.Empty;

        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return this.uniqueIdentifier;
        }
        public object CaptureState()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var saveable in this.GetComponents<ISaveable>())
            {
                var key = saveable.GetType().ToString();
                dic[key] = saveable.CaptureState();
            }

            return dic;
        }
        public void RestoreState(object state)
        {
            Dictionary<string, object> dic = (Dictionary<string, object>)(state);
            foreach (var saveable in this.GetComponents<ISaveable>())
            {
                var key = saveable.GetType().ToString();
                if (!dic.ContainsKey(key)) continue;

                saveable.RestoreState(dic[key]);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (Application.IsPlaying(this)) return;
            if (string.IsNullOrEmpty(this.gameObject.scene.path)) return;

            // シーンに初期配置した時にセーブ識別子を生成
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            UnityEditor.SerializedProperty uniqueIdentifierProp = serializedObject.FindProperty("uniqueIdentifier");

            // IDが被っていないかチェック
            if (!this.IsUnique(uniqueIdentifierProp.stringValue))
            {
                uniqueIdentifierProp.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[uniqueIdentifierProp.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {
            // 空文字列であればidそのものがないのでユニークではない
            if (string.IsNullOrEmpty(candidate)) return false;

            // idはあるが、管理テーブルにない場合は被っていない
            if (!globalLookup.ContainsKey(candidate)) return true;

            // idがあり、管理テーブルにもあってkeyのデータが自身であれば被っている
            if (globalLookup[candidate] == this) return true;

            // キーはあるが、データがない場合はテーブルから削除する。
            // そして引数の値を被っていないとする
            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            // キーのデータはある。
            // しかしデータが保持しているidと引数のidが異なっているので管理テーブルから削除して引数のidをユニークとする
            // prefab設定したid内容を反映させるため
            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
#endif
    }   
}


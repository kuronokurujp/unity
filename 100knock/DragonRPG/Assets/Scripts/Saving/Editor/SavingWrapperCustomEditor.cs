using UnityEditor;
using UnityEngine;
using System.IO;

namespace RPG.Saving 
{
    [CustomEditor(typeof(SavingWrapper))]
    public class SavingWrapperCustomEditor : Editor 
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            SavingWrapper savingWrapperObject = this.target as SavingWrapper;
            if (GUILayout.Button("Open to SaveFile"))
            {
                // windowsのメモ帳でセーブファイルを開いている
                // 他のosはサポートしていないので注意
                if (File.Exists(savingWrapperObject.GetSaveFileFullPath()))
                {
                    System.Diagnostics.Process.Start("notepad.exe", savingWrapperObject.GetSaveFileFullPath());
                }
                else
                {
                    Debug.LogFormat("{0} File is not Exist", savingWrapperObject.GetSaveFileFullPath());
                }
            }

            if (GUILayout.Button("Delete to SaveFile"))
            {
                savingWrapperObject.Delete();
            }
        }
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PrototypeSystem.PrototypeLoader {
    public class ScriptableObjectPrototypeLoader<TPrototypeData> : IPrototypeLoader<TPrototypeData> where TPrototypeData : ScriptableObject, IPrototypeData {
        private readonly string _rootFolder;
        
        public ScriptableObjectPrototypeLoader(string rootFolder = null) {
            _rootFolder = rootFolder;
        }
        
        public Dictionary<string, TPrototypeData> LoadAll() {
            var prototypeDatas = new Dictionary<string, TPrototypeData>();
            GUID[] assetGUIDs = AssetDatabase.FindAssetGUIDs($"t:{typeof(TPrototypeData).Name}", new[] {_rootFolder});
            Debug.Log($"Found {assetGUIDs.Length} asset GUIDs");
            foreach (GUID assetGUID in assetGUIDs) {
                var scriptableObjectPrototypeData = AssetDatabase.LoadAssetAtPath<TPrototypeData>(AssetDatabase.GUIDToAssetPath(assetGUID));
                prototypeDatas.Add(scriptableObjectPrototypeData.IDName, scriptableObjectPrototypeData);
            }
            // TData[] prototypeObjects = Resources.LoadAll<TPrototypeData>(_pathToLoadPrototypeObjects);
            // Debug.Log($"Loaded {prototypeObjects.Length} prototype objects from {_pathToLoadPrototypeObjects}");
            // foreach (TPrototypeData prototypeObject in prototypeObjects) {
            //     prototypeDatas.Add(prototypeObject.IDName, prototypeObject);
            // }

            return prototypeDatas;
        }
    }
}
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PrototypeSystem.PrototypeLoader {
    public class ScriptableObjectPrototypeLoader<TPrototypeData> : IPrototypeLoader<TPrototypeData> where TPrototypeData : ScriptableObject, IPrototypeData {
        private readonly string _rootFolder;
        
        public ScriptableObjectPrototypeLoader(string rootFolder = null) {
            _rootFolder = rootFolder;
        }
        
        public static IPrototypeCollection<TPrototypeData> CreatePrototypeCollection(string rootFolder = null) {
            return new PrototypeCollection<TPrototypeData>(new ScriptableObjectPrototypeLoader<TPrototypeData>(rootFolder));
        }
        
        public Dictionary<string, TPrototypeData> LoadAll() {
            var prototypeDatas = new Dictionary<string, TPrototypeData>();
            GUID[] assetGUIDs = string.IsNullOrWhiteSpace(_rootFolder) ? AssetDatabase.FindAssetGUIDs($"t:{typeof(TPrototypeData).Name}",new []{"Assets"}) : AssetDatabase.FindAssetGUIDs($"t:{typeof(TPrototypeData).Name}", new[] {_rootFolder});
            Debug.Log($"Found {assetGUIDs.Length} assets of type {typeof(TPrototypeData).Name}");
            foreach (GUID assetGUID in assetGUIDs) {
                var scriptableObjectPrototypeData = AssetDatabase.LoadAssetAtPath<TPrototypeData>(AssetDatabase.GUIDToAssetPath(assetGUID));
                if (string.IsNullOrWhiteSpace(scriptableObjectPrototypeData.IDName)) Debug.LogError($"Scriptable Object of type {typeof(TPrototypeData).Name} at {AssetDatabase.GUIDToAssetPath(assetGUID)} with assetGUID {assetGUID} has no IDName set.");
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
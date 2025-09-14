using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TypeObjectSystem.TypeLoader {
    public class ScriptableObjectTypeLoader<TType> : ITypeLoader<TType> where TType : ScriptableObject, IType {
        private readonly string _rootFolder;
        
        public ScriptableObjectTypeLoader(string rootFolder = null) {
            _rootFolder = rootFolder;
        }
        
        public static ITypeCollection<TType> CreateTypeCollection(string rootFolder = null) {
            return new TypeCollection<TType>(new ScriptableObjectTypeLoader<TType>(rootFolder));
        }
        
        public Dictionary<string, TType> LoadAll() {
            var typeDatas = new Dictionary<string, TType>();
            GUID[] assetGUIDs = string.IsNullOrWhiteSpace(_rootFolder) ? AssetDatabase.FindAssetGUIDs($"t:{typeof(TType).Name}",new []{"Assets"}) : AssetDatabase.FindAssetGUIDs($"t:{typeof(TType).Name}", new[] {_rootFolder});
            Debug.Log($"Found {assetGUIDs.Length} assets of type {typeof(TType).Name}");
            foreach (GUID assetGUID in assetGUIDs) {
                var scriptableObjectTypeData = AssetDatabase.LoadAssetAtPath<TType>(AssetDatabase.GUIDToAssetPath(assetGUID));
                if (string.IsNullOrWhiteSpace(scriptableObjectTypeData.IDName)) Debug.LogError($"Scriptable Object of type {typeof(TType).Name} at {AssetDatabase.GUIDToAssetPath(assetGUID)} with assetGUID {assetGUID} has no IDName set.");
                typeDatas.Add(scriptableObjectTypeData.IDName, scriptableObjectTypeData);
                // TODO is there a way to use parents in ScriptableObjects?
            }

            return typeDatas;
        }
    }
}

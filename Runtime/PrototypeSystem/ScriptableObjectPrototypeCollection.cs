using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PrototypeSystem {
    public class ScriptableObjectPrototypeCollection<TPrototypeData> : DictionaryPrototypeCollection<TPrototypeData>
        where TPrototypeData : ScriptableObjectPrototypeData {

        protected override Dictionary<string, TPrototypeData> LoadPrototypeDatas() {
            var prototypeDatas = new Dictionary<string, TPrototypeData>();
            GUID[] assetGUIDs = AssetDatabase.FindAssetGUIDs($"t:{typeof(TPrototypeData).Name}");
            Debug.Log($"Found {assetGUIDs.Length} asset GUIDs");
            foreach (GUID assetGUID in assetGUIDs) {
                var scriptableObjectPrototypeData = AssetDatabase.LoadAssetAtPath<TPrototypeData>(AssetDatabase.GUIDToAssetPath(assetGUID));
                prototypeDatas.Add(scriptableObjectPrototypeData.IDName, scriptableObjectPrototypeData);
            }
            // TPrototypeData[] prototypeObjects = Resources.LoadAll<TPrototypeData>(_pathToLoadPrototypeObjects);
            // Debug.Log($"Loaded {prototypeObjects.Length} prototype objects from {_pathToLoadPrototypeObjects}");
            // foreach (TPrototypeData prototypeObject in prototypeObjects) {
            //     prototypeDatas.Add(prototypeObject.IDName, prototypeObject);
            // }

            return prototypeDatas;
        }
        
    }
}
using PrototypeSystem.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public class JsonPrototypeLoaderSO<T> : PrototypeLoaderSO<T> where T : IPrototypeData {

        [SerializeField] private string relativeFolder = "Prototypes";
        [SerializeField] private string rootFolder = Application.streamingAssetsPath;

        protected override IPrototypeLoader<T> CreateInnerLoader() {
            return new JSonPrototypeLoader<T>(relativeFolder, rootFolder);
        }
    }
}
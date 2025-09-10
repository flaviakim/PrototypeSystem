using PrototypeSystem.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public class ScriptableObjectPrototypeLoaderSO<T> : PrototypeLoaderSO<T> where T : ScriptableObjectPrototypeData {
        [SerializeField] private string rootFolder = "Assets";

        protected override IPrototypeLoader<T> CreateInnerLoader() {
            return new ScriptableObjectPrototypeLoader<T>(rootFolder);
        }
    }
}
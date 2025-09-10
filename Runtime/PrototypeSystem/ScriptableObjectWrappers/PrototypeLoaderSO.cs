using System.Collections.Generic;
using PrototypeSystem.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystem.ScriptableObjectWrappers {
    public abstract class PrototypeLoaderSO<T> : ScriptableObject, IPrototypeLoader<T> where T : IPrototypeData {
        private IPrototypeLoader<T> _innerLoader;
        private IPrototypeLoader<T> InnerLoader => _innerLoader ??= CreateInnerLoader();
        protected abstract IPrototypeLoader<T> CreateInnerLoader();

        public Dictionary<string, T> LoadAll() {
            return InnerLoader.LoadAll();
        }
    }
}
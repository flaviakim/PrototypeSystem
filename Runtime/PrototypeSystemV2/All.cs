using System;
// using PrototypeSystem;
// using PrototypeSystem.PrototypeLoader;
using UnityEngine;

namespace PrototypeSystemV2 {

    public class InstanceFactory<TInstance, TPrototypeData, TInstanceData>
        where TInstance : IInstance<TPrototypeData, TInstanceData>
        where TPrototypeData : PrototypeSystem.IPrototypeData
        where TInstanceData : IInstanceData {
        public PrototypeSystem.PrototypeCollection<TPrototypeData> PrototypeCollection { get; set; }

        private readonly Func<TInstance> _newInstanceCreator;

        public InstanceFactory(PrototypeSystem.PrototypeCollection<TPrototypeData> prototypeCollection, Func<TInstance> newInstanceCreator) { 
            PrototypeCollection = prototypeCollection;
            _newInstanceCreator = newInstanceCreator;
        }

        public InstanceFactory(PrototypeSystem.PrototypeLoader.IPrototypeLoader<TPrototypeData> prototypeLoader, Func<TInstance> newInstanceCreator) {
            PrototypeCollection = new PrototypeSystem.PrototypeCollection<TPrototypeData>(prototypeLoader);
            _newInstanceCreator = newInstanceCreator;
        }

        public TInstance CreateInstance(string idName, TInstanceData instanceData) {
            if (!PrototypeCollection.TryGetPrototypeForName(idName, out TPrototypeData prototypeData)) {
                Debug.LogError($"Prototype with idName {idName} not found.");
                return default;
            }

            TInstance instance = _newInstanceCreator.Invoke();
            instance.Initialize(prototypeData, instanceData);
            return instance;
        }
    }

    public class InstanceFactoryMonoBehaviour<TInstance, TPrototypeData, TInstanceData> : InstanceFactory<TInstance,
        TPrototypeData, TInstanceData>
        where TInstance : MonoBehaviour, IInstance<TPrototypeData, TInstanceData>
        where TPrototypeData : PrototypeSystem.IPrototypeData
        where TInstanceData : IInstanceData {
        private static TInstance NewInstanceCreator() {
            var go = new GameObject(typeof(TInstance).Name);
            return go.AddComponent<TInstance>();
        }

        public InstanceFactoryMonoBehaviour(PrototypeSystem.PrototypeLoader.IPrototypeLoader<TPrototypeData> prototypeLoader) : base(prototypeLoader,
            NewInstanceCreator) {
        }
    }

    public class InstanceFactoryNew<TInstance, TPrototypeData, TInstanceData> : InstanceFactory<TInstance, TPrototypeData, TInstanceData>
        where TInstance : IInstance<TPrototypeData, TInstanceData>, new()
        where TPrototypeData : PrototypeSystem.IPrototypeData
        where TInstanceData : IInstanceData {
        private static TInstance NewInstanceCreator() {
            return new TInstance();
        }

        public InstanceFactoryNew(PrototypeSystem.PrototypeLoader.IPrototypeLoader<TPrototypeData> prototypeLoader) : base(prototypeLoader, NewInstanceCreator) {
        }
    }
    
    
    #region Examples

    public class Map : MonoBehaviour {
        private void Start() {
            var tileFactory =
                new InstanceFactoryNew<Tile, TilePrototypeData, TileInstanceData>(
                    new PrototypeSystem.PrototypeLoader.JSonPrototypeLoader<TilePrototypeData>(Application.streamingAssetsPath));
            var grid = new Tile[5, 5];
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    grid[x, y] = tileFactory.CreateInstance("grass", new TileInstanceData(new Vector2Int(x, y)));
                }
            }
        }
    }

    public class Tile : IInstance<TilePrototypeData, TileInstanceData> {
        public TilePrototypeData PrototypeData { get; private set; }
        public TileInstanceData InstanceData { get; private set; }

        public void Initialize(TilePrototypeData prototypeData, TileInstanceData instanceData) {
            PrototypeData = prototypeData;
            InstanceData = instanceData;
        }
    }

    public class TileInstanceData : IInstanceData {
        public TileInstanceData(Vector2Int position) {
            Position = position;
        }

        public Vector2Int Position { get; private set; }
    }

    public class TilePrototypeData : PrototypeSystem.IPrototypeData {
        public TilePrototypeData(string idName, int movementCost) {
            IdName = idName;
            MovementCost = movementCost;
        }

        public string IdName { get; }
        public int MovementCost { get; }
        public string IDName { get; }
        public string BasedOn { get; }
    }

    public interface IInstance<TPrototypeData, TInstanceData> where TPrototypeData : PrototypeSystem.IPrototypeData {
        public TPrototypeData PrototypeData { get; }
        public TInstanceData InstanceData { get; }
        public void Initialize(TPrototypeData prototypeData, TInstanceData instanceData);
    }

// public interface IPrototypeData {
//     public string IdName { get; }
// }

    public interface IInstanceData {
    }
    
    #endregion
}
namespace PrototypeSystem {
    public interface IInstance<out TData> where TData : IPrototypeData {
        public string IDName => Data.IDName;
        public TData Data { get; }
    }
}
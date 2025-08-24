// namespace PrototypeSystem {
//     public interface IInstanceData<out TInstanceData>
//             where TInstanceData : IInstanceData<TInstanceData> {
//         // TInstanceData Clone();
//     }
//     
//     public abstract class BaseInstanceData<TInstanceData> : IInstanceData<TInstanceData>
//             where TInstanceData : IInstanceData<TInstanceData> {
//
//         protected BaseInstanceData(BaseInstanceData<TInstanceData> original) { }
//
//         protected BaseInstanceData() { }
//
//         // public abstract TInstanceData Clone();
//     }
// }
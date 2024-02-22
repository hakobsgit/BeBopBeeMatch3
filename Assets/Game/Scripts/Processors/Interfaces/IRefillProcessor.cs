using Game.Data;
using UniRx;

namespace Game.Processors {
    public interface IRefillProcessor {
        ReactiveCommand OnRefilled { get; }
        void Refill(GridData grid);
    }
}
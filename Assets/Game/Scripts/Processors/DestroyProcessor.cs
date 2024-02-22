using Game.Data;
using Zenject;

namespace Game.Processors {
    public class DestroyProcessor : IDestroyProcessor {
        [Inject] private IRefillProcessor _refillProcessor;

        public void Destroy(GridData grid) {
            foreach (var point in grid.DestroyedPoints) {
                if (!grid[point]) continue;
                grid[point].gameObject.SetActive(false);
                grid[point] = null;
            }

            _refillProcessor.Refill(grid);
        }
    }
}
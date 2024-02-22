using Game.Data;
using Game.Objects;

namespace Game.Processors {
    public interface IMatchProcessor {
        bool Process(GridData grid, Tile swapTile1, Tile swapTile2);
    }
}
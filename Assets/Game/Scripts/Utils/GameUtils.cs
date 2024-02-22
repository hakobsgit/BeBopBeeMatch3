using Game.Data;
using Game.Data.Enums;
using Game.Objects;

namespace Game.Utils {
    public static class GameUtils {
        public static bool IsPossibleSwipeDirection(GridData grid, Tile tile, Direction direction) {
            if ((direction == Direction.Left && tile.GridPoint.x == 0) ||
                (direction == Direction.Right && tile.GridPoint.x == grid.Columns - 1) ||
                (direction == Direction.Up && tile.GridPoint.y == grid.Rows - 1) ||
                (direction == Direction.Down && tile.GridPoint.y == 0)) {
                return false;
            }

            return true;
        }
    }
}
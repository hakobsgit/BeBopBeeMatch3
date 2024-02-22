using System.Collections;

namespace Game.Controllers {
    public interface ISimulationController {
        void RandomMoves(int count = 1);
        IEnumerator RandomMovesWithAnimation(int count = 1);
    }
}
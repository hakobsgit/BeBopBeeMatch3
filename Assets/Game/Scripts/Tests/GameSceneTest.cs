using Zenject;
using System.Collections;
using System.Diagnostics;
using Game.Configs;
using Game.Controllers;
using Game.Data.Enums;
using Game.Processors;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

public class GameSceneTest : SceneTestFixture {
    [UnityTest]
    public IEnumerator TestSceneIfGridCreatedWithoutMatches() {
        var gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>("Assets/Game/Configs/General/Game Config.asset");
        var animationsValue = gameConfig.UseAnimations;
        gameConfig.UseAnimations = false;

        yield return LoadScene("Game");

        var gameController = SceneContainer.Resolve<IGameController>();

        yield return new WaitForSeconds(5);

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        var matchFound = new FullGridMatchProcessor(null).IsAnyMatch(gameController.Grid);

        stopWatch.Stop();

        Debug.Log("Time::: " + stopWatch.ElapsedMilliseconds + "ms");

        gameConfig.UseAnimations = animationsValue;

        Assert.IsFalse(matchFound, "Match detected! The grid should be created without any matches.");
    }

    [UnityTest]
    public IEnumerator TestSceneImmediateMoves1M() {
        var gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>("Assets/Game/Configs/General/Game Config.asset");
        var animationsValue = gameConfig.UseAnimations;
        gameConfig.UseAnimations = false;

        yield return LoadScene("Game");

        var simulationController = SceneContainer.Resolve<ISimulationController>();

        yield return new WaitForSeconds(2);


        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        simulationController.RandomMoves(1000000);

        stopWatch.Stop();


        Debug.Log("Time::: " + stopWatch.ElapsedMilliseconds + "ms");

        gameConfig.UseAnimations = animationsValue;

        Assert.IsTrue(
            stopWatch.ElapsedMilliseconds <= (gameConfig.MatchType == MatchType.SwappedTiles ? 50000 : 100000),
            $"1,000,000 moves took more than {(gameConfig.MatchType == MatchType.SwappedTiles ? 50 : 100)} seconds");
    }

    [UnityTest]
    public IEnumerator TestSceneAnimatedMoves100() {
        var gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>("Assets/Game/Configs/General/Game Config.asset");
        var animationsValue = gameConfig.UseAnimations;
        gameConfig.UseAnimations = true;

        yield return LoadScene("Game");

        var simulationController = SceneContainer.Resolve<ISimulationController>();

        yield return new WaitForSeconds(2);

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        yield return simulationController.RandomMovesWithAnimation(100);

        stopWatch.Stop();


        Debug.Log("Time::: " + stopWatch.ElapsedMilliseconds + "ms");

        gameConfig.UseAnimations = animationsValue;
    }
}
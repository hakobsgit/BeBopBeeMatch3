
# BeBopBee Match3

Match 3 test project for BeBopBee


## Documentation

To Setup the game we have 3 main configs.

**Location**: /Assets/Game/Configs

- **GameConfig**
  - **TileSwipeSensitivity**: Sensitivity of swipe detection
  - **UseAnimations**: If true the game will play animations
  - **MatchType**: 
    - **SwappedTiles**: Match detection will be processed for swapped tiles
    - **FullGrid**: Match detection will be processed for full grid, checks for matched tiles after every refilling.
- **LevelConfig**
  - **Columns**: Grid columns
  - **Rows**: Grid rows
  - **CellSize**: Grid cell size
  - **OverrideTilesConfig**: Possibility to override tiles for every level
- **TilesConfig**
  - **DefaultSprite**: Tiles default sprite
  - **Tiles**: 
    - **Prefab**: Each tile can have it's own prefab, with this you can add anything to the tile
    - **Sprite**: Sprite of the tile, if it is not defined DefaultSprite will be used
    - **Color**: Color of the tile, this does not have an effect with Prefab type, because for Prefab type sprite renderer is disabled

## Simulation API

Simulation uses the same functionality as the real game

**ISimulationController**

#### Make random moves in one frame without animations

```
  void RandomMoves(int count = 1)
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `count` | `int` | **Optional**. Count of moves to do |

#### Make random moves with animations

```
  IEnumerator RandomMovesWithAnimation(int count = 1)
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `count`      | `int` | **Optional**. Count of moves to do |


## Tests

The project currently contains 3 PlayMode Unity Tests

- **TestSceneIfGridCreatedWithoutMatches**: Tests if grid has been created without matches
- **TestSceneImmediateMoves1M**: Tests the Game scene by doing immediate 1,000,000 moves, will log the time used for processing 1M moves in milliseconds
- **TestSceneAnimatedMoves100**: Tests the Game scene by doing 100 animated moves

![App Screenshot](https://raw.githubusercontent.com/hakobsgit/BeBopBeeMatch3/main/Documentation/tests.png)

## Simulate moves from the inspector

GameView has a custom inspector which gives us ability to simulate moves runtime

**GameViewCustomInspector**
- **Simulate 1M Immediate**: Simulates 1,000,000 moves in one frame
- **Simulate 1M Animated**: Simulates 1,000,000 moves with animations

![App Screenshot](https://raw.githubusercontent.com/hakobsgit/BeBopBeeMatch3/main/Documentation/game_view_inspector.png)

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LevelCreatorTest
{ 
    [UnityTest]
    public IEnumerator IsTileAmountCorrectAfterLevelCreation()
    {
        LevelCreator levelCreator = SetLevel();
        while (!levelCreator.FinishedLevel)
        {
            yield return null;
        }
        Assert.AreEqual(levelCreator.MapSize, levelCreator.CreatedTiles.Count);
        Debug.Log($"Map size:{levelCreator.MapSize} , Tile Amount: {levelCreator.CreatedTiles.Count}");

    }

    [UnityTest]
    public IEnumerator GenerateLevelAndCreatePath()
    { 
        LevelCreator levelCreator = SetLevel();
        var pathManager = new GameObject();
        var pathScript = pathManager.AddComponent<PathSelector>();

        pathScript.StartPath(levelCreator.CreatedTiles[0]); 
        pathScript.StartPath(levelCreator.CreatedTiles[levelCreator.CreatedTiles.Count - 1]);

        Assert.IsNotNull(pathScript.Nodes);
        Assert.IsTrue(pathScript.Nodes.Count > 0);
        
        yield return null;
    }

    private static LevelCreator SetLevel()
    {
        var gameObject = new GameObject();
        var levelCreator = gameObject.AddComponent<LevelCreator>();
        levelCreator.SetHeight(5);
        levelCreator.SetWidth(5); 
        levelCreator.SetOffset(new Vector3(1f,0,0.75f));
        levelCreator.HexTiles = new List<TileObject>
        { 
             Resources.Load<TileObject>("Prefabs/TileObjects/WaterTile"),
             Resources.Load<TileObject>("Prefabs/TileObjects/DesertTile"),
             Resources.Load<TileObject>("Prefabs/TileObjects/GrassTile"),
             Resources.Load<TileObject>("Prefabs/TileObjects/MountainTile"),
             Resources.Load<TileObject>("Prefabs/TileObjects/ForestTile")
        };

        levelCreator.CreateLevel();
        return levelCreator;
    }

   
}

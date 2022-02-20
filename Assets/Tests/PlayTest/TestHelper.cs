using System.Collections.Generic;
using UnityEngine;

public static class TestHelper
{
    public static LevelCreator SetLevel(GameObject obj)
    { 
        obj = SetManager(obj);
        var levelCreator = obj.GetComponent<LevelCreator>(); 
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

    public static GameObject SetManager(GameObject gameObj)
    {
        gameObj.AddComponent<UIManager>();
        gameObj.AddComponent<LevelCreator>();
        gameObj.AddComponent<PathManager>();

        return gameObj;
    } 

}

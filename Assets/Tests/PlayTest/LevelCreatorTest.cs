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
        var gameObject = new GameObject();
        var levelCreator = gameObject.AddComponent<LevelCreator>();
        levelCreator.SetHeight(5);
        levelCreator.SetWidth(5);
        levelCreator.HexTile = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/HexTile"));
        levelCreator.CreateLevel();
        while(!levelCreator.FinishedLevel)
        {  
            yield return null;
        }
        Assert.AreEqual(levelCreator.MapSize, levelCreator.TilesAmount);
        Debug.Log($"Map size:{levelCreator.MapSize} , Tile Amount: {levelCreator.TilesAmount}" );
        
    }
}

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
        GameObject mainObj = new GameObject();
        mainObj = TestHelper.SetManager(mainObj);
        var levelCreator = TestHelper.SetLevel(mainObj);
        
        while (!levelCreator.FinishedLevel)
        {
            yield return new WaitForSeconds(1f);
        }
        Assert.AreEqual(levelCreator.MapSize, levelCreator.CreatedTiles.Count);
        Debug.Log($"Map size:{levelCreator.MapSize} , Tile Amount: {levelCreator.CreatedTiles.Count}");

    } 

    
   
}

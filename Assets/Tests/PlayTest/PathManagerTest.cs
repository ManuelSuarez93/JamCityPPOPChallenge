using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathManagerTest
{ 
    [UnityTest]
    public IEnumerator GenerateLevelAndCreatePath()
    { 
        GameObject mainObj = new GameObject();
        mainObj = TestHelper.SetManager(mainObj); 

        var levelCreator = TestHelper.SetLevel(mainObj);
        var pathManager = new GameObject();
        var pathScript = pathManager.AddComponent<PathManager>();

        pathScript.StartPath(levelCreator.CreatedTiles[0]); 
        pathScript.StartPath(levelCreator.CreatedTiles[levelCreator.CreatedTiles.Count - 1]);

        Assert.IsNotNull(pathScript.Nodes);
        Assert.IsTrue(pathScript.Nodes.Count > 0);
        
        yield return null;
    }
}

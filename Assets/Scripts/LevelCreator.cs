using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{ 
    #region Fields
    [SerializeField] List<TileObject> _hexTiles;
    [SerializeField] int _mapHeight, _mapWidth;
    [SerializeField] Vector3 _offset, _moveOnSpawn;
    [SerializeField] float _tileAnimSpeed;
    private List<HexTile> _createdTiles = new List<HexTile>();
    private bool _finishedLevel;
    #endregion  

    private void Start()
    {
        _mapHeight = (int)UIManager.Instance.HeightAmount;
        _mapWidth = (int)UIManager.Instance.WidthAmount;
    }

    #region Level Create methods
    public void CreateLevel()
    {
        DestroyLevel(); 
        StartCoroutine(CreateLevelRoutine());
    }

    public void DestroyLevel()
    {
        PathManager.Instance.ResetPath();
        _createdTiles.ForEach(x => Destroy(x.gameObject));
        _createdTiles.Clear();
    }
    
    private IEnumerator CreateLevelRoutine()
    {
        UIManager.Instance.EnableButtons(false);
        
        
        float currentoffsetx = 0, currentoffsetz = 0;
        _finishedLevel = false; 
        for(int i = 0; i < _mapHeight; i++)
        { 
            currentoffsetz -= i > 0 ? _offset.z : 0;
            currentoffsetx += i % 2 == 0 ? 0 : _offset.x/2;
            for(int j = 0; j < _mapWidth; j++)
            {
                currentoffsetx += j > 0 ? _offset.x : 0;
                CreateTile(currentoffsetx, currentoffsetz, _moveOnSpawn, _tileAnimSpeed);  
                yield return null;
            }
            currentoffsetx = 0f; 
            yield return null;
        }

        foreach(HexTile tile in _createdTiles)  
            tile.GetAdjacentTiles();  
            
        _finishedLevel = true;
        
        UIManager.Instance.EnableButtons(true);
        yield return null;
    }
    #endregion
    
    #region Helper methods
    private void CreateTile(float currentoffsetx, float currentoffsetz, Vector3 moveOnSpawn, float timeAmount)
    {
        var tile = _hexTiles[Random.Range(0,_hexTiles.Count)].CreateTile(currentoffsetx, currentoffsetz,_createdTiles.Count, moveOnSpawn, timeAmount); 

        if(_createdTiles == null) _createdTiles = new List<HexTile>(); 
        _createdTiles.Add(tile);
    }
 
    public void SetHeight(float height)
     => _mapHeight = (int)height;
    public void SetWidth(float width)
     =>  _mapWidth = (int)width; 
     public void SetOffset(Vector3 offset)
     => _offset = offset;
     #endregion
}

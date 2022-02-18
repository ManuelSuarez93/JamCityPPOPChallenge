using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public static LevelCreator Instance {get; set;}
    [SerializeField] List<TileObject> _hexTiles;
    [SerializeField] public int _mapHeight, _mapWidth;
    [SerializeField] Vector3 _offset;
    private List<HexTile> _createdTiles = new List<HexTile>();
    private bool _finishedLevel;
    public bool FinishedLevel => _finishedLevel;
    public int MapSize => _mapHeight * _mapWidth;
    public List<HexTile> CreatedTiles => _createdTiles;
    public List<TileObject> HexTiles {get => _hexTiles; set => _hexTiles = value;}

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    public void CreateLevel()
    {  
        if(_createdTiles.Count > 0) DestroyLevel(); 

        float currentoffsetx = 0, currentoffsetz = 0;
        _finishedLevel = false;
        for(int i = 0; i < _mapHeight; i++)
        { 
            currentoffsetz -= i > 0 ? _offset.z : 0;
            currentoffsetx += i % 2 == 0 ? 0 : _offset.x/2;
            for(int j = 0; j < _mapWidth; j++)
            {
                currentoffsetx += j > 0 ? _offset.x : 0;
                CreateTile(currentoffsetx, currentoffsetz);
            }
            currentoffsetx = 0f;
        }
        SetAdjacentTiles();
        _finishedLevel = true;
    }

    private void CreateTile(float currentoffsetx, float currentoffsetz)
    {
        var tile = _hexTiles[Random.Range(0,_hexTiles.Count)].CreateTile(currentoffsetx, currentoffsetz); 

        if(_createdTiles == null) _createdTiles = new List<HexTile>();

        _createdTiles.Add(tile);
    }

    private void SetAdjacentTiles()
    {
        foreach(HexTile tile in _createdTiles) 
            tile.GetAdjacentTiles(); 
    }
    public void DestroyLevel()
    {
        _createdTiles.ForEach(x => Destroy(x.gameObject));
        _createdTiles.Clear();
    }
    
    public void SetHeight(float height)
     => _mapHeight = (int)height;
    public void SetWidth(float width)
     =>  _mapWidth = (int)width; 
     public void SetOffset(Vector3 offset)
     => _offset = offset;
}

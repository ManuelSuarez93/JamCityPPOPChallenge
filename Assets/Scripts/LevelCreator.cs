using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{ 
    [SerializeField] List<TileObject> _hexTiles;
    [SerializeField] public int _mapHeight, _mapWidth;
    [SerializeField] Vector3 _offset;
    private List<HexTile> _createdTiles = new List<HexTile>();
    private bool _finishedLevel;
    public bool FinishedLevel => _finishedLevel;
    public int MapSize => _mapHeight * _mapWidth;
    public List<HexTile> CreatedTiles => _createdTiles;
    public List<TileObject> HexTiles {get => _hexTiles; set => _hexTiles = value;}

    private void Start()
    {
        _mapHeight = (int)UIManager.Instance.HeightAmount;
        _mapWidth = (int)UIManager.Instance.WidthAmount;
    }
    public void CreateLevel()
    {
        DestroyLevel(); 
        StartCoroutine(CreateLevelRoutine());
    }

    private void CreateTile(float currentoffsetx, float currentoffsetz)
    {
        var tile = _hexTiles[Random.Range(0,_hexTiles.Count)].CreateTile(currentoffsetx, currentoffsetz,_createdTiles.Count); 

        if(_createdTiles == null) _createdTiles = new List<HexTile>(); 
        _createdTiles.Add(tile);
    }
 
    public void DestroyLevel()
    {
        PathManager.Instance.ResetPath();
        _createdTiles.ForEach(x => Destroy(x.gameObject));
        _createdTiles.Clear();
    }
    
    private IEnumerator CreateLevelRoutine()
    {
        UIManager.Instance.EnableCreate(false);
        
        
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
                yield return null; 
            }
            currentoffsetx = 0f;
            yield return null; 
        }

        foreach(HexTile tile in _createdTiles)  
            tile.GetAdjacentTiles();  
            
        _finishedLevel = true;
        
        UIManager.Instance.EnableCreate(true);
        yield return null;
    }
    public void SetHeight(float height)
     => _mapHeight = (int)height;
    public void SetWidth(float width)
     =>  _mapWidth = (int)width; 
     public void SetOffset(Vector3 offset)
     => _offset = offset;
}

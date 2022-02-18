using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] GameObject _hexTiles;
    [SerializeField] public int _mapHeight, _mapWidth;

    [SerializeField] float _offsetx, _offsetz;

    private List<GameObject> _createdTiles = new List<GameObject>();
    private bool _finishedLevel;
    public bool FinishedLevel => _finishedLevel;
    public int MapSize => _mapHeight * _mapWidth;
    public int TilesAmount => _createdTiles.Count;
    public GameObject HexTile {get => _hexTiles; set => _hexTiles = value;}
    public void CreateLevel()
    {  
        if(_createdTiles.Count > 0) DestroyLevel(); 

        float currentoffsetx = 0, currentoffsetz = 0;
        _finishedLevel = false;
        for(int i = 0; i < _mapHeight; i++)
        { 
            currentoffsetz -= i > 0 ? _offsetz : 0;
            currentoffsetx += i % 2 == 0 ? 0 : _offsetx/2;
            for(int j = 0; j < _mapWidth; j++)
            {
                currentoffsetx += j > 0 ? _offsetx : 0;
                CreateTile(currentoffsetx, currentoffsetz);
            }
            currentoffsetx = 0f;
        }
        SetAdjacentTiles();
        _finishedLevel = true;
    }

    private void CreateTile(float currentoffsetx, float currentoffsetz)
    {
        var tile = Instantiate(_hexTiles, new Vector3(currentoffsetx, 0, currentoffsetz), Quaternion.identity);
        if(_createdTiles == null) _createdTiles = new List<GameObject>();
        _createdTiles.Add(tile);
    }

    private void SetAdjacentTiles()
    {
        foreach(GameObject tile in _createdTiles) 
            tile.GetComponent<HexTile>().GetAdjacentTiles(); 
    }
    public void DestroyLevel()
    {
        _createdTiles.ForEach(x => Destroy(x));
        _createdTiles.Clear();
    }
    
    public void SetHeight(float height)
     => _mapHeight = (int)height;
    public void SetWidth(float width)
     =>  _mapWidth = (int)width; 
}

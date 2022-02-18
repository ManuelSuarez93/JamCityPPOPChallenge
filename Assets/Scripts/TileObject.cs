using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Tiles")]
public class TileObject : ScriptableObject
{  
    [SerializeField] TileType _tileType;
    [SerializeField] GameObject _hexPrefab;
    [SerializeField] Material _tileTexture;
    
    public HexTile CreateTile(float currentoffsetx, float currentoffsetz)
    {
        var tile = Instantiate(_hexPrefab, new Vector3(currentoffsetx, 0, currentoffsetz), Quaternion.identity).GetComponent<HexTile>();
        tile.InitializeTile(_tileType,_tileTexture); 
        return tile;
    }
}

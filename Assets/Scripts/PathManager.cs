using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PathFinding;

public class PathManager : MonoBehaviour
{
    #region Fields
    public static PathManager Instance {get; set;}
    [SerializeField] private Vector3 _moveAmountOnSelectPath;
    [SerializeField] private float _animSpeed;
    private HexTile _firstTile;
    private HexTile _lastTile;
    private IList<IAStarNode> _nodes;
    private bool _isPathEnabled;
    private int _pathTotalCost;
    #endregion

    #region Properties
    public bool IsPathEnabled => _isPathEnabled;
    public IList<IAStarNode> Nodes => _nodes;
    #endregion

    void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }
    void Start() 
    {
        _isPathEnabled = false;
        _pathTotalCost = 0;
    }
    
    #region Path Creation methods
    public void StartPath(HexTile newTile)
    { 
        if(_isPathEnabled && newTile != null)
        {
            if(_firstTile == null)
            {
                UIManager.Instance.SetInfoText("Select end tile");
                _firstTile = newTile; 
                _firstTile.SetEnabled(false);
                _firstTile.SetColor(Color.green);
            }
            else
            { 
                _lastTile = newTile; 
                _lastTile.SetEnabled(false);
                _nodes = (CalculatePath());
                if(_nodes != null) 
                {
                    ShowPath(); 
                    _lastTile.SetColor(Color.magenta);
                    UIManager.Instance.SetInfoText($"Path is correct, {Environment.NewLine}Tiles: <color=blue>{_nodes.Count}</color> {Environment.NewLine} Total cost:<color=green> {_pathTotalCost} </color>");
                }
                else    
                { 
                    UIManager.Instance.SetInfoText("Path is not available");
                    UIManager.Instance.EnableCreatePath(true);
                    ResetPath();
                }
                
                SetPathEnable(false);
            }
        }
    }

    public void ResetPath()
    {  
        SetPathEnable(false); 
        _pathTotalCost = 0;

        if(_nodes != null && _nodes.Count >= 0)
        {
            SetAllTilesColor(Color.white);
            MoveAllTiles(_moveAmountOnSelectPath * -1, _animSpeed);
            SetText(false);
            _nodes.Clear();
        }
        
        _firstTile?.SetEnabled(true);
        _firstTile?.SetColor(Color.white);
        _lastTile?.SetEnabled(true);
        _lastTile?.SetColor(Color.white);
        _firstTile = null;
        _lastTile = null;
    } 

    public  IList<IAStarNode> CalculatePath() => AStar.GetPath(_firstTile, _lastTile);
    private string ShowPath()
    { 
        if(_nodes != null && _nodes.Count <= 0) return "no nodes";
        
        var nodesName = "";
        SetAllTilesColor(Color.red);
        MoveAllTiles(_moveAmountOnSelectPath, _animSpeed);
        SetText(true);

        return nodesName;
    }
    #endregion
    
    #region Tile change methods
    private void SetAllTilesColor(Color color)
    {
        foreach (IAStarNode ia in _nodes)
        {
            var tile = (HexTile)ia;
            if(tile != _firstTile && tile != _lastTile)
                tile.SetColor(color); 
        }
    }
    private void MoveAllTiles(Vector3 moveAmount, float speed)
    {
        foreach (IAStarNode ia in _nodes)
        {
            var tile = (HexTile)ia;
            tile.MoveTile(tile.ModelPosition, tile.ModelPosition + moveAmount, speed );
        }
    }
    private void SetText(bool showScore)
    {
        foreach (IAStarNode ia in _nodes)
        {
            var tile = (HexTile)ia; 
            if(tile != _firstTile)
            { 
                if(showScore)
                {
                    _pathTotalCost += (int)tile.Cost;
                    tile.ShowScore(_pathTotalCost.ToString());  
                }
                else
                    tile.ShowScore("");
            }
            
            
        }
    }
    public void SetPathEnable(bool enable) => _isPathEnabled = enable;
    #endregion
}

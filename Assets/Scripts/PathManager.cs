using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PathFinding;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance {get; set;}
    private HexTile _firstTile;
    private HexTile _lastTile;
    private IList<IAStarNode> _nodes;
    private bool _isPathEnabled;
    public IList<IAStarNode> Nodes => _nodes;
    
    void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }
    void Start() => _isPathEnabled = false;
    
    public void StartPath(HexTile newTile)
    { 
        if(_isPathEnabled && newTile != null)
        {
            if(_firstTile == null)
            {
                UIManager.Instance.SetInfoText("Select end tile");
                _firstTile = newTile; 
                _firstTile.SetEnabled(false);
            }
            else
            { 
                _lastTile = newTile; 
                _lastTile.SetEnabled(false);
                _nodes = (CalculatePath());
                if(_nodes != null) 
                {
                    ShowPath();
                    UIManager.Instance.SetInfoText("Path correct : " +  
                        String.Join(',',  
                        _nodes.Cast<HexTile>().
                        Select(x => $"Tile :{x.name.Split("Tile")[1]}")));
                }
                else    
                    UIManager.Instance.SetInfoText("Path is not possible");
                
                SetPathEnable(false);
            }
        }
    }

    public void ResetPath()
    {
        UIManager.Instance.SetInfoText("Path resetted");
        
        SetPathEnable(false);

        if(_nodes != null && _nodes.Count >= 0)
        {
            SetTileColor(Color.white);
            _nodes.Clear();
        }
        
        _firstTile?.SetEnabled(true); 
        _lastTile?.SetEnabled(true);
        _firstTile = null;
        _lastTile = null;
    } 

    private string ShowPath()
    { 
        if(_nodes != null && _nodes.Count <= 0) return "no nodes";
        
        var nodesName = "";
        SetTileColor(Color.red);

        return nodesName;
    }
    private void SetTileColor(Color color)
    {
        foreach (IAStarNode ia in _nodes)
        {
            var tile = (HexTile)ia;
            tile.SetColor(color);

        }
    }
    public void SetPathEnable(bool enable) => _isPathEnabled = enable;
    public  IList<IAStarNode> CalculatePath() => AStar.GetPath(_firstTile, _lastTile);
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;
using System;

public class PathSelector : MonoBehaviour
{
    public static PathSelector Instance {get; set;}
    private HexTile _firstTile;
    private HexTile _lastTile;
    private IList<IAStarNode> _nodes;
    public IList<IAStarNode> Nodes => _nodes;
    
    void Awake()
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
    public void StartPath(HexTile newTile)
    { 
        if(_firstTile == null)
        {
            _firstTile = newTile;
        }
        else
        { 
            _lastTile = newTile; 
            _nodes = (CalculatePath());
            ShowPath();
        }
    }

    private void ShowPath()
    { 
        if(_nodes == null) return;
        
        foreach(IAStarNode ia in _nodes)
        {
            var tile = (HexTile)ia;
            tile.gameObject.transform.position += new Vector3(0,1,0);
        }
    }

    public  IList<IAStarNode> CalculatePath() => AStar.GetPath(_firstTile, _lastTile);
}

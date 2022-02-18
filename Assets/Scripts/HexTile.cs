using System;
using System.Collections.Generic;
using System.Linq;
using PathFinding;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TileType { Grass = 1, Forest = 3, Desert = 5, Mountain = 10, Water = 0} 
public class HexTile : MonoBehaviour, IAStarNode
{ 
    [SerializeField] private TileType _tileType;  
    [SerializeField] private List<HexTile> _neighbours;
    [SerializeField] private float _adjacentRadius;  
    private float _cost; 
    private bool _isEnabled; 
    public float Cost => _cost;
    public IEnumerable<IAStarNode> Neighbours => _neighbours;

    private void Start()
    { 
        _isEnabled = true;
    } 
    private void OnMouseDown()
    {
        if(_isEnabled)
        { 
            PathSelector.Instance.StartPath(this);
            _isEnabled = false;
        }
    }

    public void GetAdjacentTiles()
    {
        try
        {
            var adj = Physics.OverlapSphere(transform.position, _adjacentRadius);
            foreach(Collider a in adj)
            {
                if(a.GetComponent<HexTile>() && gameObject != a.gameObject)
                    _neighbours.Add(a.GetComponent<HexTile>());
            } 
        }
        catch(Exception e)
        {
            
        }
        
    }
    
    public float CostTo(IAStarNode neighbour)
    {  
        var cost =  _neighbours.FirstOrDefault(x => x == (HexTile)neighbour)._cost;
        Debug.Log($"Cost is: {cost}");
        return cost;
    }

    public float EstimatedCostTo(IAStarNode target)
    {
        float totalCost = 0f; 
        HexTile current = this;
        var tiles = LevelCreator.Instance.CreatedTiles;
        foreach(HexTile tile in tiles)
        {
            try
            { 
                current =  current.Neighbours.FirstOrDefault(x => x.Neighbours.Contains(target)) != null ? 
                    (HexTile)current.Neighbours.FirstOrDefault(x => x.Neighbours.Contains(target)) :
                    (HexTile)current.Neighbours.Cast<HexTile>().OrderBy(x => x.Cost).FirstOrDefault();
                    
                totalCost += current.Cost;  
            }
            catch(Exception e){}

        }
       
        Debug.Log($" Estimated Cost is: {totalCost}");
        return totalCost;
    }
    public void InitializeTile(TileType newType, Material newTexture)
    {  
        _tileType = newType;
        GetComponentInChildren<MeshRenderer>().material = newTexture;
        _cost = (int)_tileType;
    } 
}
 

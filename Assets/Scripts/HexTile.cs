using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathFinding;
using TMPro;
using UnityEngine;

public enum TileType { Grass = 1, Forest = 3, Desert = 5, Mountain = 10, Water = -1} 
public class HexTile : MonoBehaviour, IAStarNode
{ 
    [SerializeField] private TileType _tileType;  
    [SerializeField] private List<HexTile> _neighbours;
    [SerializeField] private float _adjacentRadius;  
    [SerializeField] private MeshRenderer _model;
    [SerializeField] private TextMeshProUGUI _text;
    private float _cost; 
    private bool _isEnabled; 
    public float Cost => _cost;
    public IEnumerable<IAStarNode> Neighbours => _neighbours;
    public TileType TileType => _tileType;
    private float _totalCost;

    private void Start()
    { 
        _isEnabled = true; 
        if(!_model) _model = GetComponentInChildren<MeshRenderer>();
    } 
    private void OnMouseDown()
    {
        if(_isEnabled) 
        {
            if((int)_tileType != -1)
                PathManager.Instance.StartPath(this); 
            else
                UIManager.Instance.SetInfoText($"The type of tile {_tileType.ToString()} cannot be used in the path");
        }
             
    }

    public void GetAdjacentTiles()
    { 
        var adj = Physics.OverlapSphere(transform.position, _adjacentRadius);
        if(adj != null)
        {
            foreach(Collider a in adj)
            {
                var t = a.GetComponent<HexTile>();
                if(t != null && gameObject != a.gameObject && t.TileType != TileType.Water)
                    _neighbours.Add(t); 
            }
        }
        
    }
    
    public float CostTo(IAStarNode neighbour)
    {  
        var cost =  _neighbours.FirstOrDefault(x => x == (HexTile)neighbour)._cost;
        Debug.Log($"Cost is: {cost}");
        return  _neighbours.FirstOrDefault(x => x == (HexTile)neighbour)._cost;;
    }

    public float EstimatedCostTo(IAStarNode target)
    { 
        StartCoroutine(Search(target));  

        return _totalCost;
    }
 

    public void InitializeTile(TileType newType, Material newTexture,int tileNumber)
    {  
        _tileType = newType;
        GetComponentInChildren<MeshRenderer>().material = newTexture;
        _cost = (int)_tileType;
        _neighbours = new List<HexTile>();
        name = $"Tile{tileNumber}";
    }

    private IEnumerator Search(IAStarNode target)
    { 
        _totalCost = 0f; 
        HexTile current = this;
        bool finishedSearch = false; 
        var targetTile = (HexTile)target;

        while (!finishedSearch)
        {
            if (current.Neighbours.FirstOrDefault(x => x.Neighbours.Contains(target)) != null)
            {
                current = (HexTile)current.Neighbours.FirstOrDefault(x => x.Neighbours.Contains(target));
                finishedSearch = true;
            }
            else
            {
                current = current.Neighbours.Cast<HexTile>().OrderBy(x => Vector3.Distance(x.transform.position, targetTile.transform.position)).FirstOrDefault();
            }
            _totalCost += current.Cost;
            yield return null;
        }
 
        Debug.Log($" Estimated Cost from {name} to {targetTile.name} is: {_totalCost}");
    }

    public void MoveTile(Vector3 currentPos, Vector3 newPos, float amount) => StartCoroutine(LerpMove(currentPos, newPos, amount));
    private IEnumerator LerpMove(Vector3 currentPos, Vector3 newPos, float amount)
    {
        var timer = 0f;
        while(timer <= amount)
        {
            transform.position = Vector3.Lerp(currentPos, newPos, timer/amount);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.05f);
        }
        transform.position = newPos; 
    }
    public void SetEnabled(bool enable) => _isEnabled = enable;
    public void SetColor(Color color)
    {
        if(_model != null) _model.material.color = color;
    }
}
 

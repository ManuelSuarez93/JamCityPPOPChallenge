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
    #region Fields 
    [SerializeField] private TileType _tileType;  
    [SerializeField] private List<HexTile> _neighbours;
    [SerializeField] private float _adjacentRadius;  
    [SerializeField] private MeshRenderer _model;
    [SerializeField] private TextMeshPro _text; 
    private float _cost; 
    private bool _isEnabled; 
    private float _estimatedCost;
    #endregion

    #region Properties
    public float Cost => _cost;
    public IEnumerable<IAStarNode> Neighbours => _neighbours;
    public TileType TileType => _tileType;
    public Vector3 ModelPosition => _model.transform.position; 
    #endregion

    #region OnMouse events
    private void OnMouseDown()
    {
        if(CanUseForPath()) 
        {
            if((int)_tileType != -1)
                PathManager.Instance.StartPath(this); 
            else
                UIManager.Instance.SetInfoText($"The type of tile {_tileType.ToString()} cannot be used in the path");
        }   
    }

    private void OnMouseEnter()  
    {
        if(CanUseForPath())
            SetColor(Color.blue);
    }  
    private void OnMouseExit()
    { 
        if(CanUseForPath())
            SetColor(Color.white);
    }
    #endregion

    
    #region AStar methods
    public float CostTo(IAStarNode neighbour) 
        => _neighbours.FirstOrDefault(x => x == (HexTile)neighbour)._cost;

    public float EstimatedCostTo(IAStarNode target)
    { 
        StartCoroutine(Search(target));  

        return _estimatedCost;
    }
    #endregion

    #region Class methods 
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
    public void InitializeTile(TileType newType, Material newTexture,int tileNumber)
    {  
        _tileType = newType;
        GetComponentInChildren<MeshRenderer>().material = newTexture;
        _cost = (int)_tileType;
        _neighbours = new List<HexTile>();
        _isEnabled = true; 
        _estimatedCost = 0;
        if(!_model) _model = GetComponentInChildren<MeshRenderer>();
        if(!_text) _text = GetComponentInChildren<TextMeshPro>();

        name = $"Tile{tileNumber}";
    }
    private IEnumerator Search(IAStarNode target)
    { 
        _estimatedCost = 0f; 
        HexTile current = this;
        bool finishedSearch = false; 
        var targetTile = (HexTile)target;

        while (!finishedSearch)
        {
            if(current.Neighbours.Count() > 0)
            {
                if (current.Neighbours?.FirstOrDefault(x => x.Neighbours.Contains(target)) != null)
                {
                    current = (HexTile)current.Neighbours.FirstOrDefault(x => x.Neighbours.Contains(target));
                    finishedSearch = true;
                }
                else
                {
                    current = current.Neighbours?
                    .Cast<HexTile>()
                    .OrderBy(x => Vector3.Distance(x.transform.position, targetTile.transform.position))
                    .FirstOrDefault();
                }
            }

            if(current)
                _estimatedCost += current.Cost;

            yield return null;
        }  
    }

    private IEnumerator LerpMove(Vector3 currentPos, Vector3 newPos, float amount)
    {
        var timer = 0f; 
        while(timer <= amount)
        {
            var t = timer/amount; 
            _model.transform.position = Vector3.Lerp(currentPos, newPos, t);
            timer += Time.deltaTime; 
            yield return null;
        }
        _model.transform.position = newPos; 
    }

    private bool CanUseForPath() => _isEnabled && PathManager.Instance.IsPathEnabled;
    public void MoveTile(Vector3 currentPos, Vector3 newPos, float amount) 
        => StartCoroutine(LerpMove(currentPos, newPos, amount));
    public void SetEnabled(bool enable) => _isEnabled = enable;
    public void SetColor(Color color)
    {
        if(_model) _model.material.color = color;
    }
    public void ShowScore(string newText)
    {
        if(_text) _text.text = newText;
    }

    #endregion
}
 

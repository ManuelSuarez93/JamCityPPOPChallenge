using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathFinding;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TileType { Grass, Forest, Desert, Mountain, Water} 
public class HexTile : MonoBehaviour, IAStarNode
{ 
    [SerializeField] private TileType _tileType; 
    [SerializeField] private Transform _hex;
    [SerializeField] private List<HexTile> _neighbours;
    [SerializeField] private float _adjacentRadius;
    [SerializeField]  float _cost;
    
    private float _timer;
    private bool _isSelected;
    private bool _isEnabled; 
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
   
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _adjacentRadius);
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

        if(current.Neighbours.FirstOrDefault(x => x.Neighbours.Contains(target)) != null)
        { 
            current =  (HexTile)current.Neighbours.FirstOrDefault(x => x.Neighbours.Contains(target));
            totalCost += current._cost;
        }
        else
        { 
            current = _neighbours.OrderBy(x => x._cost).FirstOrDefault();
            totalCost += current._cost;
        }

        Debug.Log($" Estimated Cost is: {totalCost}");
        return totalCost;
    }

     // private void OnMouseEnter()
    // { 
    //     StopAllCoroutines();

    //     if(!isSelected)
    //         StartCoroutine(DoLerp(1f, new Vector3(_hex.position.x, _hex.position.y + 1))); 

    //     isSelected = true;
    // }
    // private void OnMouseExit()
    // { 
    //     StopAllCoroutines();

    //     if(isSelected)
    //         StartCoroutine(DoLerp(1f, new Vector3(_hex.position.x, _hex.position.y - 1)));

    //     isSelected = false;
    // }
    // IEnumerator DoLerp(float amount, Vector3 position)
    // { 
    //     _timer = 0;
    //     while(_timer < amount)
    //     {
    //         float t = _timer / amount;
    //         t = t * t * (3f - 2f * t);
    //         _hex.position = Vector3.Lerp(_hex.position, position * amount, _timer/amount);
            
    //         _timer += Time.deltaTime;
    //         yield return null; 
    //     }
    //     _hex.position = position * amount;
        
    // }
}
 

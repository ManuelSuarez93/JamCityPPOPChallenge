using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;

public class PathSelector : MonoBehaviour
{
    public static PathSelector Instance {get; set;}
    HexTile firstTile;
    HexTile lastTile; 
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
        
        if(firstTile == null)
        {
            firstTile = newTile;
        }
        else
        {
            if(lastTile == null) 
                lastTile = newTile; 
            else 
                CalculatePath(); 
        }
    }

    public  void CalculatePath() => Debug.Log(AStar.GetPath(firstTile, lastTile).Count);
}

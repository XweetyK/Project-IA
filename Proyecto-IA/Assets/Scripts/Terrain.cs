using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    public enum TerrainType { Land, Gravel }

    [SerializeField] TerrainType type = TerrainType.Land;

    public TerrainType Type{
        get{return type; }
        set{ type = value; }
    }
}

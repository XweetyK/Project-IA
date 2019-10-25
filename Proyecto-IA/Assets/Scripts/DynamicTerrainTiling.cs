using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DynamicTerrainTiling : MonoBehaviour
{
    [SerializeField] float tileScaleToObjectScale = 0.5f;
    public void updateTiling(){
        foreach (Transform child in transform) {
            GameObject obj = child.gameObject;
            if (obj.layer == 9) {
                float xScale = obj.transform.localScale.x / tileScaleToObjectScale;
                float zScale = obj.transform.localScale.z / tileScaleToObjectScale;
                Vector2 vec = new Vector2(xScale, zScale);
                obj.GetComponent<Renderer>().sharedMaterial.mainTextureScale = vec;
            }
        }

    }
}

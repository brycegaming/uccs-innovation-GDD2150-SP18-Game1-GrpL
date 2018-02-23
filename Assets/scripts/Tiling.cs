using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour
{
    private GameObject wallTile;

    private Vector2 tileStart;
    public Vector2 numTiles;
    public Vector2 imageSize;
    public Vector2 imageScale;

    void Awake()
    {
    }
    
	// Use this for initialization
	void Start ()
	{
	    tileStart = this.transform.position;
	    
	    wallTile = new GameObject("wallTile:");
	    SpriteRenderer spriteRenderer = wallTile.AddComponent<SpriteRenderer>() as SpriteRenderer;
	    spriteRenderer.sprite = this.GetComponent<SpriteRenderer>().sprite;
	    wallTile.transform.position = tileStart;
	    wallTile.transform.localScale = imageScale;

	    for (int i = 0; i < numTiles.y; i++)
	    {
	        for (int j = 0; j  < numTiles.x; j++)
	        {
	            wallTile.transform.position += new Vector3(imageSize.x, 0, 0);
	            GameObject.Instantiate(wallTile);
	        }  
	        wallTile.transform.position = new Vector3(tileStart.x, wallTile.transform.position.y - imageSize.y, 0);
	    }
	    
	    GameObject.Destroy(wallTile);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

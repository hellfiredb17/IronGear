using System.Collections.Generic;
using UnityEngine;
using HexWorld;

public class HexBoardCreator : MonoBehaviour
{
    //---- Variables
    //--------------
    public HexTilePreferences Preferences;

    public HexBoardModel Model;
    public HexBoardView View;

    public List<HexTile> HexTiles;

    //---- Public
    //-----------
    public void CreateBoard(int cols, int rows, Vector3 scale)
    {
        int size = cols * rows;

        // Set Model data
        Model.Size = new Point2(cols, rows);        

        // Create tiles
        float w = Preferences.SizeX * scale.x;
        float h = Preferences.SizeY * scale.z;
        int col = 0;
        int row = 0;
        float offset = 0;
        bool up = false;

        MeshRenderer lowerBound = null;
        MeshRenderer uppperBound = null;

        HexTiles = new List<HexTile>(size);
        for (int i = 0; i < size; i++)
        {
            if(col >= cols)
            {
                offset += (h * 2.0f);
                col = 0;
                ++row;
            }
            
            // Create tile
            HexTile tile = Instantiate<HexTile>(Preferences.Prefab, View.HexTileTransform, false);
            if (!up)
            {
                tile.transform.localPosition = new Vector3(col * w, 0, offset);
            }
            else
            {
                tile.transform.localPosition = new Vector3(col * w, 0, offset + h);
            }

            tile.transform.localScale = scale;
            tile.name = "Hex_" + col + "_" + row;

            // Set Tile data
            tile.SetModelData();

            // Add tile to list / model
            HexTiles.Add(tile);
            Model.AddHexTile(tile.Model);
            
            // Grab lower and upper bound
            if(i == 0)
            {
                lowerBound = tile.View.Renderer;
            }
            else if (i == size - 1)
            {
                uppperBound = tile.View.Renderer;
            }

            up = !up;
            ++col;
        }

        // Center board
        if (lowerBound && uppperBound)
        {
            Vector3 lowerLeft = lowerBound.bounds.min;
            Vector3 upperRight = uppperBound.bounds.max;
            Vector3 center = lowerLeft - (upperRight * .5f);
            center.y = View.HexTileTransform.position.y;
            View.HexTileTransform.position = center;
        }
    }

    public void CreateBoardWithData(HexBoardModel boardModel)
    {
        // TODO
    }

    public void ClearBoard()
    {
        // Remove gameobjects
        for(int i = 0; i < HexTiles.Count; i++)
        {
            if (!HexTiles[i])
                continue;
            DestroyImmediate(HexTiles[i].gameObject);
        }
        HexTiles.Clear();

        // Clear data
        Model.Clear();
    }    
}

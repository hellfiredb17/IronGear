using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexWorld
{
    /// <summary>
    /// Controller/Logic for Hexboard
    /// </summary>
    [System.Serializable]
    public class HexBoardController
    {
        //---- Variables
        //--------------
        public HexBoard HexBoard;

        //---- Public
        //-----------
        public void CreateBoard()
        {
            HexBoardModel model = HexBoard.Model;
            HexBoardView view = HexBoard.View;
            HexTilePreferences preferences = HexBoard.Preferences;

            view.HexTiles = new List<HexTile>(model.HexTileCount);
            for(int i = 0; i < model.HexTileCount; i++)
            {
                HexTile hexTile = Object.Instantiate(preferences.Prefab, view.Root);
                HexTileModel hexTileModel = model.HexTileModels[i];                
                hexTile.name = "Tile_" + i;
                hexTile.Model = hexTileModel;
                hexTile.View.SetMaterial(preferences.Materials[hexTileModel.MaterialName]);
                hexTile.View.SetTexture(preferences.Textures[hexTileModel.TextureName]);
                hexTile.transform.localPosition = hexTileModel.Position.Convert();
                hexTile.transform.localRotation = Quaternion.Euler(hexTileModel.Rotation.Convert());
                hexTile.transform.localScale = hexTileModel.Scale.Convert();
                view.HexTiles.Add(hexTile);
            }
        }
    } // end class
} // end namespace

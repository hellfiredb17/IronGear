using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Main View for a Hex Tile
/// </summary>
namespace HexWorld
{
    [RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider))]
    public class HexTileView : MonoBehaviour
    {
        //---- Variables
        //--------------
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshCollider _meshCollider;

        //---- Properties
        //---------------
        public MeshRenderer Renderer => _meshRenderer;
        public MeshCollider Collider => _meshCollider;
        public string MaterialName => _meshRenderer.sharedMaterial.name;
        public string TextureName => _meshRenderer.sharedMaterial.GetTexture("_MainTex").name;
        public bool IsColliderOn => _meshCollider.enabled;

        //---- Functions
        //--------------
        public void EnableCollider(bool value)
        {
            _meshCollider.enabled = value;
        }

        public void SetMaterial(Material mat)
        {
            _meshRenderer.material = Instantiate<Material>(mat);
        }

        public void SetSharedMaterial(Material material)
        {
            _meshRenderer.sharedMaterial = material;
        }

        public void SetTexture(Texture texture)
        {
            _meshRenderer.material.SetTexture("_MainTex", texture);
        }

        public void SetSharedTexture(Texture texture)
        {
            _meshRenderer.sharedMaterial.SetTexture("_MainTex", texture);
        }
    }
} // end namespace

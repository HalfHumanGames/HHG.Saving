using HHG.Common.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HHG.Saving.Runtime
{
    [RequireComponent(typeof(Saver), typeof(TilemapExporter))]
    public class SaveTilemap : MonoBehaviour, ISavable
    {
        private Lazy<TilemapExporter> _exporter = new Lazy<TilemapExporter>();
        private TilemapExporter exporter => _exporter.FromComponent(this);

        [System.Serializable]
        public class Data : SavableData
        {
            public List<string> Tiles = new List<string>();
            public List<SerializableTilemap> Tilemaps = new List<SerializableTilemap>();
        }

        public SavableData Save()
        {
            TilemapAsset tilemap = ScriptableObject.CreateInstance<TilemapAsset>();
            exporter.Save(tilemap);
            Data data = new Data
            {
                Tiles = tilemap.Tiles.Select(t => AssetRegistry.GetGuid(t)).ToList(),
                Tilemaps = tilemap.Tilemaps.ToList()
            };
            Destroy(tilemap);
            return data;
        }

        public void Load(SavableData saveData)
        {
            Data data = saveData as Data;
            TilemapAsset tilemap = ScriptableObject.CreateInstance<TilemapAsset>();
            tilemap.Initiialize(data.Tiles.Select(t => AssetRegistry.GetAsset<TileBase>(t)), data.Tilemaps);
            exporter.Load(tilemap);
            Destroy(tilemap);
        }
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Temp.Match2D {
    [CreateAssetMenu(fileName = "BlockSpawnerTile", menuName = "Tile/Block Spawner Tile")]
    public class BlockSpawnerTile : TileBase {
        public Sprite EditorPreviewSprite;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = !Application.isPlaying ? EditorPreviewSprite : null;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif
            //Board.RegisterSpawner(position);
            return base.StartUp(position, tilemap, go);
        }
    }
}

using Unit.GameScene.Units.Blocks.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Temp.Match2D {
    [CreateAssetMenu(fileName = "BlockPlacerTile", menuName = "Tile/Block Placer Tile")]
    public class BlockPlacerTile : TileBase {
        public Sprite PreviewEditorSprite;
        [FormerlySerializedAs("placedMatchMatchBlockView")] [FormerlySerializedAs("placedBlockView")] [FormerlySerializedAs("PlacedBlock")] [Tooltip("If null this will be a random gem")]
        public MatchBlockView placedMatchBlockView = null;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = !Application.isPlaying ? PreviewEditorSprite : null;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif
            //Board.RegisterCell(position, PlacedBlock);
            return base.StartUp(position, tilemap, go);
        }
    }
}
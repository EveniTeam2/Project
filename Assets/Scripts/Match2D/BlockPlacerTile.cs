using UnityEngine;
using UnityEngine.Tilemaps;

namespace Match2D {
    [CreateAssetMenu(fileName = "GemPlacerTile", menuName = "Tile/Gem Placer")]
    public class BlockPlacerTile : TileBase {
        public Sprite PreviewEditorSprite;
        [Tooltip("If null this will be a random gem")]
        //public Tile PlacedGem = null;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            //When no playing (so in editor outside of play mode) return the given preview Sprite, otherwise return null 
            //wo that tile is "invisible" during play (the gem are gameobject handled by our system, not the tilemap)
            tileData.sprite = !Application.isPlaying ? PreviewEditorSprite : null;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif
            //This tile is only used in editor to help design the level. At runtime, we notify the board that this tile is
            //a place for a gem, then delete the GameObject that was just visual aid at design time. The Board will take care
            //of creating a gem there.
            //Board.RegisterCell(position, PlacedGem);

            return base.StartUp(position, tilemap, go);
        }
    }
}
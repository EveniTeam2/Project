using Core.Utils;
using Unit.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    public class BlockPool : CustomPool<Block>
    {
        public BlockPool(Block prefab, Transform root, int size, bool isFlexible)
            : base(prefab, root, OnCreate, OnGet, OnRelease, OnDestroy, size, isFlexible) { }

        private static void OnCreate(Block block, CustomPool<Block> blockPool)
        {
            block.gameObject.SetActive(false);
        }

        private static void OnGet(Block block)
        {
            block.gameObject.SetActive(true);
        }

        private static void OnRelease(Block block)
        {
            block.gameObject.SetActive(false);
        }

        private static void OnDestroy(Block block)
        {
            Object.Destroy(block.gameObject);
        }
    }
}
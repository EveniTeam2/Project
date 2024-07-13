using Core.Utils;
using Unit.Boards.Blocks;
using Unit.Boards.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Unit.Boards
{
    /// <summary>
    /// 블록 풀링을 관리하는 클래스입니다.
    /// </summary>
    public class BlockPool : CustomPool<Block>, IBlockPool
    {
        public BlockPool(Block prefab, Transform root, int size, bool isFlexible)
            : base(prefab, root, OnCreate, OnGet, OnRelease, OnDestroy, size, isFlexible){ }

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
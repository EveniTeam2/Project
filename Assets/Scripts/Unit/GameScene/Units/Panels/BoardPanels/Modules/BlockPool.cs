using Core.Utils;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Panels.BoardPanels.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Panels.BoardPanels.Modules
{
    /// <summary>
    ///     블록 풀링을 관리하는 클래스입니다.
    /// </summary>
    public class BlockPool : CustomPool<BlockView>, IBlockPool
    {
        public BlockPool(BlockView prefab, Transform root, int size, bool isFlexible)
            : base(prefab, root, OnCreate, OnGet, OnRelease, OnDestroy, size, isFlexible)
        {
        }

        private static void OnCreate(BlockView blockView, CustomPool<BlockView> blockPool)
        {
            blockView.gameObject.SetActive(false);
        }

        private static void OnGet(BlockView blockView)
        {
            blockView.gameObject.SetActive(true);
        }

        private static void OnRelease(BlockView blockView)
        {
            blockView.gameObject.SetActive(false);
        }

        private static void OnDestroy(BlockView blockView)
        {
            Object.Destroy(blockView.gameObject);
        }
    }
}
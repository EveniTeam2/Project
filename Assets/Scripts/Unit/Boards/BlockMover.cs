using System;
using System.Collections;
using Unit.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    public class BlockMover
    {
        private readonly WaitForSeconds _swapDelay = new(0.2f);
        private readonly float _duration;

        public BlockMover(float duration)
        {
            _duration = duration;
        }

        public IEnumerator SwapBlock(Block currentBlock, Block targetBlock, Tuple<float, float> currentPos, Tuple<float, float> targetPos)
        {
            var currentBlockStartPos = currentBlock.transform.position;
            var targetBlockStartPos = targetBlock.transform.position;
            var elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                currentBlock.transform.position = Vector3.Lerp(currentBlockStartPos, new Vector3(currentPos.Item1, currentPos.Item2, 0), elapsedTime / _duration);
                targetBlock.transform.position = Vector3.Lerp(targetBlockStartPos, new Vector3(targetPos.Item1, targetPos.Item2, 0), elapsedTime / _duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.position = new Vector3(currentPos.Item1, currentPos.Item2, 0);
            targetBlock.transform.position = new Vector3(targetPos.Item1, targetPos.Item2, 0);

            yield return _swapDelay;
        }

        public IEnumerator DropBlock(Tuple<float, float> targetPos, float adjustSpeed, Block currentBlock)
        {
            var currentBlockStartPos = currentBlock.transform.position;
            var elapsedTime = 0f;

            while (elapsedTime < _duration * adjustSpeed)
            {
                currentBlock.transform.position = Vector3.Lerp(currentBlockStartPos, new Vector3(targetPos.Item1, targetPos.Item2, 0), elapsedTime / _duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentBlock.transform.position = new Vector3(targetPos.Item1, targetPos.Item2, 0);
        }
    }
}
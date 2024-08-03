using System;
using System.Collections;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Panels.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Panels.Modules.BoardModules
{
    /// <summary>
    ///     블록의 이동을 처리하는 클래스입니다.
    /// </summary>
    public class MatchMatchBlockMover : IMatchBlockMover
    {
        private readonly float _blockGap;
        private readonly float _bounceDuration;
        private readonly float _bounceHeight;
        private readonly float _dropDurationPerUnit;
        private readonly float _duration;
        private readonly MonoBehaviour _monoBehaviour;
        private readonly WaitForSeconds _swapDelay;

        public MatchMatchBlockMover(float duration, float dropDurationPerUnit, float bounceHeight, float bounceDuration,
            WaitForSeconds progressTime, float blockGap, MonoBehaviour monoBehaviour)
        {
            _swapDelay = progressTime;
            _duration = duration;
            _bounceHeight = bounceHeight * blockGap;
            _bounceDuration = bounceDuration * blockGap;
            _dropDurationPerUnit = dropDurationPerUnit;
            _blockGap = blockGap;
            _monoBehaviour = monoBehaviour;
        }

        /// <summary>
        ///     두 블록을 스왑합니다.
        /// </summary>
        /// <param name="currentMatchBlockView">현재 블록</param>
        /// <param name="targetMatchBlockView">목표 블록</param>
        /// <param name="currentPos">현재 블록의 위치</param>
        /// <param name="targetPos">목표 블록의 위치</param>
        public IEnumerator SwapBlock(BlockView currentMatchBlockView, BlockView targetMatchBlockView, Tuple<float, float> currentPos,
            Tuple<float, float> targetPos)
        {
            var currentBlockStartPos = currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition;
            var targetBlockStartPos = targetMatchBlockView.GetComponent<RectTransform>().anchoredPosition;
            var elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(currentBlockStartPos,
                    new Vector3(currentPos.Item1, currentPos.Item2, 0), elapsedTime / _duration);
                targetMatchBlockView.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(targetBlockStartPos,
                    new Vector3(targetPos.Item1, targetPos.Item2, 0), elapsedTime / _duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(currentPos.Item1, currentPos.Item2, 0);
            targetMatchBlockView.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(targetPos.Item1, targetPos.Item2, 0);

            yield return _swapDelay;
        }

        /// <summary>
        ///     블록을 목표 위치로 떨어뜨립니다.
        /// </summary>
        /// <param name="targetPos">목표 위치</param>
        /// <param name="currentMatchBlockView">현재 블록</param>
        public IEnumerator DropBlock(Tuple<float, float> targetPos, BlockView currentMatchBlockView)
        {
            var currentBlockStartPos = currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition;
            var targetPosition = new Vector3(targetPos.Item1, targetPos.Item2, 0);
            var distance = Vector3.Distance(currentBlockStartPos, targetPosition);
            var elapsedTime = 0f;

            while (elapsedTime < distance * _dropDurationPerUnit)
            {
                currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(currentBlockStartPos,
                    targetPosition, elapsedTime / (distance * _dropDurationPerUnit));
                elapsedTime += Time.deltaTime * _blockGap;
                yield return null;
            }

            currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition = targetPosition;

            yield return _monoBehaviour.StartCoroutine(BounceBlock(targetPosition, currentMatchBlockView));
        }

        /// <summary>
        ///     블록이 바닥에 떨어진 후 튕기는 애니메이션을 처리합니다.
        /// </summary>
        /// <param name="targetPosition">목표 위치</param>
        /// <param name="currentMatchBlockView">현재 블록</param>
        private IEnumerator BounceBlock(Vector3 targetPosition, BlockView currentMatchBlockView)
        {
            var elapsedTime = 0f;
            var boundDuration = _bounceDuration / 2;
            var bounceTargetPosition = targetPosition + Vector3.up * _bounceHeight;

            while (elapsedTime < boundDuration)
            {
                currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(targetPosition,
                    bounceTargetPosition, elapsedTime / _bounceDuration);
                elapsedTime += Time.deltaTime * _blockGap;
                yield return null;
            }

            currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition = bounceTargetPosition;

            elapsedTime = 0f;
            while (elapsedTime < boundDuration)
            {
                currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(bounceTargetPosition,
                    targetPosition, elapsedTime / _bounceDuration);
                elapsedTime += Time.deltaTime * _blockGap;
                yield return null;
            }

            currentMatchBlockView.GetComponent<RectTransform>().anchoredPosition = targetPosition;
        }
    }
}
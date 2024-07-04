using ScriptableObjects.Scripts.Blocks;
using System;
using TMPro;
using Unit.Blocks.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Unit.Blocks
{
    public abstract class Block : MonoBehaviour, IDraggable
    {
        public event Action<Vector3> OnMatchCheck;

        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TextMeshPro text;

        public BlockType Type { get; private set; }

        private Vector3 startPosition;
        private Vector3 offset;

        private Camera mainCamera;
        private InputAction clickAction;
        private InputAction dragAction;

        private void Awake()
        {
            mainCamera = Camera.main;

            var inputActions = new InputActionMap("BlockActions");
            clickAction = inputActions.AddAction("Click", binding: "<Pointer>/press");
            dragAction = inputActions.AddAction("Drag", binding: "<Pointer>/position");
            
            clickAction.started += OnBeginDrag;
            clickAction.canceled += OnEndDrag;
            dragAction.performed += OnDrag;

            inputActions.Enable();
        }

        public void Initialize(NewBlock info, Action<Vector3> matchCheckHandler)
        {
            text.text = info.text;
            sprite.color = info.color;
            Type = info.type;
            OnMatchCheck += matchCheckHandler;
        }

        public void OnBeginDrag(InputAction.CallbackContext context)
        {
            if (mainCamera == null) return;

            startPosition = transform.position;
            offset = startPosition - mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        public void OnDrag(InputAction.CallbackContext context)
        {
            if (mainCamera == null) return;

            var newPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + offset;
            transform.position = new Vector3(newPos.x, newPos.y, startPosition.z);
        }

        public void OnEndDrag(InputAction.CallbackContext context)
        {
            var endPosition = transform.position;
            transform.position = startPosition;
            OnMatchCheck?.Invoke(endPosition);
        }
    }
}
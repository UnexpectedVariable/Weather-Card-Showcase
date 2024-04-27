using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Second_Scene
{
    internal class SwipeDetector : MonoBehaviour
    {
        [SerializeField]
        private InputAction _press = null;
        [SerializeField]
        private InputAction _position = null;
        [SerializeField]
        private RectTransform _activeArea = null;

        private Rect _screenRect;

        public event EventHandler<Vector2> OnSwipeStarted = null;
        public event EventHandler<Vector2> OnSwipe = null;
        public event EventHandler OnSwipeFinished = null;

        private Vector2 _lastPosition = Vector2.zero;

        private void Awake()
        {
            Debug.Log("SwipeDetector awakened");
            InitializeScreenRect();
            InitializeInputActions();
        }

        private void InitializeInputActions()
        {
            _press.Enable();

            _press.started += (context) =>
            {
                Debug.Log($"Mouse press performed");
                _position.Enable();
                _lastPosition = _position.ReadValue<Vector2>();
            };
            _press.canceled += (context) =>
            {
                Debug.Log($"Mouse press canceled");
                _position.Disable();
                OnSwipeFinished?.Invoke(this, EventArgs.Empty);
            };

            _position.performed += (context) =>
            {
                PerformSwipe();
            };
        }

        private void InitializeScreenRect()
        {
            if (_activeArea == null) _activeArea = GetComponent<RectTransform>();
            var corners = new Vector3[4];
            _activeArea.GetWorldCorners(corners);
            
            _screenRect = new Rect(corners[0], corners[2]);
        }

        private void PerformSwipe()
        {
            var newPosition = _position.ReadValue<Vector2>();
            if (!_screenRect.Contains(newPosition))
            {
                return;
            }

            var direction = newPosition - _lastPosition;
            _lastPosition = newPosition;
            if (direction.x == 0) return;

            OnSwipe?.Invoke(this, direction);
        }

        private void OnDisable()
        {
            _press.Disable();
        }

        private void OnEnable()
        {
            _press.Enable();
        }
    }
}

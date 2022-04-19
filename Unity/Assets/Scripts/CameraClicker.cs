using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace clicker
{
    public class CameraClicker : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _maxDistance = 10f;
        [SerializeField] private UnityEvent _onClickSuccess;

        private RaycastHit[] _hits;

        protected void Awake()
        {
            _hits = new RaycastHit[1];
        }

        protected void Update()
        {
            if (!Input.GetMouseButtonDown(0) || UIManager.Instance.IsPointerOverUI())
            {
                return;
            }
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            int hitsAmount = Physics.RaycastNonAlloc(ray, _hits, _maxDistance, _layerMask.value);
            if (hitsAmount > 0)
            {
                _onClickSuccess?.Invoke();
            }

            else
            {
                UIManager.Instance.HideUpgradeMenu();
            }
        }



    }
}

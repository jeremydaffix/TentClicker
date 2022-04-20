using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace clicker
{
    /// <summary>
    /// Composant permettant de gérer les clics sur la tente (pour gagner des ressources) et en dehors de l'UI.
    /// </summary>
    public class CameraClicker : MonoBehaviour
    {
        [SerializeField] private Camera _camera; // caméra principale
        [SerializeField] private LayerMask _layerMask; // layer des objets clickables
        [SerializeField] private float _maxDistance = 10f; // distance maximum du raycast
        [SerializeField] private UnityEvent _onClickSuccess; // événement à appeler lors d'un clic réussi

        private RaycastHit[] _hits;

        protected void Awake()
        {
            _hits = new RaycastHit[1];
        }

        protected void Update()
        {
            // bouton gauche de la souris pas appuyé, ou pointeur sur l'UI : on sort
            if (!Input.GetMouseButtonDown(0) || UIManager.Instance.IsPointerOverUI())
            {
                return;
            }

            // lancer de rayon à partir du pointeur de la souris
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            int hitsAmount = Physics.RaycastNonAlloc(ray, _hits, _maxDistance, _layerMask.value);
            if (hitsAmount > 0) // on a touché au moins un objet du bon layer : récupération des ressources
            {
                _onClickSuccess?.Invoke();
            }

            // clic ni sur l'UI, ni sur la tente : fermeture des menus / popup
            else
            {
                UIManager.Instance.HideUpgradeMenu();
                UIManager.Instance.HidePopup();
            }
        }
    }
}

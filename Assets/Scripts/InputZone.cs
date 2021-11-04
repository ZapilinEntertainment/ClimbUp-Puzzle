using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ClimbUpPuzzle
{
    public sealed class InputZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public Vector3 TargetTouchPosition { get; private set; }
        public bool FollowTouchPoint => _raycasting & _legitHitPosition;
        private Camera _cam;
        private ControlDisk _activatedControlDisk = null;
        private RaycastHit _raycastHit;
        private bool _legitHitPosition = false, _raycasting = false, _anyDiskControlled = false;
        private const string BACKWALL_TAG = "Backwall", DISK_TAG = "GameController";

        public void Prepare(CameraController cc)
        {
            _cam = cc.Camera;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _raycasting = true;
            Raycast(eventData.position);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
           if (_raycasting) Raycast(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _raycasting = false;
            ClearControlDiskData();
        }

        private void ClearControlDiskData()
        {
            if (_anyDiskControlled)
            {
                _activatedControlDisk.Deactivate();
                _activatedControlDisk = null;
                _anyDiskControlled = false;
            }
        }

        private void Raycast(Vector3 position)
        {
            if (Physics.Raycast(_cam.ScreenPointToRay(position), out _raycastHit))
            {
                var collider = _raycastHit.collider;
                if (collider.CompareTag(DISK_TAG))
                {
                    _legitHitPosition = true;
                    if (_anyDiskControlled)
                    {
                        if (_activatedControlDisk.gameObject != collider.gameObject)
                        {
                            _activatedControlDisk.Deactivate();
                            ActivateDisk();
                        }
                    }
                    else ActivateDisk();
                    _legitHitPosition = true;
                    TargetTouchPosition = _raycastHit.point;

                    void ActivateDisk()
                    {
                        _activatedControlDisk = collider.GetComponent<ControlDisk>();
                        _anyDiskControlled = true;
                        _activatedControlDisk.Activate();
                    }
                }
                else
                {
                    if (collider.CompareTag(BACKWALL_TAG))
                    {
                        _legitHitPosition = true;
                        TargetTouchPosition = _raycastHit.point;
                    }
                }
            }
            else ClearControlDiskData();
        }       
    }
}

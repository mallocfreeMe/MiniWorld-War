using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Character
{
    public class Player : MonoBehaviour
    {
        // camera movement 
        private Camera _camera;
        private Vector3 _originDragPos;
        private Vector3 _dragPosDifference;
        private bool _isDragged;

        // select units 
        public Transform selectionAreaTransform;
        private Vector3 _startSelectionPos;
        private List<GameObject> _selectedUnits;

        // control units 
        private bool _isSelected;
        private bool _isMoving;
        private Vector3 _targetPos;

        private void Start()
        {
            _camera = Camera.main;
            _selectedUnits = new List<GameObject>();
            selectionAreaTransform.gameObject.SetActive(false);
        }

        private void Update()
        {
            SelectUnits();
            MoveUnits();
        }

        private void LateUpdate()
        {
            ControlCamera();
        }

        private void ControlCamera()
        {
            // positive -> scroll up -> zoom out camera
            // negative -> scroll down -> zoom in camera
            if (Input.mouseScrollDelta.y < 0 && _camera.orthographicSize < 20)
            {
                _camera.orthographicSize++;
            }
            else if (Input.mouseScrollDelta.y > 0 && _camera.orthographicSize > 5)
            {
                _camera.orthographicSize--;
            }

            // hold middle mouse button to drag the camera view
            if (Input.GetMouseButton(2))
            {
                _dragPosDifference = Helper.GetMouseWorldPos(_camera) - transform.position;
                if (!_isDragged)
                {
                    _isDragged = true;
                    _originDragPos = Helper.GetMouseWorldPos(_camera);
                }
            }
            else
            {
                _isDragged = false;
            }

            if (_isDragged)
            {
                transform.position = _originDragPos - _dragPosDifference;
            }
        }

        private void SelectUnits()
        {
            // left mouse pressed
            if (Input.GetMouseButtonDown(0))
            {
                selectionAreaTransform.gameObject.SetActive(true);
                _startSelectionPos = Helper.GetMouseWorldPos(_camera);
            }

            // left mouse in between
            if (Input.GetMouseButton(0))
            {
                var currentSelectionPos = Helper.GetMouseWorldPos(_camera);
                var lowerLeft = new Vector3(Mathf.Min(_startSelectionPos.x, currentSelectionPos.x),
                    Mathf.Min(_startSelectionPos.y, currentSelectionPos.y));
                var upperRight = new Vector3(Mathf.Max(_startSelectionPos.x, currentSelectionPos.x),
                    Mathf.Max(_startSelectionPos.y, currentSelectionPos.y));
                selectionAreaTransform.position = lowerLeft;
                selectionAreaTransform.localScale = upperRight - lowerLeft;
            }

            // left mouse released
            if (Input.GetMouseButtonUp(0))
            {
                selectionAreaTransform.gameObject.SetActive(false);
                Collider2D[] collider2Ds =
                    Physics2D.OverlapAreaAll(_startSelectionPos, Helper.GetMouseWorldPos(_camera));

                // deselect all units
                foreach (var unit in _selectedUnits)
                {
                    unit.transform.GetChild(0).gameObject.SetActive(false);
                }

                _selectedUnits.Clear();
                _isSelected = false;
                _isMoving = false;

                // select units within selection area 
                foreach (var c in collider2Ds)
                {
                    if (c.gameObject.CompareTag("Farmer"))
                    {
                        _selectedUnits.Add(c.gameObject);
                        c.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }

                // check any units being selected
                if (_selectedUnits.Count > 0)
                {
                    _isSelected = true;
                }
            }
        }

        private void MoveUnits()
        {
            if (_isSelected && Input.GetMouseButtonDown(1))
            {
                _isMoving = true;
                _targetPos = Helper.GetMouseWorldPos(_camera);
            }

            if (_isMoving)
            {
                List<Vector3> targetPosList =
                    GetPosListAround(_targetPos, new float[] {1f, 2f, 3f}, new int[] {5, 10, 20});

                var targetPosListIndex = 0;

                foreach (var unit in _selectedUnits)
                {
                    var rtsUnit = unit.GetComponent<RtsUnit>();
                    if (_camera.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
                    {
                        rtsUnit.Move("left");
                    }
                    else if (_camera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
                    {
                        rtsUnit.Move("right");
                    }
                    else if (_camera.ScreenToWorldPoint(Input.mousePosition).y < transform.position.y)
                    {
                        rtsUnit.Move("back");
                    }
                    else if (_camera.ScreenToWorldPoint(Input.mousePosition).y > transform.position.y)
                    {
                        rtsUnit.Move("front");
                    }

                    rtsUnit.transform.position =
                        Vector3.MoveTowards(rtsUnit.transform.position,
                            new Vector3(targetPosList[targetPosListIndex].x, targetPosList[targetPosListIndex].y,
                                rtsUnit.transform.position.z), Time.deltaTime * 5);

                    targetPosListIndex = (targetPosListIndex + 1 % targetPosList.Count);

                    if (rtsUnit.transform.position == _targetPos)
                    {
                        _isMoving = false;
                        rtsUnit.Idle();
                    }
                }
            }
        }

        private List<Vector3> GetPosListAround(Vector3 startPos, float[] ringDistArr, int[] ringPosCountArr)
        {
            List<Vector3> posList = new List<Vector3>();
            posList.Add(startPos);
            for (var i = 0; i < ringDistArr.Length; i++)
            {
                posList.AddRange(GetPosListAround(startPos, ringDistArr[i], ringPosCountArr[i]));
            }

            return posList;
        }

        private List<Vector3> GetPosListAround(Vector3 startPos, float dist, int posCount)
        {
            List<Vector3> posList = new List<Vector3>();
            for (var i = 0; i < posCount; i++)
            {
                var angle = i * (360f / posCount);
                var dir = Quaternion.Euler(0, 0, angle) * new Vector3(1, 0);
                var pos = startPos + dir * dist;
                posList.Add(pos);
            }

            return posList;
        }
    }
}
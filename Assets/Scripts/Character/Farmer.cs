using System;
using UnityEngine;

namespace Character
{
    public class Farmer : MonoBehaviour
    {
        private Camera _camera;
        private Animator _animator;
        private bool _isSelected;
        private bool _isMoving;
        private Vector3 _targetPos;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                if (hit.collider.gameObject.CompareTag("Farmer") && Input.GetMouseButton(0))
                {
                    _isSelected = true;
                    Debug.Log("selected");
                }
            }
        }

        private void Update()
        {
            if (_isSelected && Input.GetMouseButtonDown(0))
            {
                _isMoving = true;
                _targetPos = new Vector3(_camera.ScreenToWorldPoint(Input.mousePosition).x,
                    _camera.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            }

            if (_isMoving)
            {
                if (_camera.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
                {
                    _animator.SetBool("Walk Left", true);
                    _animator.SetBool("Walk Right", false);
                    _animator.SetBool("Walk Back", false);
                    _animator.SetBool("Walk Front", false);
                }
                else if (_camera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
                {
                    _animator.SetBool("Walk Left", false);
                    _animator.SetBool("Walk Right", true);
                    _animator.SetBool("Walk Back", false);
                    _animator.SetBool("Walk Front", false);
                }
                else if (_camera.ScreenToWorldPoint(Input.mousePosition).y < transform.position.x)
                {
                    _animator.SetBool("Walk Left", false);
                    _animator.SetBool("Walk Right", false);
                    _animator.SetBool("Walk Back", false);
                    _animator.SetBool("Walk Front", true);
                }
                else if (_camera.ScreenToWorldPoint(Input.mousePosition).y > transform.position.x)
                {
                    _animator.SetBool("Walk Left", false);
                    _animator.SetBool("Walk Right", false);
                    _animator.SetBool("Walk Back", true);
                    _animator.SetBool("Walk Front", false);
                }

                transform.position = Vector3.MoveTowards(transform.position, _targetPos, 5 * Time.deltaTime);
                if (transform.position == _targetPos)
                {
                    _isMoving = false;
                    _animator.SetBool("Walk Left", false);
                    _animator.SetBool("Walk Right", false);
                    _animator.SetBool("Walk Back", false);
                    _animator.SetBool("Walk Front", false);
                }
            }
        }
    }
}
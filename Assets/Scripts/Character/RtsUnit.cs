using System;
using UnityEngine;

namespace Character
{
    public class RtsUnit : MonoBehaviour
    {
        private Animator _animator;
        private string _dir;
        private bool _dirIsLocked;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _dirIsLocked = false;
        }

        public void Move(Vector3 currentPos, Vector3 targetPos)
        {
            var dirX = Mathf.Abs((int) (currentPos.x - targetPos.x));
            var dirY = Mathf.Abs((int) (currentPos.y - targetPos.y));

            if (dirX - dirY >= 0 && !_dirIsLocked)
            {
                if (currentPos.x > targetPos.x)
                {
                    _dir = "left";
                }
                else
                {
                    _dir = "right";
                }
            }
            else if(dirX - dirY < 0 && !_dirIsLocked)
            {
                if (currentPos.y > targetPos.y)
                {
                    _dir = "front";
                }
                else
                {
                    _dir = "back";
                }
            }

            _dirIsLocked = true;
            Move();
        }

        public void ChangeDirection()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _dirIsLocked = false;
            }
        }

        public void Idle()
        {
            _animator.SetBool("Walk Left", false);
            _animator.SetBool("Walk Right", false);
            _animator.SetBool("Walk Back", false);
            _animator.SetBool("Walk Front", false);
            _dir = "";
            _dirIsLocked = false;
        }

        private void Move()
        {
            if (_dir == "left")
            {
                _animator.SetBool("Walk Left", true);
                _animator.SetBool("Walk Right", false);
                _animator.SetBool("Walk Back", false);
                _animator.SetBool("Walk Front", false);
            }
            else if (_dir == "right")
            {
                _animator.SetBool("Walk Left", false);
                _animator.SetBool("Walk Back", false);
                _animator.SetBool("Walk Front", false);
                _animator.SetBool("Walk Right", true);
            }
            else if (_dir == "front")
            {
                _animator.SetBool("Walk Left", false);
                _animator.SetBool("Walk Right", false);
                _animator.SetBool("Walk Back", false);
                _animator.SetBool("Walk Front", true);
            }
            else if (_dir == "back")
            {
                _animator.SetBool("Walk Left", false);
                _animator.SetBool("Walk Right", false);
                _animator.SetBool("Walk Back", true);
                _animator.SetBool("Walk Front", false);
            }
        }
    }
}
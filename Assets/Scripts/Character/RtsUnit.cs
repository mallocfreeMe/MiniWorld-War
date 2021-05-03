using UnityEngine;

namespace Character
{
    public class RtsUnit : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Move(string direction)
        {
            if (direction == "left")
            {
                _animator.SetBool("Walk Right", false);
                _animator.SetBool("Walk Back", false);
                _animator.SetBool("Walk Front", false);
                _animator.SetBool("Walk Left", true);
            }
            else if (direction == "right")
            {
                _animator.SetBool("Walk Left", false);
                _animator.SetBool("Walk Back", false);
                _animator.SetBool("Walk Front", false);
                _animator.SetBool("Walk Right", true);
            }
            else if (direction == "front")
            {
                _animator.SetBool("Walk Left", false);
                _animator.SetBool("Walk Right", false);
                _animator.SetBool("Walk Back", false);
                _animator.SetBool("Walk Front", true);
            }
            else if (direction == "back")
            {
                _animator.SetBool("Walk Left", false);
                _animator.SetBool("Walk Right", false);
                _animator.SetBool("Walk Back", true);
                _animator.SetBool("Walk Front", false);
            }
        }

        public void Idle()
        {
            _animator.SetBool("Walk Left", false);
            _animator.SetBool("Walk Right", false);
            _animator.SetBool("Walk Back", false);
            _animator.SetBool("Walk Front", false);
        }
    }
}
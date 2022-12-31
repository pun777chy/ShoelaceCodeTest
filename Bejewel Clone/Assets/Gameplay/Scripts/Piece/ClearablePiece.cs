using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoelace.Piece
{
    public class ClearablePiece : MonoBehaviour
    {
        public AnimationClip clearAnimation;

        private bool isBeingCleared = false;
        public  bool IsBeingCleared
        {
            get
            {
                return isBeingCleared;
            }
        }
        protected MainPiece piece;
        private void Awake()
        {
            piece = GetComponent<MainPiece>();
        }
        public void Clear()
        {
            isBeingCleared = true;
            StartCoroutine(ClearCoroutine());
        }
        private IEnumerator ClearCoroutine()
        {
            Animator animator = GetComponent<Animator>();
            if(animator)
            {
                animator.Play(clearAnimation.name);
                yield return new WaitForSeconds(clearAnimation.length);
                Destroy(gameObject);
            }
        }
    }
}

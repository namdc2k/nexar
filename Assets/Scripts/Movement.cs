using SO;
using UnityEngine;

public class Movement : MonoBehaviour, IMove{
    private Vector3 _targetPosition;
    private Animator _animator;

    void Start() {
        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, _targetPosition,
            Time.fixedDeltaTime * 10);
    }

    private void Update() {
        _animator.SetFloat("Speed", (_targetPosition - transform.position).magnitude);
    }

    public void Move(Vector3 target) {
        _targetPosition = target;
    }
}
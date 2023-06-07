using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    public Vector3 TargetPosition;

    private Rigidbody _rigidbody;
    private float _speed;
    private float _moveTime = 0.5f;
    private float _inverseMoveTime;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _speed = 3f;

        _inverseMoveTime = 1 / _moveTime;

        StartCoroutine(Move());
    }


    private IEnumerator Move()
    {
        // 이동할 남은 거리 계산
        float remainDistance = (transform.position - TargetPosition).sqrMagnitude;  // 이동할 지점과 현재 위치의 차이에 .sqrMagnitude를 이용해 벡터 길이의 제곱을 구함

        while(remainDistance > float.Epsilon)  // 남은 거리가 0에 가까운 극한보다 큰 동안
        {
            Vector3 newPosition = Vector3.MoveTowards(_rigidbody.position, TargetPosition, _inverseMoveTime * Time.deltaTime);
            _rigidbody.MovePosition(newPosition);

            remainDistance = (transform.position - TargetPosition).sqrMagnitude;
            yield return null;
        }
    }
}

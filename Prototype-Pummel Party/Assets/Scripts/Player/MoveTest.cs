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
        // �̵��� ���� �Ÿ� ���
        float remainDistance = (transform.position - TargetPosition).sqrMagnitude;  // �̵��� ������ ���� ��ġ�� ���̿� .sqrMagnitude�� �̿��� ���� ������ ������ ����

        while(remainDistance > float.Epsilon)  // ���� �Ÿ��� 0�� ����� ���Ѻ��� ū ����
        {
            Vector3 newPosition = Vector3.MoveTowards(_rigidbody.position, TargetPosition, _inverseMoveTime * Time.deltaTime);
            _rigidbody.MovePosition(newPosition);

            remainDistance = (transform.position - TargetPosition).sqrMagnitude;
            yield return null;
        }
    }
}

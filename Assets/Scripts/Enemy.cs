using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Attached to enemy object not it's children
    [Header("Configuration Parameters")]
    [Tooltip("Determins the speed of the enemy.")]
    [SerializeField]
    int _moveSpeed = 12;
    [Tooltip("Determins at which postion on the x axis the enemy gets destroyed offscreen.")]
    [SerializeField]
    float _enemyPosLimit = -11f;

    void Update()
    {
        DestroyEnemy();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isGameOver())
            Move();

        else
            transform.position += Vector3.zero;
    }

    void Move()
    {
        transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
    }

    void DestroyEnemy()
    {
        if (transform.position.x < _enemyPosLimit || GameManager.Instance.isGameOver())
        {
            Destroy(gameObject);
        }
    }
}

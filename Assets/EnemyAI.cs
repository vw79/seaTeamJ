using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase }
    public EnemyState currentState = EnemyState.Idle;

    public Transform player;
    public Tilemap floorTilemap;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    private Vector3 patrolTarget;
    private bool playerDetected;

    public CircleCollider2D detectionCollider;
    public BoxCollider2D povCollider;

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
        }
    }

    private void Idle()
    {
        // Logic for idle state, can include waiting for a few seconds
        if (Random.value < 0.01f)
        {
            SetRandomPatrolTarget();
            currentState = EnemyState.Patrol;
        }
    }

    private void Patrol()
    {
        MoveTowards(patrolTarget, patrolSpeed);

        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            currentState = EnemyState.Idle;
        }
    }

    private void Chase()
    {
        if (player != null)
        {
            MoveTowards(player.position, chaseSpeed);
        }
    }

    private void SetRandomPatrolTarget()
    {
        // Get random position within tilemap bounds
        BoundsInt bounds = floorTilemap.cellBounds;

        Vector3Int randomCellPosition = new Vector3Int(
            Random.Range(bounds.xMin, bounds.xMax),
            Random.Range(bounds.yMin, bounds.yMax),
            0);

        if (floorTilemap.HasTile(randomCellPosition))
        {
            patrolTarget = floorTilemap.GetCellCenterWorld(randomCellPosition);
        }
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the trigger");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected in trigger");
            if (other == povCollider)
            {
                Debug.Log("Player in POV");
                currentState = EnemyState.Chase;
                playerDetected = true;
            }
            else if (other == detectionCollider && !playerDetected)
            {
                Debug.Log("Player in detection range");
                playerDetected = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == player.GetComponent<Collider2D>())
        {
            if (other == povCollider)
            {
                currentState = EnemyState.Patrol;
                playerDetected = false;
            }
        }
    }
}
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
    private bool playerInDetectionZone = false;
    private bool playerInPOVZone = false;

    private Animator anim;
    private string enemyLastFacePosition = "Front";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

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
        if (!playerInDetectionZone)
        {
            anim.Play(GetIdleAnimation());
        }

        if (Random.value < 0.01f && !playerInDetectionZone)
        {
            SetRandomPatrolTarget();
            currentState = EnemyState.Patrol;
        }
    }

    private void Patrol()
    {
        MoveTowards(patrolTarget, patrolSpeed, false);

        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            currentState = EnemyState.Idle;
        }
    }

    private void Chase()
    {
        if (player != null)
        {
            MoveTowards(player.position, chaseSpeed, true);
        }
    }

    private void SetRandomPatrolTarget()
    {
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

    private void MoveTowards(Vector3 target, float speed, bool isChasing)
    {
        Vector3 direction = (target - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        UpdateAnimationBasedOnMovement(direction, isChasing);
        transform.position = newPosition;
    }

    private void UpdateAnimationBasedOnMovement(Vector3 direction, bool isChasing)
    {
        if (direction != Vector3.zero)
        {
            if (direction.x < 0)
            {
                enemyLastFacePosition = "Left";
                anim.Play(isChasing ? "enemyRunLeft" : "enemyWalkLeft");
            }
            else if (direction.x > 0)
            {
                enemyLastFacePosition = "Right";
                anim.Play(isChasing ? "enemyRunRight" : "enemyWalkRight");
            }
            else if (direction.y > 0)
            {
                enemyLastFacePosition = "Back";
                anim.Play(isChasing ? "enemyRunBack" : "enemyWalkBack");
            }
            else if (direction.y < 0)
            {
                enemyLastFacePosition = "Front";
                anim.Play(isChasing ? "enemyRunFront" : "enemyWalkFront");
            }
        }
        else
        {
            anim.Play(GetIdleAnimation());
        }
    }

    private string GetIdleAnimation()
    {
        switch (enemyLastFacePosition)
        {
            case "Back":
                return "enemyIdleBack";
            case "Left":
                return "enemyIdleLeft";
            case "Right":
                return "enemyIdleRight";
            default:
                return "enemyIdleFront";
        }
    }

    public void OnPlayerEnterDetectionZone()
    {
        playerInDetectionZone = true;
    }

    public void OnPlayerExitDetectionZone()
    {
        playerInDetectionZone = false;
    }

    public void OnPlayerEnterPOVZone()
    {
        currentState = EnemyState.Chase;
        playerInPOVZone = true;
    }

    public void OnPlayerExitPOVZone()
    {
        if (playerInPOVZone)
        {
            currentState = playerInDetectionZone ? EnemyState.Patrol : EnemyState.Idle;
            playerInPOVZone = false;
        }
    }
}
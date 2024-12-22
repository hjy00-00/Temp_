using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    [SerializeField] Player cMonster;

    public LayerMask playerLayer;               // �÷��̾ ���� ���̾�
    public LayerMask obstacleLayer;             // ��ֹ��� ���� ���̾�

    public enum E_STATE { NONE = -1, MOVE, TRACKING, ATTACK }
    [SerializeField] E_STATE eMonsterState = E_STATE.NONE;

    [SerializeField] float fMoveSpeed = 3f;                  // ���� �̵� �ӵ�
    [SerializeField] float fStopDuration = 2f;               // ���� �ð�
    [SerializeField] float fDetectionRange = 10f;            // �÷��̾� �ν� �Ÿ�
    [SerializeField] float fDetectionAngle = 120f;           // ��ä�� ���� ����
    [SerializeField] float fAttackRange = 2f;                // ���� �Ÿ�
    [SerializeField] float fRotationSpeed = 5f;              // ȸ�� �ӵ�
    [SerializeField] float fObstacleDetectionRange = 1.5f;   // ��ֹ� ���� �Ÿ�

    Rigidbody rb_Monster;                       // ������ Rigidbody
    public float fPatrolRange = 5f;             // ��Ʈ�� �̵� �ݰ�
    public float fPatrolChangeInterval = 3f;    // ��Ʈ�� ���� ���� �ֱ�
    Vector3 patrolTarget;                       // ���� ��Ʈ�� ��ǥ ��ġ
    Vector3 currentDirection;                   // ���� �̵� ����
    public Transform targetPos;                 // ���� ���� ���
    Vector3 originalPos;                        // ������ �ʱ� ��ġ

    bool isLife = true;
    Spawn cSpawn;
    void Start() {
        rb_Monster = GetComponent<Rigidbody>();
        originalPos = transform.position;
        currentDirection = transform.forward; // �ʱ� ���� ����
        eMonsterState = E_STATE.MOVE;

        cSpawn = GameManager.GetInstance().Spawn;
    }

    void FixedUpdate() {
        if(isLife) {
            switch(eMonsterState) {
                case E_STATE.NONE:
                    Stop();
                    break;

                case E_STATE.MOVE:
                    Patrol();
                    break;

                case E_STATE.TRACKING:
                    FollowPlayer();
                    break;

                case E_STATE.ATTACK:
                    AttackPlayer();
                    break;
            }
        }
    }
    void Update() {
        if(isLife) {
            DetectObstacle();   // ��ֹ� ����
            UpdateState();      // ���� ����
            Die();              // HP üũ
        }
    }

    void UpdateState() {
        if(FindPlayer()) {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPos.position);
            eMonsterState = distanceToPlayer <= fAttackRange ? E_STATE.ATTACK : E_STATE.TRACKING;
        }
        else if(eMonsterState == E_STATE.TRACKING) {
            eMonsterState = E_STATE.NONE; // �÷��̾ ������ NONE ���·� ��ȯ
        }
    }

    void Stop() {
        rb_Monster.velocity = Vector3.zero; // ���� ����
        StartCoroutine(StopTimer());
    }

    IEnumerator StopTimer() {
        yield return new WaitForSeconds(fStopDuration);
        eMonsterState = E_STATE.MOVE; // �̵� ���·� ����
    }

    void Patrol() {
        if(Vector3.Distance(transform.position, patrolTarget) < 0.5f) {
            SetRandomPatrolTarget(); // ���ο� ���� ��ǥ ����
        }
        else {
            Vector3 directionToTarget = (patrolTarget - transform.position).normalized;
            rb_Monster.velocity = directionToTarget * fMoveSpeed;
            RotateTowards(patrolTarget); // ��ǥ�� ���� ȸ��
        }
    }

    void SetRandomPatrolTarget() {
        Vector3 randomOffset = new Vector3(
            Random.Range(-fPatrolRange, fPatrolRange),
            0,
            Random.Range(-fPatrolRange, fPatrolRange)
        );
        patrolTarget = originalPos + randomOffset;

        if(Physics.Raycast(patrolTarget + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f)) {
            patrolTarget.y = hit.point.y;
        }
    }

    void FollowPlayer() {
        currentDirection = (targetPos.position - transform.position).normalized;
        rb_Monster.velocity = currentDirection * fMoveSpeed;
        RotateTowards(targetPos.position);
    }

    void AttackPlayer() {
        rb_Monster.velocity = Vector3.zero;
        RotateTowards(targetPos.position);
        StartCoroutine(DelayAttack());
    }

    IEnumerator DelayAttack() {
        yield return new WaitForSeconds(2f);
        Debug.Log("Monster is attacking!");
    }

    void DetectObstacle() {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        if(Physics.Raycast(rayOrigin, currentDirection, out hit, fObstacleDetectionRange, obstacleLayer)) {
            float randomAngle = Random.Range(-90f, 90f);
            currentDirection = Quaternion.Euler(0, randomAngle, 0) * currentDirection;
            RotateTowards(transform.position + currentDirection);
        }
    }

    bool FindPlayer() {
        Collider[] hits = Physics.OverlapSphere(transform.position, fDetectionRange, playerLayer);
        foreach(Collider hit in hits) {
            Transform potentialPlayer = hit.transform;
            Vector3 directionToPlayer = (potentialPlayer.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if(angleToPlayer <= fDetectionAngle / 2f) {
                if(Physics.Raycast(transform.position, directionToPlayer, out RaycastHit rayHit, fDetectionRange, playerLayer)) {
                    if(rayHit.collider.transform == potentialPlayer) {
                        targetPos = potentialPlayer; // �÷��̾ Ÿ������ ����
                        return true; // �÷��̾� �߰�
                    }
                }
            }
        }
        return false; // �÷��̾� �̹߰�
    }

    void RotateTowards(Vector3 _target) {
        Vector3 direction = (_target - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * fRotationSpeed);
    }

    void Die() {
        if(cMonster.PlayerStatus.nHP <= 0) {
            Debug.Log("Die");
            isLife = false;
            cSpawn.SpawnInven(this.transform.gameObject, cMonster.GetInventory, DropInventory.E_INVEN_TYPE.PLAYER);
            this.transform.position = cMonster.ReSpawnPos.transform.position;
        }
    }
}
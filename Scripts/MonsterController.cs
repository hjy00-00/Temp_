using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    [SerializeField] Player cMonster;

    public LayerMask playerLayer;               // 플레이어가 속한 레이어
    public LayerMask obstacleLayer;             // 장애물이 속한 레이어

    public enum E_STATE { NONE = -1, MOVE, TRACKING, ATTACK }
    [SerializeField] E_STATE eMonsterState = E_STATE.NONE;

    [SerializeField] float fMoveSpeed = 3f;                  // 몬스터 이동 속도
    [SerializeField] float fStopDuration = 2f;               // 멈춤 시간
    [SerializeField] float fDetectionRange = 10f;            // 플레이어 인식 거리
    [SerializeField] float fDetectionAngle = 120f;           // 부채꼴 감지 각도
    [SerializeField] float fAttackRange = 2f;                // 공격 거리
    [SerializeField] float fRotationSpeed = 5f;              // 회전 속도
    [SerializeField] float fObstacleDetectionRange = 1.5f;   // 장애물 감지 거리

    Rigidbody rb_Monster;                       // 몬스터의 Rigidbody
    public float fPatrolRange = 5f;             // 패트롤 이동 반경
    public float fPatrolChangeInterval = 3f;    // 패트롤 방향 변경 주기
    Vector3 patrolTarget;                       // 현재 패트롤 목표 위치
    Vector3 currentDirection;                   // 현재 이동 방향
    public Transform targetPos;                 // 추적 중인 대상
    Vector3 originalPos;                        // 몬스터의 초기 위치

    bool isLife = true;
    Spawn cSpawn;
    void Start() {
        rb_Monster = GetComponent<Rigidbody>();
        originalPos = transform.position;
        currentDirection = transform.forward; // 초기 방향 설정
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
            DetectObstacle();   // 장애물 감지
            UpdateState();      // 상태 갱신
            Die();              // HP 체크
        }
    }

    void UpdateState() {
        if(FindPlayer()) {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPos.position);
            eMonsterState = distanceToPlayer <= fAttackRange ? E_STATE.ATTACK : E_STATE.TRACKING;
        }
        else if(eMonsterState == E_STATE.TRACKING) {
            eMonsterState = E_STATE.NONE; // 플레이어를 잃으면 NONE 상태로 전환
        }
    }

    void Stop() {
        rb_Monster.velocity = Vector3.zero; // 몬스터 정지
        StartCoroutine(StopTimer());
    }

    IEnumerator StopTimer() {
        yield return new WaitForSeconds(fStopDuration);
        eMonsterState = E_STATE.MOVE; // 이동 상태로 복귀
    }

    void Patrol() {
        if(Vector3.Distance(transform.position, patrolTarget) < 0.5f) {
            SetRandomPatrolTarget(); // 새로운 랜덤 목표 설정
        }
        else {
            Vector3 directionToTarget = (patrolTarget - transform.position).normalized;
            rb_Monster.velocity = directionToTarget * fMoveSpeed;
            RotateTowards(patrolTarget); // 목표를 향해 회전
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
                        targetPos = potentialPlayer; // 플레이어를 타겟으로 설정
                        return true; // 플레이어 발견
                    }
                }
            }
        }
        return false; // 플레이어 미발견
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
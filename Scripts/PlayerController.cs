using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    [SerializeField] Player m_cPlayer;

    [SerializeField] float m_fWalkSpeed = 5.0f;         //걷기 속도
    [SerializeField] float m_fCrouchSpeed = 1.0f;       //앉기 속도
    float m_fApplySpeed;


    [SerializeField] float m_fJumpForce;

    [SerializeField] bool m_bLife = true;
    bool m_bMove = false;
    bool m_bCrouch = false;
    [SerializeField] bool m_bGround = true;

    [SerializeField] float m_fCrouchPosY = 0.675f;      //앉기 카메라 위치
    [SerializeField] float m_fOriginPosY = 1.35f;       //기존 카메라 위치
    float m_fApplyCrouchPosY;

    /*******************/
    [SerializeField] float m_fLookSensitivity;             //카메라 민감도
    [SerializeField] float m_fCameraRotationLimit;
    float m_fCurrentCameraRotationX = 0;




    [SerializeField] float m_fCamAngleMin = 5.0f;       //위 최댓값
    [SerializeField] float m_fCamAngleMax = -2.0f;      //아래 최솟값



    [SerializeField] Transform m_tranCamera;
    [SerializeField] Transform m_tranCamAxis;
    [SerializeField] Transform m_transPlayer;
    [SerializeField] Transform m_transDropPos;
    [SerializeField] Transform m_transProjectilePos;
    [SerializeField] Transform m_transForward;

    [SerializeField] GUIPlayerInfo m_cPlayerInfo;

    GUIManager m_cGuiManager;
    SkillManager m_cSkillManager;
    QuestManager m_cQuestManager;

    Spawn m_cSpawn;

    Camera m_cCamera;
    Rigidbody m_cPlayerRigid;

    [SerializeField] Animator m_aniMove;

    [SerializeField] Item m_cTempItem;

    public bool Life { get { return m_bLife; } set { m_bLife = value; } }
    public Transform DropPos { get { return m_transDropPos; } }
    public Transform ProjectilePos { get { return m_transProjectilePos; } }
    public Animator Animator { get { return m_aniMove; } }
    public Transform Forward { get { return m_transForward; } }
    public Rigidbody PlayerRigid { get { return m_cPlayerRigid; } }
    /*********************************************************************************/
    private void Start() {
        m_cPlayerRigid = GetComponent<Rigidbody>();
        m_cCamera = GameManager.GetInstance().MainCamera;

        m_fApplySpeed = m_fWalkSpeed;
        m_fOriginPosY = m_tranCamAxis.transform.localPosition.y;
        m_fApplyCrouchPosY = m_fOriginPosY;

        m_cGuiManager = GameManager.GetInstance().GUIManager;
        m_cSkillManager = GameManager.GetInstance().SkillManager;
        m_cQuestManager = GameManager.GetInstance().QuestManager;

        m_cSpawn = GameManager.GetInstance().Spawn;

    }

    private void FixedUpdate() {
        //플레이어 이동        
        Move();
    }

    void Update() {
        m_cPlayerInfo.UpdataPlayerStatus(m_cPlayer);

        if(m_bLife) {
            if(!m_cPlayer.HpCheck() && GameManager.GetInstance().GetState == GameManager.E_STATE.PLAY) {
                Die();
            }
        }

        // 행동
        IsGround();
        TryJump();
        TryCrouch();

        // 팝업 오픈
        TryOpenInventory();
        TryOpenQuestList();
        TryOpenSkillList();
        TrySkill();



        if(!m_cGuiManager.GUIActivated) {
            //X : 공격
            if(m_bMove != true) {
                if(Input.GetKeyDown(KeyCode.X)) {
                    Debug.Log("X 입력");
                    TryShot();
                }
            }

            //임시 경험치 획득 키
            if(Input.GetKeyDown(KeyCode.M)) {
                m_cPlayer.GetExp(10);
            }
            //임시 아이템 획득 키
            if(Input.GetKeyDown(KeyCode.P)) {
                GameManager.GetInstance().Inventory.AcquireItem(m_cTempItem, 10);
            }
            //체력 -1
            if(Input.GetKeyDown(KeyCode.O)) {
                m_cPlayer.Damage(1);
            }

        }
    }

    /*********************************************************************************/

    void TryOpenInventory() {
        if(Input.GetKeyDown(KeyCode.I)) {
            m_cGuiManager.SetGUIPopup(GUIManager.E_GUI_STATE.INVENTORY);
        }
    }

    void TryOpenQuestList() {
        if(Input.GetKeyDown(KeyCode.L)) {
            m_cGuiManager.SetGUIPopup(GUIManager.E_GUI_STATE.QUEST_LIST);
        }
    }

    void TryOpenSkillList() {
        if(Input.GetKeyDown(KeyCode.K)) {
            m_cGuiManager.SetGUIPopup(GUIManager.E_GUI_STATE.SKILL_LIST);
        }
    }
    void TrySkill() {
        if(Input.GetKeyDown(KeyCode.C)) {
            SelectSkill();
        }
        if(Input.GetKeyUp(KeyCode.C)) {
            SelectSkill();
        }
    }
    void SelectSkill() {

    }

    public void TryShot() {
        if(m_cPlayer.MpCheck(m_cSkillManager.UseMp) == true && m_cSkillManager.Attack == false) {
            m_cSkillManager.Shot(null, m_transProjectilePos);
            m_cPlayer.UseMp(m_cSkillManager.UseMp);
        }
    }
    /*********************************************************************************/
    void TryCrouch() {
        if(Input.GetKeyDown(KeyCode.LeftControl)) {
            Crouch();
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)) {
            Crouch();
        }
    }
    void Crouch() {
        m_bCrouch = !m_bCrouch;
        if(m_bCrouch) {
            m_fApplySpeed = m_fCrouchSpeed;
            m_fApplyCrouchPosY = m_fCrouchPosY;
        }
        else {
            m_fApplySpeed = m_fWalkSpeed;
            m_fApplyCrouchPosY = m_fOriginPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }
    IEnumerator CrouchCoroutine() {
        float posY = m_tranCamAxis.transform.localPosition.y;
        int count = 0;

        while(posY != m_fApplyCrouchPosY) {
            count++;
            posY = Mathf.Lerp(posY, m_fApplyCrouchPosY, 0.3f);
            m_tranCamAxis.transform.localPosition = new Vector3(0, posY, 0);
            if(count > 15)
                break;
            yield return null;
        }
        m_tranCamAxis.transform.localPosition = new Vector3(0, m_fApplyCrouchPosY, 0f);
    }

    void IsGround() {
        //isGround = Physics.Raycast(transform.position, Vector3.down, m_cPlayer.BoxCollider.bounds.extents.y + 0.1f);
    }
    void TryJump() {
        if(GUIManager.E_GUI_STATE.LEVELUP != GameManager.GetInstance().GUIManager.PopUp || GUIManager.E_GUI_STATE.DIALOGUE != GameManager.GetInstance().GUIManager.PopUp) {
            if(Input.GetKeyDown(KeyCode.Space) && m_bGround) {
                Jump();
            }
        }
    }
    void Jump() {
        m_cPlayerRigid.velocity = transform.up * m_fJumpForce;
    }


    void Move() {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * m_fWalkSpeed;

        m_cPlayerRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    public void CameraRotation() {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * m_fLookSensitivity;
        m_fCurrentCameraRotationX -= _cameraRotationX;
        m_fCurrentCameraRotationX = Mathf.Clamp(m_fCurrentCameraRotationX, -m_fCameraRotationLimit, m_fCameraRotationLimit);

        m_cCamera.transform.localEulerAngles = new Vector3(m_fCurrentCameraRotationX, 0f, 0f);
    }
    public void CharacterRotation() {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * m_fLookSensitivity;
        m_cPlayerRigid.MoveRotation(m_cPlayerRigid.rotation * Quaternion.Euler(_characterRotationY));


    }

    public void JoystickMove(Vector2 _inputDir) {
        Vector2 moveInput = _inputDir;
        m_bMove = moveInput.magnitude != 0;
        if(m_bMove) {
            Vector3 lookForward = new Vector3(m_tranCamera.forward.x, 0f, m_tranCamera.forward.z).normalized;
            Vector3 lookRight = new Vector3(m_tranCamera.right.x, 0f, m_tranCamera.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            m_transPlayer.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * m_fApplySpeed;

            m_aniMove.SetFloat("speedY", moveInput.y);
            m_aniMove.SetFloat("speedX", moveInput.x);
        }
    }





    /*********************************************************************************/

    void Die() {
        Debug.Log("Die");
        m_bLife = false;
        m_cSpawn.SpawnInven(this.transform.gameObject, m_cPlayer.GetInventory, DropInventory.E_INVEN_TYPE.PLAYER);
        this.transform.position = m_cPlayer.ReSpawnPos.transform.position;
    }
}
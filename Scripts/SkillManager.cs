using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    public enum E_SKILL { FORWARD }
    public E_SKILL m_eSkill;

    //public GUISlot[] m_listSkillSlot;
    [SerializeField] GameObject m_objSlotParent;
    [SerializeField] Transform m_transProjectilePos;

    [SerializeField] Animator m_ani;

    bool m_bAttack;
    Transform m_transTarget;
    Transform m_transForward;

    int m_nUseMp;
    int m_nDamage;
    float m_fShotSpeed;
    [SerializeField] float m_fDestroyTime;
    float m_fShotDist;
    float m_fShotDelay;

    [SerializeField] Skill[] skills;
    [SerializeField] GameObject[] skillsPooling;

    [SerializeField] List<GameObject>[] m;


    [SerializeField] GameObject m_prefabMagicBall;

    GameObject[] m_listTargetPool;
    [SerializeField] GameObject[] m_objMagicBall;


    public bool Attack { get { return m_bAttack; } }
    public int UseMp { get { return m_nUseMp; } }
    public float ShotDist { get { return m_fShotDist; } }
    public float ShotDelay { get { return m_fShotDelay; } }
    public int Damage { get { return m_nDamage; } }

    /*********************************************************************************/
    private void Awake() {
        m_objMagicBall = new GameObject[5];


        Generate();
    }

    private void Start() {
        //m_listSkillSlot = m_objSlotParent.GetComponentsInChildren<GUISlot>();
        m_transProjectilePos = GameManager.GetInstance().PlayerController.ProjectilePos;
        m_ani = GameManager.GetInstance().PlayerController.Animator;
        m_transForward = GameManager.GetInstance().PlayerController.Forward;
    }
    /*********************************************************************************/

    void Generate() {
        for(int i = 0; i < m_objMagicBall.Length; i++) {
            m_objMagicBall[i] = Instantiate(m_prefabMagicBall);
            m_objMagicBall[i].SetActive(false);
        }
    }
    
    // 맵 이동 시 (로딩 시) 풀링 하게 변경해야 할 듯 (플레이어의 스킬 종류를 받아와서...)
    public GameObject MakeOBJ(string _type) {

        switch(_type) {
            case "MagicBall":
                m_listTargetPool = m_objMagicBall;
                break;
        }

        for(int i = 0; i < m_listTargetPool.Length; i++) {
            if(!m_listTargetPool[i].activeSelf) {
                m_listTargetPool[i].SetActive(true);
                return m_listTargetPool[i];
            }
        }

        return null;
    }
    /*********************************************************************************/
    public void Shot(Transform _target, Transform _startingPos) {
        m_bAttack = true;
        m_transProjectilePos = _startingPos;
        if(_target != null)
            m_transTarget = _target;
        else
            m_transTarget = m_transForward;

        //임시 (입력 처리하는 필요성 느낌) 
        SetMagic(E_SKILL.FORWARD);
        SkillAin();
    }

    public void SetShotDist() {
        m_fDestroyTime = m_fShotDist / m_fShotSpeed;
    }

    Skill SearchSkill(string _skillName) {
        for(int i = 0; i < skills.Length; i++) {
            if(skills[i].skillName == _skillName) {
                return skills[i];
            }
        }
        Debug.LogError(_skillName + " : Not Skill");
        return null;
    }
    public void SetMagic(E_SKILL _magic) {
        switch(_magic) {
            case E_SKILL.FORWARD:
                m_ani.SetBool("shot", true);

                Skill skill = SearchSkill("MagicBall");

                GameObject go_skill = MakeOBJ("MagicBall");

                go_skill.SetActive(true);
                go_skill.transform.position = m_transProjectilePos.position;
                go_skill.transform.rotation = Quaternion.identity;
                go_skill.transform.LookAt(m_transTarget.transform);
                m_nUseMp = skill.useMp;
                m_nDamage = GameManager.GetInstance().DamageSum(skill);
                m_fShotSpeed = skill.Speed;
                m_fShotDist = skill.Dist;
                m_fShotDelay = skill.Delay;

                SkillController control = go_skill.GetComponent<SkillController>();
                control.SetSkill = skill;

                SetShotDist();

                StartCoroutine(TimmerProcess(m_fDestroyTime, skill, go_skill));


                /*
                GameObject objBall = MakeOBJ("MagicBall");
                objBall.SetActive(true);
                objBall.transform.position = m_transProjectilePos.position;
                objBall.transform.rotation = Quaternion.identity;
                objBall.transform.LookAt(m_transTarget.transform);
                m_nUseMp = m_cMagicBall.UseMp;
                m_nDamage = m_cMagicBall.Damage;
                m_fShotSpeed = m_cMagicBall.Speed;
                m_fShotDist = m_cMagicBall.Dist;
                m_fShotDelay = m_cMagicBall.Delay;
                SetShotDist();
                
                StartCoroutine(ProcessTimmer(m_fDestroyTime, m_cMagicBall, objBall));
                */
                break;
        }
        m_eSkill = _magic;
    }

    // 속도와 유지시간을 계산하여 오븢젝트 온오프
    IEnumerator TimmerProcess(float _time, Skill _skill, GameObject _go) {
        _skill.ProjectileMove = true;
        yield return new WaitForSeconds(_time);
        _skill.ProjectileMove = false;
        _go.gameObject.SetActive(false);
    }



    public void SkillAin() {
        switch(m_eSkill) {
            case E_SKILL.FORWARD:
                StartCoroutine(DelayProcess());
                break;
        }
    }

    /*********************************************************************************/

    // [ 애니메이션 ]

    // 투사체_정면
    IEnumerator DelayProcess() {
        yield return new WaitForSeconds(m_fShotDelay);
        m_ani.SetBool("shot", false);
        m_bAttack = false;
    }

    // 설치형


}
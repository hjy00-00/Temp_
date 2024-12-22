using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour {
    [SerializeField] Skill m_cSkill;

    SkillManager m_cSkillManager;
    public Skill SetSkill { set { m_cSkill = value; } }
    /*********************************************************************************/
    private void Start() {
        m_cSkillManager = GameManager.GetInstance().SkillManager;
    }
    private void Update() {
        if(m_cSkill.ProjectileMove) {
            transform.Translate(Vector3.forward * m_cSkill.Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider _other) {
        Debug.Log(m_cSkillManager.Damage);
        switch (_other.tag) {
            case "Player":
                Player player = _other.GetComponent<Player>();
                player.Damage(m_cSkillManager.Damage);
                break;
        }
        this.gameObject.SetActive(false);
    }
}

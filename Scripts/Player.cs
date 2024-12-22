using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	const int STR_ATK = 3, STR_DEF = 1,
				CON_MaxHP = 5,
				INT_M_ATK = 5;
	const float AGI_MOVE = 0.1f, AGI_ATTACK = 0.1f;
	const int WIS_MaxMP = 10, WIS_M_DEF = 3;

	[SerializeField] float m_fSpeed = 5.0f;              //이동 속도
	[SerializeField] float m_fMouseRoatation = 720.0f;   //회전 속도(마우스)
	[SerializeField] float m_rotation = 0.0f;            //플레이어의 회전 각도
	[SerializeField] float m_fWheel = -2.0f;

	[SerializeField] GameObject go_ReSpawnPos;
	public enum E_LOCATION { TOWN, DUNGEON };
	[SerializeField] E_LOCATION m_eLocation;
	public enum PLAYER_RANK { F, E, D, C, B, A, S, SS, SSS };
	[SerializeField] PLAYER_RANK m_eRank;

	[SerializeField] string m_strName;
	[SerializeField] Status m_sStatus;
	[SerializeField] int m_nLevel;
	[SerializeField] int m_nMoney;
	[SerializeField] int m_nLevelUpPoint;
	[SerializeField] bool m_bMpAttack;

	[SerializeField] Inventory m_cInventory;

	GUIManager m_cGuiManager;
	SkillManager m_cSkillManager;
	LevelUpManager m_cLevelupManager;
	EquipmentSlot m_cEquipmentSlot;


	[SerializeField] Animator m_animator;
	/****************************************/
	public Inventory GetInventory { get { return m_cInventory; } }

	public float MouseRoatation { get { return m_fMouseRoatation; } }
	public float Rotation { get { return m_rotation; } set { m_rotation = value; } }
	public float Wheel { get { return m_fWheel; } set { m_fWheel = value; } }
	public GameObject ReSpawnPos { get { return go_ReSpawnPos; } }
	public float MoveSpeed { get { return m_fSpeed; } }
	public E_LOCATION PlayerLocation { get { return m_eLocation; } set { m_eLocation = value; } }

	public PLAYER_RANK PlayerRank { get { return m_eRank; } set { m_eRank = value; } }
	public string Name { get { return m_strName; } set { m_strName = value; } }
	public Status PlayerStatus { get { return m_sStatus; } set { m_sStatus = value; } }
	public int Level { get { return m_nLevel; } set { m_nLevel = value; } }
	public int Money { get { return m_nMoney; } set { m_nMoney = value; } }
	public int LevelUpPoint { get { return m_nLevelUpPoint; } set { m_nLevelUpPoint = value; } }

	public Transform GetTransform { get { return this.transform; } }

	private void Start() {
		m_cGuiManager = GameManager.GetInstance().GUIManager;
		m_cSkillManager = GameManager.GetInstance().SkillManager;
		m_cLevelupManager = GameManager.GetInstance().LevelUpManager;
		m_cEquipmentSlot = GameManager.GetInstance().EquipmentSlot;

		m_animator = GetComponent<Animator>();
	}
	/********************************************************************************/
	public string ToStatus() {
		int maxHP, maxMP, atk, m_atk, def, m_def, critical = 0;

		maxHP = (m_sStatus.nCon * CON_MaxHP);
		maxHP += (m_cEquipmentSlot.StatusEquipment.nCon * CON_MaxHP);
		m_sStatus.nMaxHP = maxHP;
		maxMP = (m_sStatus.nWis * WIS_MaxMP);
		maxMP += (m_cEquipmentSlot.StatusEquipment.nWis * WIS_MaxMP);
		m_sStatus.nMaxMP = maxMP;

		atk = (m_sStatus.nStr + m_cEquipmentSlot.StatusEquipment.nStr) * STR_ATK;
		atk += m_cEquipmentSlot.StatusEquipment.nATK;
		m_sStatus.nATK = atk;
		m_atk = (m_sStatus.nInt + m_cEquipmentSlot.StatusEquipment.nInt) * INT_M_ATK;
		m_atk += m_cEquipmentSlot.StatusEquipment.nM_ATK;
		m_sStatus.nM_ATK = m_atk;

		def = (m_sStatus.nStr + m_cEquipmentSlot.StatusEquipment.nStr) * STR_DEF;
		def += m_cEquipmentSlot.StatusEquipment.nDEF;
		m_sStatus.nDEF = def;

		m_def = (m_sStatus.nWis + m_cEquipmentSlot.StatusEquipment.nWis) * WIS_M_DEF;
		m_def += m_cEquipmentSlot.StatusEquipment.nM_DEF;
		m_sStatus.nDEF = m_def;

		critical = m_sStatus.nCritical + m_cEquipmentSlot.StatusEquipment.nCritical;

		return string.Format(
			"HP:{0} / {1} ({2} + {3})\n" +
			"MP:{4} / {5} ({6} + {7})\n" +
			"EXP:{8} / {9}\n\n" +

			"ATK:{10}\n" +
			"M_ATK:{11}\n" +
			"DEF:{12}\n" +
			"M_DEF:{13}\n" +
			"Critical:{14}\n\n" +

			"Str[근력]:{15} ({16} + {17})\n" +
			"Con[건강]:{18} ({19} + {20})\n" +
			"Int[마력]:{21} ({22} + {23})\n" +
			"Agi[민첩]:{24} ({25} + {26})\n" +
			"Luk[행운]:{27} ({28} + {29})\n" +
			"Wis[지혜]:{30} ({31} + {32})",

			m_sStatus.nHP, maxHP, m_sStatus.nMaxHP, m_cEquipmentSlot.StatusEquipment.nMaxHP + (m_cEquipmentSlot.StatusEquipment.nCon * CON_MaxHP),
			m_sStatus.nMP, maxMP, m_sStatus.nMaxMP, m_cEquipmentSlot.StatusEquipment.nMaxMP + (m_cEquipmentSlot.StatusEquipment.nWis * WIS_MaxMP),
			m_sStatus.nEXP, m_sStatus.nMaxEXP,
			atk, m_atk, def, m_def, critical,

			m_sStatus.nStr + m_cEquipmentSlot.StatusEquipment.nStr, m_sStatus.nStr, m_cEquipmentSlot.StatusEquipment.nStr,
			m_sStatus.nCon + m_cEquipmentSlot.StatusEquipment.nCon, m_sStatus.nCon, m_cEquipmentSlot.StatusEquipment.nCon,
			m_sStatus.nInt + m_cEquipmentSlot.StatusEquipment.nInt, m_sStatus.nInt, m_cEquipmentSlot.StatusEquipment.nInt,
			m_sStatus.nAgi + m_cEquipmentSlot.StatusEquipment.nAgi, m_sStatus.nAgi, m_cEquipmentSlot.StatusEquipment.nAgi,
			m_sStatus.nLuk + m_cEquipmentSlot.StatusEquipment.nLuk, m_sStatus.nLuk, m_cEquipmentSlot.StatusEquipment.nLuk,
			m_sStatus.nWis + m_cEquipmentSlot.StatusEquipment.nWis, m_sStatus.nWis, m_cEquipmentSlot.StatusEquipment.nWis);
	}

	//전투
	public bool HpCheck() {
		if(m_sStatus.nHP >= 0) {
			return true;
		}
		else
			return false;
	}
	public bool MpCheck(int _mp) {
		if(m_sStatus.nMP >= _mp) {
			return true;
		}
		else
			return false;
	}


	public void Damage(int _damage) {
		m_sStatus.nHP -= _damage;
	}
	public void UseMp(int _mp) {
		m_sStatus.nMP -= _mp;

	}

	public void UsePotin(int _hp, int _mp) {
		m_sStatus.nHP += _hp;
		m_sStatus.nMP += _mp;
		RecoveryCheck();
	}
	public void RecoveryCheck() {
		if(m_sStatus.nHP > m_sStatus.nMaxHP) {
			m_sStatus.nHP = m_sStatus.nMaxHP;
		}
		if(m_sStatus.nMP > m_sStatus.nMaxMP) {
			m_sStatus.nMP = m_sStatus.nMaxMP;
		}
	}

	public void GetMoney(int _money) {
		m_nMoney += _money;
	}
	public void GetExp(int _exp) {
		m_sStatus.nEXP += _exp;
		LevelUpCheck();
	}
	public void LevelUpCheck() {
		int temp = 0;
		while(m_sStatus.nEXP >= m_sStatus.nMaxEXP) {
			m_nLevel++;
			m_sStatus.nEXP -= m_sStatus.nMaxEXP;
			m_sStatus.nMaxEXP *= 2;
			m_nLevelUpPoint++;
			m_cLevelupManager.Shuffle = true;

			if(temp >= 10) {
				break;
			}
		}
		if(m_nLevelUpPoint > 0)
			m_cGuiManager.SetGUIPopup(GUIManager.E_GUI_STATE.LEVELUP);

	}
	public void UpStats(int _stats, int _num) {
		switch(_stats) {
			case 1: m_sStatus.nStr += _num; break;
			case 2: m_sStatus.nCon += _num; break;
			case 3: m_sStatus.nInt += _num; break;
			case 4: m_sStatus.nAgi += _num; break;
			case 5: m_sStatus.nLuk += _num; break;
			case 6: m_sStatus.nWis += _num; break;
		}
	}
	public void UpStats(Status _status) {
		m_sStatus += _status;
	}
}
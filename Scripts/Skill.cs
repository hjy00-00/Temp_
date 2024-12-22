using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New/skill")]
public class Skill : ScriptableObject {
	public string skillName;
	public SKILL_ATTACK_TYPE skillAttackType;
	public SKILL_TYPE skillType;
	public SKILL_ATTRIBUTE skillAttribute;
	public Sprite skillImage;
	public GameObject skillPrefab;
	public int skillMaxLv = 1;

	public int useHp;
	public int useMp;
	public float x = 1;
	[SerializeField][TextArea] string skillInfo;

	public bool ProjectileMove;		// 투사체 이동 제어
	public float Speed;             // 투사체 속도
	public float Dist;				// 투사체 거리
	public float Delay;				// 투사체 재공격 딜레이

	public enum SKILL_ATTACK_TYPE { PHYSICAL, MAGIC, MIX }
	public enum SKILL_TYPE { ACTIVE, PASSIVE, BUFF, CURE }
	public enum SKILL_ATTRIBUTE { NONE = -1, AQUA, DARK, ELEC, FIRE, LIGHT, SOIL, WIND }

	public string SkillInfo { get { return skillInfo; } }

	/*********************************************************************************/

	public Skill(SKILL_TYPE _type, SKILL_ATTRIBUTE _Attribute, string _name, int _useHP, int _useMP, float _x, string _info, string _icon) {
		skillType = _type;
		skillAttribute = _Attribute;
		skillName = _name;
		useHp = _useHP;
		useMp = _useMP;
		x = _x;
		skillInfo = _info;
		skillImage = Resources.Load<Sprite>("RPG_inventory_icons/" + _icon);
	}
}
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

	public bool ProjectileMove;		// ����ü �̵� ����
	public float Speed;             // ����ü �ӵ�
	public float Dist;				// ����ü �Ÿ�
	public float Delay;				// ����ü ����� ������

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
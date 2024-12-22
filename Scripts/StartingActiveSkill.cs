using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingActiveSkill : MonoBehaviour {
    [SerializeField] Skill[] StartingSkills;

    public Skill ShuffleSkill() {
        int num = Random.Range(0, StartingSkills.Length);
        return StartingSkills[num];
    }
}
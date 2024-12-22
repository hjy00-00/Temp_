using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue {
    public string dialogue;
    public Sprite cg;
}
public class GUIDialogue : MonoBehaviour {
    [SerializeField] Image image_StandingCG;
    [SerializeField] Image image_DialogueBox;
    [SerializeField] Text text_Dialogue;

    [SerializeField] bool isDialogue = false;
    int count = 0;

    [SerializeField] Dialogue[] dialogue;

    GUIManager guiManager;
    /********************************************************************************/
    private void Start() {
        guiManager = GameManager.GetInstance().GUIManager;
    }
    void Update() {
        if(isDialogue) {
            if(Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0)) {
                if(count < dialogue.Length && guiManager.TargetNPC.QuestState != NPC.QUEST_STATE.CHOICE)
                    NextDialogue();
                else
                    OnOff(false);
            }
        }
    }
    /********************************************************************************/
    public void ShowDialogue(Dialogue[] _dialogues) {
        dialogue = _dialogues;
        OnOff(true);
        count = 0;

        if(guiManager.TargetNPC.QuestState != NPC.QUEST_STATE.CHOICE)
            NextDialogue();
    }
    public void OnOff(bool _flag) {
        image_DialogueBox.gameObject.SetActive(_flag);
        image_StandingCG.gameObject.SetActive(_flag);
        text_Dialogue.gameObject.SetActive(_flag);
        isDialogue = _flag;

        GameManager.GetInstance().GUIManager.SetGUIPopup(GUIManager.E_GUI_STATE.DIALOGUE);
    }

    void NextDialogue() {
        text_Dialogue.text = dialogue[count].dialogue;
        image_StandingCG.sprite = dialogue[count].cg;
        count++;
    }
}
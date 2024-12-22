using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStatus : MonoBehaviour {
    [SerializeField] Text m_textName;
    [SerializeField] Text m_textLevel;
    [SerializeField] Text m_textStatus;

    public Text Name { get { return m_textName; } }
    public Text Level { get { return m_textLevel; } }
    public Text Status { get { return m_textStatus; } }

    public void Initialize(Player _player) {
        m_textName.text = _player.Name;
        m_textLevel.text = string.Format("Level.{0}", _player.Level);
        m_textStatus.text = _player.ToStatus();
    }
}
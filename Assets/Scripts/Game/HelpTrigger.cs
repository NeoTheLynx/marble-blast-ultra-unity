using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTrigger : MonoBehaviour
{
    public AudioClip helpTutorialSfx;
    [TextArea(2, 10)] public string helpText;

    private Translations stringTable;


    public void TriggerEnter()
    {
        stringTable = GameObject.Find("TranslationObject").GetComponent<Translations>();
        GameManager.instance.PlayAudioClip(helpTutorialSfx);
        GameUIManager.instance.SetCenterText(stringTable.getValue(helpText));
    }
}

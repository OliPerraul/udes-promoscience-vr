using UnityEngine;
using UnityEngine.UI;

public class LocalizeText : MonoBehaviour
{
    [SerializeField]
    ScriptableLocalizeString localizeString;

    [SerializeField]
    Text text;

    void Start ()
    {
        text.text = localizeString.Value;
	}
	
}

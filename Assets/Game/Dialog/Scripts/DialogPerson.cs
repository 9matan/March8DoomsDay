using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogPerson : MonoBehaviour
{

	public string pname;
	public Text uiText;
	public Image uiTextBckg;
	public Text uiName;

	void Awake()
	{
		uiName.text = pname;
    }

	public void SetTest()
	{
		SetText("dddddddddddddddddddddddddd df           46             adsffaf  fasdf g fh fhf h jgj ");
    }

	public void SetText(string str)
	{
		uiText.text = str;

		uiTextBckg.rectTransform.SetHeight(LayoutUtility.GetPreferredHeight(uiText.rectTransform));
		uiTextBckg.rectTransform.SetHeight(LayoutUtility.GetPreferredWidth(uiText.rectTransform));

	}

}

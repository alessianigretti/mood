using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonResizer : MonoBehaviour
{
	public TextMeshProUGUI textLabel;
	public float minSize = 73f;
	public float offset = 5f;
	
	void Start()
	{
		float currentX = GetComponent<RectTransform>().rect.x;
		float currentY = GetComponent<RectTransform>().rect.y;
		float currentWidth = GetComponent<RectTransform>().rect.width;

		if (textLabel.rectTransform.rect.height > minSize)
			GetComponent<RectTransform>().rect.Set(currentX, currentY, currentWidth, textLabel.rectTransform.rect.height + offset);
	}
}

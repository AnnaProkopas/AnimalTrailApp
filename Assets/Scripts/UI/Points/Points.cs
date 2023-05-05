using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Text text;
    [SerializeField] private Animator animator;
    [SerializeField] private PointVariation incObject;
    [SerializeField] private PointVariation decObject;
    
    [SerializeField] private int alertValue;

    private int value;

    public void AnimatedChange(int newValue, int variation)
    {
        if (variation > 0)
        {
            var copyObject = Instantiate(incObject, rectTransform.position, Quaternion.identity);
            copyObject.transform.SetParent(rectTransform);
            copyObject.UpdateText(variation.ToString());
        }
        else if (variation < 0)
        {
            var copyObject = Instantiate(decObject, rectTransform.position, Quaternion.identity);
            copyObject.transform.SetParent(rectTransform);
            copyObject.UpdateText((-1 * variation).ToString());
        }

        value = newValue;
        UpdateText();
    }

    public void HiddenChange(int newValue)
    {
        value = newValue;
        UpdateText();
    }
    private void UpdateText()
    {
        text.text = value.ToString();

        if (value <= alertValue)
        {
            animator.SetBool("alert", true);
        } else {
            animator.SetBool("alert", false);
        }
    }
}

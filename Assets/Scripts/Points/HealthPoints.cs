using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    // [SerializeField] private RectTransform rectTransform;
    [SerializeField] private HealthPoint[] points;
    // [SerializeField] private PointVariation incObject;
    // [SerializeField] private PointVariation decObject;
    
    [SerializeField] private int alertValue;

    private int value;

    public void AnimatedChange(int newValue, int variation)
    {
        // if (variation > 0)
        // {
        //     var copyObject = Instantiate(incObject, rectTransform.position, Quaternion.identity);
        //     copyObject.transform.SetParent(rectTransform);
        //     copyObject.UpdateText(variation.ToString());
        // }
        // else if (variation < 0)
        // {
        //     var copyObject = Instantiate(decObject, rectTransform.position, Quaternion.identity);
        //     copyObject.transform.SetParent(rectTransform);
        //     copyObject.UpdateText((-1 * variation).ToString());
        // }

        HiddenChange(newValue);
    }

    public void HiddenChange(int newValue)
    {
        value = newValue;
        UpdateView();
    }
    private void UpdateView()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i < value / 2)
                points[i].ChangeValue(PointState.Full, PointDisabledState.None);
            else if (i == value / 2 && value % 2 == 1)
                points[i].ChangeValue(PointState.Half, PointDisabledState.None);
            else
                points[i].ChangeValue(PointState.Empty, PointDisabledState.None); 
        }
    }
}

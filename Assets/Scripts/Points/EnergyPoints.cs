using EventBusModule;
using EventBusModule.Energy;
using UnityEngine;

public class EnergyPoints : MonoBehaviour, IEnergyPointsHandler
{
    [SerializeField] private EnergyPoint[] points;
    
    [SerializeField] private int alertValue;

    private int value;

    private void Start()
    {
        UpdateView();
    }
    
    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }
    
    public void HandleTotalEnergy(int currentValue, int variation, bool isAnimated)
    {
        value = currentValue;

        if (isAnimated)
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
        }
        
        UpdateView();
    }

    private void UpdateView()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i < value / 2)
                points[i].ChangeValue(PointState.Full);
            else if (i == value / 2 && value % 2 == 1)
                points[i].ChangeValue(PointState.Half);
            else
                points[i].ChangeValue(PointState.Empty); 
        }
    }

}

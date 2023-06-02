using UnityEngine;
using UnityEngine.EventSystems;

public class Joybutton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    
    [SerializeField] private Player player;

    [SerializeField] private GameObject greenInputButton;
    [SerializeField] private GameObject yellowInputButton;
    [SerializeField] private GameObject redInputButton;

    private void Start()
    {
        player.setActiveCustomJoyButton += SetEnableInputButton;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        player.EnableAttackMode();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.DisableAttackMode();
    }

    private void SetEnableInputButton(JoyButtonState state)
    {
        greenInputButton.SetActive(false);
        yellowInputButton.SetActive(false);
        redInputButton.SetActive(false);

        switch (state)
        {
            case JoyButtonState.Hide:
                greenInputButton.SetActive(true);
                break;
            case JoyButtonState.Human:
                yellowInputButton.SetActive(true);
                break;
            case JoyButtonState.Eat:
                redInputButton.SetActive(true);
                break;
        }
    }
}

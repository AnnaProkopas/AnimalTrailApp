using UnityEngine;
using UnityEngine.EventSystems;

public class Joybutton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    
    [SerializeField] private Player player;

    [SerializeField] private GameObject greenInputButton;

    private void Start()
    {
        player.setActiveGreenJoyButton += SetEnableInputButton;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        player.EnableAttackMode();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.DisableAttackMode();
    }

    private void SetEnableInputButton(bool active)
    {
        greenInputButton.SetActive(active);
    }
}

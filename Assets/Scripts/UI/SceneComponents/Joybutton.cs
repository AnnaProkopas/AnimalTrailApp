using UnityEngine;
using UnityEngine.EventSystems;

public class Joybutton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] 
    private Player player;

    public void OnPointerDown(PointerEventData eventData)
    {
        player.EnableAttackMode();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.DisableAttackMode();
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class Shield : MonoBehaviour, IPointerDownHandler, IPointerUpHandler //IUpdateSelectedHandler
{
    private PlayerController _playerController;
    private float _fixedTime;
    
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _playerController.SetGreenState(true);
        _fixedTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (_playerController.GetGreenState())
        {
            if (Mathf.Abs(Time.time - _fixedTime) > 2f) _playerController.SetGreenState(false);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        _playerController.SetGreenState(false);
    }

    public void SetPlayerController(PlayerController pc)
    {
        _playerController = pc;
    }
}

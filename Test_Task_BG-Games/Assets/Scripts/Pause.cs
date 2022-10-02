using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private Animator _animatorBlackScreen;
    [SerializeField] private Animator _animatorContinue;
    public void PauseGame()
    {
        ActivateAnimations(false);
        _animatorBlackScreen.Play("BlackScreenPopUp4Pause");
        _animatorContinue.Play("ContinueAnimation");
    }

    public void ResumeGame()
    {
        ActivateAnimations(true);
    }

    private void ActivateAnimations(bool b)
    {
        _playerController.SetAgentEnabled(b);
        _animatorBlackScreen.gameObject.SetActive(!b);
        _animatorContinue.gameObject.SetActive(!b);
    }

    public void SetPlayerController(PlayerController pc)
    {
        _playerController = pc;
    }
}

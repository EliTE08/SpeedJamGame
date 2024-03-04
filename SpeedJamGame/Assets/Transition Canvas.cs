using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionCanvas : Singleton<TransitionCanvas>
{
    [SerializeField] private Image fader;
    [SerializeField] private float speed = 0.6f;

    public void Transition(string sceneName)
    {
        fader.DOFade(1f, speed).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
            fader.DOFade(0f, speed);
        });
    }
}

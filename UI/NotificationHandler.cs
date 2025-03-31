using UnityEngine;
using TMPro;
using DG.Tweening;

public class NotificationHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    public float fadeInDuration = 0.3f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 0.5f;
    public float slideDistance = 50f;

    private void Start()
    {

    }

    public void DisplayMessage(string text)
    {
        // Set the message text
        messageText.text = text;

        // Reset initial state
        canvasGroup.alpha = 0f;
        Vector3 startPos = transform.localPosition - new Vector3(0, slideDistance, 0);
        transform.localPosition = startPos;

        // Create and execute the animation sequence
        Sequence sequence = DOTween.Sequence();

        // Fade in and slide up
        sequence.Append(canvasGroup.DOFade(1f, fadeInDuration));
        sequence.Join(transform.DOLocalMoveY(startPos.y + slideDistance, fadeInDuration)
            .SetEase(Ease.OutBack));

        // Hold the message
        sequence.AppendInterval(displayDuration);

        // Fade out
        sequence.Append(canvasGroup.DOFade(0f, fadeOutDuration));

        // Destroy when done
        sequence.OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        // Kill any active tweens
        DOTween.Kill(transform);
        DOTween.Kill(canvasGroup);
    }
}
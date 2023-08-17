using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextEffect : MonoBehaviour
{
    [SerializeField] float fadeDuratioin = 5f;
    [SerializeField] float yAmount = 1f;

    private TMP_Text thisText = null;

    private void OnEnable()
    {
        if(thisText == null) thisText = GetComponent<TMP_Text>();
        Destroy(gameObject, 1f);
        transform.DOMoveY(transform.position.y + yAmount, fadeDuratioin);
        thisText.DOFade(0f, fadeDuratioin); 
    }
}

using UnityEngine;

public class TestHeart : MonoBehaviour
{
    [SerializeField] float fillAmount = 0.1f;
    [SerializeField] GameObject textEffect = null;
    [SerializeField] Canvas canvas = null;
    private Transform mask = null;

    private void Start()
    {
        mask = transform.GetChild(0);
    }

    private void OnMouseDown()
    {
        Vector3 maskScale = mask.localScale;
        maskScale.y -= fillAmount;

        Vector3 maskPos = mask.position;
        maskPos.y += fillAmount * 0.5f;

        mask.position = maskPos;
        mask.localScale = maskScale;

        GameObject g = Instantiate(textEffect, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        g.transform.SetParent(canvas.transform);
        g.transform.localScale = Vector3.one;
        g.SetActive(true);
    }
}

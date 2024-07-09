using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHoverEffect : MonoBehaviour
{
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;
    private int layer;
    public Vector3 hoverScale = new Vector3(0.3f, 0.3f, 1.3f); // 放大比例
    public float scaleSpeed = 10f; // 放大速度


    void Start()
    {
        originalScale = transform.localScale; // 记录初始大小
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleToSize_bigger(hoverScale)); // 鼠标悬停时放大
    }

    void OnMouseExit()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleToSize_smaller(originalScale)); // 鼠标移开时恢复原始大小
    }

    IEnumerator ScaleToSize_bigger(Vector3 targetScale)
    {   

 
        layer = spriteRenderer.sortingOrder;
        spriteRenderer.sortingOrder = 20;


        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            yield return null;
        }
        transform.localScale = targetScale; // 确保完全缩放到目标大小
    }


        IEnumerator ScaleToSize_smaller(Vector3 targetScale)
    {   
        spriteRenderer.sortingOrder = layer;


        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            yield return null;
        }
        transform.localScale = targetScale; // 确保完全缩放到目标大小
    }

}
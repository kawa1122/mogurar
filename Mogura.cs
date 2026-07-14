using System.Collections;
using UnityEngine;

public class Mogura : MonoBehaviour
{
    [Header("出現設定")]
    public float minWait = 1f;
    public float maxWait = 3f;
    public float upTime = 2f;

    Vector3 downPos;
    Vector3 upPos;

    bool isUp = false;
    bool isHit = false;

    Renderer rend;

    Color normalColor = Color.white;
    Color hitColor = Color.red;

    Coroutine currentRoutine;

    void Start()
    {
        rend = GetComponent<Renderer>();

        downPos = transform.position;
        upPos = downPos + Vector3.up * 1.5f;

        rend.material.color = normalColor;

        currentRoutine = StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (!GameManager.instance.IsGameOver())
        {
            // ランダム待機
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            if (GameManager.instance.IsGameOver())
                yield break;

            // 出現
            transform.position = upPos;
            rend.material.color = normalColor;

            isUp = true;
            isHit = false;

            yield return new WaitForSeconds(upTime);

            if (GameManager.instance.IsGameOver())
                yield break;

            // 引っ込む
            transform.position = downPos;
            isUp = false;
        }

        // ゲーム終了時は必ず引っ込める
        transform.position = downPos;
        isUp = false;
    }

    void OnMouseDown()
    {
        if (GameManager.instance.IsGameOver())
        {
            return;
        }
        if (isUp && !isHit)
        {
            isHit = true;

            // 赤色に変更
            rend.material.color = hitColor;

            // スコア加算
            GameManager.instance.AddScore(1);

            // 叩かれたら即引っ込む
            StartCoroutine(HideAfterHit());
        }
    }

    IEnumerator HideAfterHit()
    {
        yield return new WaitForSeconds(0.1f);

        transform.position = downPos;
        isUp = false;
    }
    void Update()
    {
        if (GameManager.instance.IsGameOver())
        {
            transform.position = downPos;
            isUp = false;
        }
    }
}
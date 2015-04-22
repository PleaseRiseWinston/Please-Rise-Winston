using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Random = UnityEngine.Random;

public class Headbobber : MonoBehaviour
{
    float bobbingSpeed = 4.5f;
    float horizontalBobbingAmount = 0.7f;
    float verticalBobbingAmount = 1.5f;
    float depthBobbingAmount = 1.5f;
    Vector3 midpoint;

    void Start()
    {
        midpoint = GameObject.FindGameObjectWithTag("GameCamera").transform.localPosition;

        StartCoroutine(BobUp());
    }

    IEnumerator BobUp()
    {
        yield return StartCoroutine(HOTween.To(transform, bobbingSpeed, "position", midpoint + new Vector3(Random.Range(0, horizontalBobbingAmount), Random.Range(0, verticalBobbingAmount), Random.Range(0, depthBobbingAmount)), false).WaitForCompletion());
        yield return new WaitForSeconds(2f);
        StartCoroutine(BobUp());
    }
}

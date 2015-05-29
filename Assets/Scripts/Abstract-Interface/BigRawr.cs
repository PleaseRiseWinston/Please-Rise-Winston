using UnityEngine;
using System.Collections;

public class BigRawr : Rawr
{
    public override void Move()
    {
        StartCoroutine(MoveLeft());
    }

    public override void SetColor()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }

    IEnumerator MoveLeft()
    {
        while (true)
        {
            transform.Translate(new Vector3(5f * Time.deltaTime, 0, 0));
            yield return null;
        }
    }
}

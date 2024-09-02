using System;
using UnityEngine;

public class LimitPosition : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var distanceZ = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceZ)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceZ)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceZ)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distanceZ)).y;
       
        transform.position = new Vector3(
            Math.Clamp(transform.position.x, leftBorder, rightBorder),
            Math.Clamp(transform.position.y, topBorder, bottomBorder),
            transform.position.z);
    }
}

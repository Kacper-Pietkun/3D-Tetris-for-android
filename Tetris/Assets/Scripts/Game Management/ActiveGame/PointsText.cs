using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsText : MonoBehaviour
{
    [SerializeField]
    private Text pointsText;

    private void Update()
    {
        pointsText.text = "Points: " + ActiveGame.points;
    }
}

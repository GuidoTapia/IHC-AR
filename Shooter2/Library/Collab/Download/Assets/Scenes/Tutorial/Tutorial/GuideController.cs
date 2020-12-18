using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuideController : MonoBehaviour
{
    public void nextButton()
    {
        SceneManager.LoadScene("Menu");
    }
}

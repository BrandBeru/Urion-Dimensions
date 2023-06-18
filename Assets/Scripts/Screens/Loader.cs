using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static string nextLevel;

    public static void Charging(string name)
    {
        nextLevel = name;
        SceneManager.LoadScene("ScreenLoader");
    }
}

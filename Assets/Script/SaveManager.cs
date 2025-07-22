using UnityEngine;

public static class SaveManager
{
    public static void SaveScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
    }

    public static int LoadScore() => PlayerPrefs.GetInt("score", 0);
}

using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WinLooseScript : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public TMP_Text WinText;
    public TMP_Text LooseText;
    public GameObject EndScreen;
    public GameObject Player;

    public void AddEnemi(GameObject enemy)
    {
        Enemies.Add(enemy);
    }
    public void RemoveEnemi(GameObject Enemy)
    {
        Enemies.Remove(Enemy);
    }

    public void WIN()
    {
        Debug.Log("Player wins!!");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EndScreen.SetActive(true);
        WinText.gameObject.SetActive(true);

    }

    public void Loose()
    {
        Debug.Log("Player loose");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EndScreen.SetActive(true);
        LooseText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Enemies.Count  == 0)
        {
            WIN();
        }
        if (Player == null)
        {
            Loose();
        }
        for (int i = Enemies.Count -1; i >=0 ; i--)
        {
            if(Enemies[i] == null)
            {
                Enemies.RemoveAt(i);
            }
        }
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPages;

    private int currentPageIndex = 0;

    void Start()
    {
        // Pause the game at the start
        Time.timeScale = 0;

        // Make sure only the first tutorial page is shown
        ShowCurrentPage();
    }

    void Update()
    {
        // Check for any input
        if (Input.anyKeyDown || Input.touchCount > 0)
        {
            NextPage();
        }
    }

    void NextPage()
    {
        // Increment the page index
        currentPageIndex++;

        if (currentPageIndex < tutorialPages.Length)
        {
            // Show the next page
            ShowCurrentPage();
        }
        else
        {
            // All tutorials shown, resume the game
            EndTutorial();
        }
    }

    void ShowCurrentPage()
    {
        // Hide all pages
        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }

        // Show the current page
        if (currentPageIndex < tutorialPages.Length)
        {
            tutorialPages[currentPageIndex].SetActive(true);
        }
    }

    void EndTutorial()
    {
        // Hide all tutorial pages
        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }

        // Resume the game
        Time.timeScale = 1;
    }
}
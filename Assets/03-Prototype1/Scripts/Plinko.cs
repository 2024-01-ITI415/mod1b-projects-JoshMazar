using UnityEngine;
using System.Collections;
using UnityEngine.UI; // a

public class Plinko : MonoBehaviour
{
    static private Plinko S; // a private Singleton
    [Header("Set in Inspector")]
    public Text uitLevel; // The UIText_Level Text
    public Text uitShots; // The UIText_Shots Text
    public Text uitButton; // The Text on UIButton_View
    public Vector3 machinesPos; // The place to put machines
    public GameObject[] machines; // An array of the machines
    [Header("Set Dynamically")]
    public int level; // The current level
    public int levelMax; // The number of levels
    public int shotsTaken;
    public GameObject machine; // The current machine
    public GameMode mode = GameMode.idle;
    public string showing = "Show BallShooter"; // FollowBall mode
    void Start()
    {
        S = this; // Define the Singleton
        level = 0;
        levelMax = machines.Length;
        StartLevel();
    }
    void StartLevel()
    {
        // Get rid of the old machine if one exists
        if (machine != null)
        {
            Destroy(machine);
        }
        // Destroy old balls if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }
        // Instantiate the new machine
        machine = Instantiate<GameObject>(machines[level]);
        machine.transform.position = machinesPos;
        shotsTaken = 0;
        // Reset the camera
        SwitchView("Show Both");
        // Reset the goal
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
    }
    void UpdateGUI()
    {
        // Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    void Update()
    {
        UpdateGUI();
        // Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            // Change mode to stop checking for level end
            mode = GameMode.levelEnd;
            // Zoom out
            SwitchView("Show Both");
            // Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }
    public void SwitchView(string eView = "")
    { // c
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Ball Shooter":
                FollowBall.POI = null;
                uitButton.text = "Show Machine";
                break;
            case "Show Machine":
                FollowBall.POI = S.machine;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowBall.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Ball Shooter";
                break;
        }
    }
    // Static method that allows code anywhere to increment shotsTaken
    public static void ShotFired()
    { // d
        S.shotsTaken++;
    }
}

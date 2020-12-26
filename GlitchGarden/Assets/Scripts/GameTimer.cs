using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    // Config parameters
    [Tooltip("Level timer in seconds.")]
    [SerializeField] float levelTime = 10f;

    // Cached References
    Slider slider;
    Animator animator;
    LevelController levelController;

    // State variables
    bool timerExpired = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        slider = GetComponent<Slider>();

        levelController = FindObjectOfType<LevelController>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (timerExpired) { return; }
        
        slider.value = Time.timeSinceLevelLoad / levelTime;

        timerExpired = (Time.timeSinceLevelLoad >= levelTime);

        if (timerExpired)
        {
            animator.enabled = false;

            levelController.TimerExpired();
        }

    }
}

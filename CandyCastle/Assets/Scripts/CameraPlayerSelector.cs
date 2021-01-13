using UnityEngine;

public class CameraPlayerSelector : MonoBehaviour
{
    bool isGameSessionFound = false;
    Player player;

    // Update is called once per frame
    void Update()
    {
        if (!isGameSessionFound)
        {
            foreach (Player tempPlayer in FindObjectsOfType<Player>())
            {
                if (tempPlayer.AmIActive())
                {
                    player = tempPlayer;
                    break;
                }
            }

            GetComponent<Cinemachine.CinemachineStateDrivenCamera>().m_AnimatedTarget = player.GetComponent<Animator>();

            Cinemachine.CinemachineVirtualCamera[] vcams = GetComponentsInChildren<Cinemachine.CinemachineVirtualCamera>();

            foreach (Cinemachine.CinemachineVirtualCamera vcam in vcams)
            {
                vcam.m_Follow = player.transform;
            }

            isGameSessionFound = true;
        }
        
    }
}

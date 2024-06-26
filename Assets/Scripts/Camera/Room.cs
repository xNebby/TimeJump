using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject VirtualCamera;
    public static float CameraBlendTime = 1f;
    public int RoomID;
    public Vector2 RoomPosition;
    public List<int> AdjacentRooms;
    public bool PlayerBounce;
    public Vector2 PlayerBounceVector;
    private float PlayerPauseDuration;

    private void OnEnable()
    {
        PlayerPauseDuration = 1f;
        VirtualCamera = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (PlayerBounce)
            {
                MovementStatusManager.Instance.AddTimedMovementEffect("RoomChange", PlayerBounceVector, PlayerPauseDuration);
            } else
            {
                MovementStatusManager.Instance.AddTimedMovementEffect("RoomChange", 0f, PlayerPauseDuration);
            }
            RoomLoader.Instance.LoadRoom(RoomID);
            VirtualCamera.SetActive(true);
            EventManager.TriggerEvent("CAM_UpdateFollow");
            TimerManager.AddTimer("CAM_FinishedTransition", CameraBlendTime, delegate { EventManager.TriggerEvent("CAM_RoomBlended"); });
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            VirtualCamera.SetActive(false);
        }
    }
}

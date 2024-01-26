using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject VirtualCamera;
    public int RoomID;
    public Vector2 RoomPosition;
    public List<int> AdjacentRooms;
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
            MovementStatusManager.Instance.AddTimedMovementEffect("RoomChange", 0f, PlayerPauseDuration);
            RoomLoader.Instance.LoadRoom(RoomID);
            VirtualCamera.SetActive(true);
            EventManager.TriggerEvent("CAM_UpdateFollow");
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

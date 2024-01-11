using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rules for loading rooms:
// All adjacent rooms to the current active room should be loaded
// When a new room is loaded, Any change in rooms should be Deloaded- 
//      Do not Deload then Reload room- this causes flickering.


public class RoomLoader : SingletonClass<RoomLoader>
{
    public Dictionary<int, RoomEntry> Rooms;

    public List<GameObject> Prefabs;

    public List<int> ActiveRooms;

    void OnEnable()
    {
        Rooms = new Dictionary<int, RoomEntry>();
        //EventManager.StartListening()
        IterateRooms();
        if (ActiveRooms.Count == 0)
        {
            LoadRoom(1);
        }
    }
    void IterateRooms()
    {
        foreach (GameObject Entry in Prefabs)
        {
            Room EntryComponent = Entry.GetComponent<Room>();
            RoomEntry TempEntry = new RoomEntry();
            TempEntry.RoomPrefab = Entry;
            TempEntry.RoomPosition = EntryComponent.RoomPosition;
            TempEntry.AdjacentRooms = EntryComponent.AdjacentRooms;
            Rooms.Add(EntryComponent.RoomID, TempEntry);
        }
    }
    void AddRoom(GameObject v_RoomPrefab, Vector2 v_RoomPosition, List<int> v_AdjacentRooms)
    {
        RoomEntry TempEntry = new RoomEntry();
        TempEntry.RoomPrefab = v_RoomPrefab;
        TempEntry.RoomPosition = v_RoomPosition;
        TempEntry.AdjacentRooms = v_AdjacentRooms;
    }
    public void LoadRoom(int RoomID)
    {
        Debug.Log("Loading room:" + RoomID.ToString());
        List<int> TempActiveRooms = new List<int>(Rooms[RoomID].AdjacentRooms);
        if (!TempActiveRooms.Contains(RoomID))
        {
            TempActiveRooms.Add(RoomID);
        }
        List<int> ActiveRoomsClone = new List<int>(ActiveRooms);
        foreach (int TempRoomID in ActiveRoomsClone)
        {
            Debug.Log("ActiveRooms:" + TempRoomID.ToString());
            if (!TempActiveRooms.Contains(TempRoomID))
            {
                ActiveRooms.Remove(TempRoomID);
                Destroy(GameObject.Find("Room" + TempRoomID.ToString() + " (clone)"));
            } else
            {
                TempActiveRooms.Remove(TempRoomID);
            }
        }
        if (TempActiveRooms.Count > 0)
        {
            Debug.Log(TempActiveRooms.Count);
            foreach (int TempRoomID in TempActiveRooms)
            {
                Debug.Log(TempRoomID);
                ActiveRooms.Add(TempRoomID);
                GameObject RoomChild = Instantiate(Rooms[TempRoomID].RoomPrefab, Rooms[TempRoomID].RoomPosition, Quaternion.identity);
                RoomChild.transform.parent = transform;
            }
        }
    }
}

public class RoomEntry
{
    public GameObject RoomPrefab;
    public Vector2 RoomPosition;
    public List<int> AdjacentRooms; // Give the IDs of adjacent rooms per room basis.
                                    // allow for one way room connections via no connection back through a room.
}
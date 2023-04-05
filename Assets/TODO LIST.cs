using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TODOLIST : MonoBehaviour
{
    /*
     *  Fix the rotation when player is on the ground. 
     *  MAke it so that when the player falls, raycast below them in the line of gravity- if they are capable of moving downwards, do so
     *  otherwise, check if there are any contact points- Check if they are at the same rotation to the players current normal. 
     *  if the rotation is then greater than the players, but lower than the walking limit rotation, allow the player to be on a ramp-
     *  otherwise, set the player to be wallsliding. 
     */
}

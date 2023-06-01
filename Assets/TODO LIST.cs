using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TODOLIST : MonoBehaviour
{
    /*
     *  ||
     * 
     *  Fix the rotation when player is on the ground. 
     *  MAke it so that when the player falls, raycast below them in the line of gravity- if they are capable of moving downwards, do so
     *  otherwise, check if there are any contact points- Check if they are at the same rotation to the players current normal. 
     *  if the rotation is then greater than the players, but lower than the walking limit rotation, allow the player to be on a ramp-
     *  otherwise, set the player to be wallsliding. 
     *  
     *  Go over the collision stuff and redo it
     *  
     *  Figure out why there are the jagged shadow artefacts (maybe something to do with resolution?)
     *  
     *  Set up an interaction script which modularly allows me to add an interaction to things. 
     *  
     *  ADD A MAP sometime
     *  make the debug variant of the map let me right click to tp to positions. #
     *  
     *  Random world gen mayhaps?
     *  
     *  Text boxes. Figure them out lol.
     *  
     *  Decide what we're doing with the UI- Dead cells inspired?
     *  
     *  Set up the UI elements for inputs. Bind each visual thing to their actual key :p
     */
}

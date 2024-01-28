// The libraries that this script uses.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Defining the class that the script stores all its methods and properties under.
public class MenuFunctions : MonoBehaviour
{
    // The variables are all given a scope (In this case public) and then define what type they are. The name of the variable can be defined afterwards. 
    public List<Button> Buttons;
    public List<GameObject> GameObjs;
    // The OnEnable method is ran whenever the gameobject this script is attached to is loaded.
    void OnEnable()
    {
        if (Buttons.Count != GameObjs.Count) 
        {
            Debug.LogError("There is a missing gameobject / button input in " + gameObject.name);    
        }
        // Runs through the list, and adds a listener to each entry's button.
        for (int i = 0; i < Buttons.Count; i++)
        {
            //Debug.Log(i);
            ButtonContainer tempContainer = Buttons[i].gameObject.GetComponent<ButtonContainer>();
            tempContainer.IndexOfButton = i;
            // Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button. The delegate allows for parameters to be passed.
            Buttons[i].onClick.AddListener(delegate { LoadMenu(tempContainer.IndexOfButton); });
        }
    }
    // This method loads whenever a button is clicked- a gameobject is passed, and it loads that gameobject. It then deloads the button. 
    void LoadMenu(int v_Index)
    {
        GameObject v_Obj = GameObjs[v_Index];
        // prints the name of the game object.
        Debug.Log(v_Obj.name);
        // Loads the given game object.
        v_Obj.SetActive(true);
        // deloads the current game object.
        gameObject.SetActive(false);
    }
}


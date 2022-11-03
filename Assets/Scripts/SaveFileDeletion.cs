using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileDeletion : MonoBehaviour
{
    // Start is called before the first frame update
    public void SaveFileDelete(int saveSlot)
    {
        GameControl.control.CheckForDeletion(saveSlot);
    }
}

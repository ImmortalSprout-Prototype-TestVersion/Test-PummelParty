using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void OnClickBackButton()
    {
        SceneManager.LoadScene(0);
    }
}

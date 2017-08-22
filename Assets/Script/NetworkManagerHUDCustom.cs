using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerHUDCustom : NetworkManager {

    public override void OnStartHost(){
        Debug.Log("Chamou");
        base.OnStartHost();
    }


}

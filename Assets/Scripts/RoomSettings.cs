using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSettings : MonoBehaviour
{
    private bool isSetup;

    // Start is called before the first frame update
    void Start()
    {
        isSetup = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ÉãÅ[ÉÄê›íËÇçsÇ§
    public void SettingOptions(GameObject[] players, GamePlayer player)
    {
        if (!isSetup)
        {
            if (player.isMine && player.id == 1) // id == 1ÇÃèÍçáÇÃÇ›
            {
                if (GameSettings.isHideSubmarine)
                {
                    player.hideSubmarineOpt = 1;
                }
                else
                {
                    player.hideSubmarineOpt = 2;
                }
                if (GameSettings.isNote)
                {
                    player.noteOpt = 1;
                }
                else
                {
                    player.noteOpt = 2;
                }
                isSetup = true;
            }
            else if (!player.isMine && player.id == 1)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    GameObject tmpPlayerObj = players[i];
                    GamePlayer tmpPlayer = tmpPlayerObj.GetComponent<GamePlayer>();
                    tmpPlayer.noteOpt = player.noteOpt;
                    tmpPlayer.hideSubmarineOpt = player.hideSubmarineOpt;
                }
            }
            if (player.id != 1)
            {
                if (player.noteOpt != 0 && player.hideSubmarineOpt != 0)
                {
                    isSetup = true;
                }

                if (player.noteOpt == 1)
                {
                    GameSettings.isNote = true;
                }
                if (player.hideSubmarineOpt == 1)
                {
                    GameSettings.isHideSubmarine = true;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public bool pack0Unlock = false;
    public bool pack1Unlock = false;
    public bool pack2Unlock = false;
    public bool pack3Unlock = false;
    public bool pack4Unlock = false;
    public bool pack5Unlock = false;
    public bool pack6Unlock = false;
    public bool pack7Unlock = false;

    public bool allPack = false;

    public int highScore = 1;
    public int lastPack = 0;
    public int Money = 0;

    public SaveFile (GameManager gm)
    {
        pack0Unlock = gm.packsEnabled[0];
        pack1Unlock = gm.packsEnabled[1];
        pack2Unlock = gm.packsEnabled[2];
        pack3Unlock = gm.packsEnabled[3];
        pack4Unlock = gm.packsEnabled[4];
        pack5Unlock = gm.packsEnabled[5];
        pack6Unlock = gm.packsEnabled[6];
        pack7Unlock = gm.packsEnabled[7];

        highScore = gm.highScore;
        lastPack = gm.lastPack;
        Money = gm.Money;
        allPack = gm.allPack;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour
{
    ParticleSystem ps;
    int PPU = 16;
    ParticleSystem.MainModule main;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
        ParticleSystemRenderer pr = GetComponent<ParticleSystemRenderer>();
        // print("original size : " + main.startSize);
        int xTiles = ps.textureSheetAnimation.numTilesX;
        // print("x tiles " + ps.textureSheetAnimation.numTilesX);
        main.startSize = new ParticleSystem.MinMaxCurve((float)pr.material.mainTexture.width/xTiles/PPU);
        // print((float)pr.material.mainTexture.width/xTiles/PPU);
        
    }
}

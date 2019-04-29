using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRespawnSequence : GameIntroSequence
{

    new void Start()
    {
        _alienPhrases = new string[] {
                "Died again, eh? Might set you off a bit.. But you can just buy a new ship if you have any lives left, of course."
        };
        _responsePhrases = new string[] {
                "Continue..."
        };

        base.Start();
    }

}

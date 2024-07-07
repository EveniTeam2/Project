using System;
using System.Collections.Generic;
using Unit.Blocks;
using Unit.Character;
using UnityEngine;

public interface IRaceable {
    event Action OnFailed;
    event Action<List<Block>> OnSuccess;
}

public class Race : MonoBehaviour {
    BaseCharacter player;
    BaseCharacter monster;
    IShowable background;

    public void AttachBoard(IRaceable data) {
        data.OnSuccess += OnSuccessMatch;
        data.OnFailed += OnFailedMatch;
    }

    private void OnFailedMatch() {
        background.Stop();
    }

    private void OnSuccessMatch(List<Block> list) {
        background.Move(player.Speed);
        foreach (var block in list) {
            
        }
    }
}

public interface IShowable {
    void Move(float spd);
    void Stop();
}

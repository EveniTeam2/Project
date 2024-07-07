using System;
using System.Collections.Generic;
using Unit.Blocks;
using Unit.Character;
using UnityEngine;

public interface IRaceable {
    event Action OnFailed;
    event Action<List<Block>> OnSuccess;
}

public interface IShowable {
    void Move(float spd);
    void Stop();
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
        // TODO
    }
    private void OnSuccessMatch(List<Block> list) {
        background.Move(player.Speed);
        foreach (var block in list) {
            // TODO
        }
    }
}

using UnityEngine;
using System.Collections;

public interface IDobermanState
{
    void Execute();
    void Enter(Doberman doberman);
    void Exit();
    void OnTriggerEnter(Collider2D other);
}

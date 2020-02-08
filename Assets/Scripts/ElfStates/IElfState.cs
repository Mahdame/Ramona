using UnityEngine;
using System.Collections;

public interface IElfState
{
    void Execute();
    void Enter(Elf elf);
    void Exit();
    void OnTriggerEnter(Collider2D other);
}

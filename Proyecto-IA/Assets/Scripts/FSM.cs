using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM {
    int[,] _machine;
    int _actualState;

    public FSM(int cantEstados, int cantEventos) {
        _machine = new int[cantEstados, cantEventos];
        for (int i = 0; i < cantEstados; i++) {
            for (int j = 0; j < cantEventos; j++) {
                _machine[i, j] = -1;
            }
        }
        _actualState = 0;
    }

    public void SetRelation(int srcEstado, int destino, int evento) {
        _machine[srcEstado, evento] = destino;
    }

    public void SendEvent(int evento) {
        if (_machine[_actualState, evento] != -1) {
            _actualState = _machine[_actualState, evento];
        }
    }
    public int ActualState{
        get { return _actualState; }
    }
}

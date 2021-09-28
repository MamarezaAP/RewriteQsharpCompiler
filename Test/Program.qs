﻿namespace Quantum.Test {
    open Microsoft.Quantum.Intrinsic;

    @EntryPoint()
    operation Main() : Unit {
        let message = "Quantom World";
        Message($"Hello {message}");
    }

    function Add(n : Int, m : Int) : Int {
        return n + m;
    }

    operation BellState(q1 : Qubit, q2 : Qubit) : Unit is Adj {
        H(q1);
        CNOT(q1, q2);
    }
}

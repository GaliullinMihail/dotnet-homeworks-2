.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator extends System.Object
{
    .method public static float64 Calculate
        (float64 value1, valuetype Hw2.CalculatorOperation operation, float64 value2) cil managed
    {
    .maxstack 4
    // load value1
    ldarg.0
    // load value2
    ldarg.2
    
    //addition
    ldarg.1
    ldc.i4 0
    beq.s addOperation
    
    //substract
    ldarg.1
    ldc.i4 1
    beq.s subOperation
    
    //multiply
    ldarg.1
    ldc.i4 2
    beq.s multOperation
    
    //divide
    ldarg.1
    ldc.i4 3
    beq.s divOperation
    
    //undefined
    ldarg.1
    ldc.i4 4
    beq.s undefOperation
    
    addOperation:
    add
    ret
    
    subOperation:
    sub
    ret
    
    multOperation:
    mul
    ret
    
    divOperation:
    div
    ret
    
    undefOperation:
    pop
    newobj  instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
    throw
    }
}
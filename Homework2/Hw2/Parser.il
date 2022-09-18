.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser extends System.Object
{
    .method public static void ParseCalcArguments(string[] args, 
        [out] float64& value1,
        [out] valuetype Hw2.CalculatorOperation& operation,
        [out] float64& value2) 
        cil managed
    {
        //args == null
        ldarg.0
        brfalse.s badArguments
        
        //notSupportedLength
        ldarg.0
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        brfalse.s badArguments
        
        //parsing value1
        ldarg.0
        ldc.i4.0
        ldelem.ref
        ldarg.1
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse.s badArguments
        
        //parsing value2
        ldarg.0
        ldc.i4.2
        ldelem.ref
        ldarg.3
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse.s badArguments
        
        //parsing operation
        ldarg.2
        ldarg.0
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        stind.i1
        
        ret
        
        badArguments:
        newobj  instance void [System.Runtime]System.ArgumentException::.ctor()
        throw
        
    }
    
    .method public static bool IsArgLengthSupported(string[] args) cil managed
    {
        .maxstack 2
        ldarg.0
        ldlen
        ldc.i4.3
        ceq
        ret
    }
    
    .method private static valuetype Hw2.CalculatorOperation ParseOperation(string arg) cil managed
    {
        .maxstack 3
        .locals init ([0] valuetype Hw2.CalculatorOperation)

        ldstr "+"
        ldarg.0
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s addOperation

        ldstr "-"
        ldarg.0
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s substractOperation

        ldstr "*"
        ldarg.0
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s multiplyOperation

        ldstr "/"
        ldarg.0
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s divideOperation

        ldstr "_"
        ldarg.0
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s undefinedOperation
        
        
        br.s badOperation


        addOperation: 
        ldc.i4.0
        br.s endOfMethod
        

        substractOperation: 
        ldc.i4.1
        br.s endOfMethod
        

        multiplyOperation: 
        ldc.i4.2
        br.s endOfMethod
        

        divideOperation: 
        ldc.i4.3
        br.s endOfMethod
        

        undefinedOperation:
        newobj instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
        throw
        
        badOperation:
        newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
        throw

        endOfMethod
        :
        
        stloc.0
        ldloc.0
        ret    
    }
}
namespace Avalonia.FuncUI.Lib

open System.Reflection
open System.Collections.Generic

[<RequireQualifiedAccess>]
module Func =
    open System

    (* get IL of method body *)
    let private getIL (func: 'a -> 'b) =
        let t = func.GetType()
        let mutable method = t.GetMethod("Invoke", BindingFlags.DeclaredOnly ||| BindingFlags.Instance ||| BindingFlags.Public)

        if method = null then 
            method <- t.GetMethod("Invoke", BindingFlags.Instance ||| BindingFlags.Public)

        let methodBody = method.GetMethodBody()
        methodBody.GetILAsByteArray()

    (* check if func is comparable *)
    let isComparable(func: 'a -> 'b) =
        let funcType = func.GetType()

        let hasFields = funcType.GetFields()
      
        if funcType.IsGenericType || not (Seq.isEmpty hasFields) then
            false
        else
            true

    (* compares two functions for equality *)
    let compare (funcA: 'a -> 'b) (funcB: 'c -> 'd) : bool=
        if not (isComparable funcA) then
            raise (Exception("function 'funcA' is generic or has outer dependencies"))

        if not (isComparable funcB) then
            raise (Exception("function 'funcB' is generic or has outer dependencies"))

        let bytesA = getIL funcA
        let bytesB = getIL funcB
        let spanA = ReadOnlySpan(bytesA)
        let spanB = ReadOnlySpan(bytesB)
        spanA.SequenceEqual(spanB)

    (* get hash of method body *)
    let hashMethodBody (func: 'a -> 'b) : int =
        let bytes = (getIL func) :> System.Collections.IStructuralEquatable
        bytes.GetHashCode(EqualityComparer<byte>.Default)

module Reflection =
    open System.Reflection

    let findPropertyByName (obj: 't) (propName: string): PropertyInfo =
        obj
            .GetType()
            .GetProperty(propName)
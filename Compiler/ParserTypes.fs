module ParserTypes

open QuadSupport
open Symbol

type expressionType =
    {
        Code    : quadType list;
        Place   : quadElementType;
    }

type conditionType = 
    {
        Code    : quadType list;
        True    : int ref list;
        False   : int ref list;
    }

let voidExpression = 
    { 
        Code    = []
        Place   = QNone
    }

let voidCondition  = 
    { 
        Code    = []
        True    = []
        False   = []
    }

type parameterList = (expressionType*pass_mode) list

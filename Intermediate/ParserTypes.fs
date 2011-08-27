module ParserTypes

open QuadSupport

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
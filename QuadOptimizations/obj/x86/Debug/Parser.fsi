// Signature file for parser generated by fsyacc
module Parser
type token = 
  | EQ
  | NE
  | LT
  | LE
  | GT
  | GE
  | PLUS
  | MINUS
  | ASTER
  | SLASH
  | MOD
  | NOT
  | AND
  | OR
  | LPAREN
  | RPAREN
  | LSBR
  | RSBR
  | LCBR
  | RCBR
  | COMMA
  | COLON
  | SEMICOLON
  | ASSIGN
  | T_BYTE
  | T_ELSE
  | T_FALSE
  | T_IF
  | T_INT
  | T_PROC
  | T_REF
  | T_RETURN
  | T_WHILE
  | T_TRUE
  | T_EOF
  | ID of (string)
  | STRING of (string list*string)
  | INT of (int16)
  | BYTE of (byte)
type tokenId = 
    | TOKEN_EQ
    | TOKEN_NE
    | TOKEN_LT
    | TOKEN_LE
    | TOKEN_GT
    | TOKEN_GE
    | TOKEN_PLUS
    | TOKEN_MINUS
    | TOKEN_ASTER
    | TOKEN_SLASH
    | TOKEN_MOD
    | TOKEN_NOT
    | TOKEN_AND
    | TOKEN_OR
    | TOKEN_LPAREN
    | TOKEN_RPAREN
    | TOKEN_LSBR
    | TOKEN_RSBR
    | TOKEN_LCBR
    | TOKEN_RCBR
    | TOKEN_COMMA
    | TOKEN_COLON
    | TOKEN_SEMICOLON
    | TOKEN_ASSIGN
    | TOKEN_T_BYTE
    | TOKEN_T_ELSE
    | TOKEN_T_FALSE
    | TOKEN_T_IF
    | TOKEN_T_INT
    | TOKEN_T_PROC
    | TOKEN_T_REF
    | TOKEN_T_RETURN
    | TOKEN_T_WHILE
    | TOKEN_T_TRUE
    | TOKEN_T_EOF
    | TOKEN_ID
    | TOKEN_STRING
    | TOKEN_INT
    | TOKEN_BYTE
    | TOKEN_end_of_input
    | TOKEN_error
type nonTerminalId = 
    | NONTERM__startstart
    | NONTERM_start
    | NONTERM_func_def
    | NONTERM_func_head
    | NONTERM_func_params
    | NONTERM_fpar_list
    | NONTERM_fpar_def
    | NONTERM_data_type
    | NONTERM_type
    | NONTERM_r_type
    | NONTERM_local_def
    | NONTERM_local_def_list
    | NONTERM_var_def
    | NONTERM_stmt
    | NONTERM_stmt_list
    | NONTERM_compound_stmt
    | NONTERM_func_call
    | NONTERM_expr_list
    | NONTERM_expr
    | NONTERM_ref_value
    | NONTERM_l_value
    | NONTERM_cond
    | NONTERM_compop
/// This function maps tokens to integer indexes
val tagOfToken: token -> int

/// This function maps integer indexes to symbolic token ids
val tokenTagToTokenId: int -> tokenId

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
val prodIdxToNonTerminal: int -> nonTerminalId

/// This function gets the name of a token as a string
val token_to_string: token -> string
val start : (Microsoft.FSharp.Text.Lexing.LexBuffer<'cty> -> token) -> Microsoft.FSharp.Text.Lexing.LexBuffer<'cty> -> ( quadType list list ) 

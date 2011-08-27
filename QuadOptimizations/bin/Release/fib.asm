xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _fibonacci_0
     mov ax, 4C00h
     int 21h
main endp

;@1:	"unit , tailRecursive, - , -"
_tailRecursive_1 proc near
	push bp
	mov bp, sp
	sub sp, 4
;@2:	">    , index, limit, (3, 4)"
	mov ax, word ptr [bp+8]
	mov si, word ptr [bp+4]
	mov cx, word ptr [si-2]
	cmp ax, cx
	jg  @label3
	jmp @label4
;@3:	"ret  , -, -, -"
@label3 :
	jmp @tailRecursive_1
;@4:	"par  , "Fibonacci number " , R , -"
@label4 :
	lea si, byte ptr @str_1
	push si
;@5:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@6:	"par  , index , V , -"
	mov ax, word ptr [bp+8]
	push ax
;@7:	"call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@8:	"par  , " is " , R , -"
	lea si, byte ptr @str_2
	push si
;@9:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@10:	"par  , cur , V , -"
	mov ax, word ptr [bp+10]
	push ax
;@11:	"call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@12:	"par  , ".\n" , R , -"
	lea si, byte ptr @str_3
	push si
;@13:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@14:	"par  , cur , V , -"
	mov ax, word ptr [bp+10]
	push ax
;@15:	"+    , prev, cur, $1"
	mov ax, word ptr [bp+12]
	mov cx, word ptr [bp+10]
	add ax, cx
	mov word ptr [bp-2], ax
;@16:	"par  , $1 , V , -"
	mov ax, word ptr [bp-2]
	push ax
;@17:	"+    , index, 1, $2"
	mov ax, word ptr [bp+8]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-4], ax
;@18:	"par  , $2 , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@19:	"call , - , - , tailRecursive"
	sub sp, 2
	push word ptr [bp+4]
	call near ptr _tailRecursive_1
	add sp, 10
;@20:	"endu , tailRecursive, - , -"
@tailRecursive_1 :
	mov sp, bp
	pop bp
	ret
_tailRecursive_1 endp

;@21:	"unit , fibonacci, - , -"
_fibonacci_0 proc near
	push bp
	mov bp, sp
	sub sp, 4
;@22:	"par  , "Limit: " , R , -"
	lea si, byte ptr @str_4
	push si
;@23:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@24:	"par  , $3 , RET , -"
	lea si, word ptr [bp-4]
	push si
;@25:	"call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@26:	":=   , $3, - , limit"
	mov ax, word ptr [bp-4]
	mov word ptr [bp-2], ax
;@27:	"par  , 0 , V , -"
	mov ax, 0
	push ax
;@28:	"par  , 1 , V , -"
	mov ax, 1
	push ax
;@29:	"par  , 1 , V , -"
	mov ax, 1
	push ax
;@30:	"call , - , - , tailRecursive"
	sub sp, 2
	push bp
	call near ptr _tailRecursive_1
	add sp, 10
;@31:	"endu , fibonacci, - , -"
@fibonacci_0 :
	mov sp, bp
	pop bp
	ret
_fibonacci_0 endp

extrn _writeString : proc
extrn _writeInteger : proc
extrn _readInteger : proc
; "Fibonacci number "
@str_1 db 'Fibonacci number ', 0

; " is "
@str_2 db ' is ', 0

; ".\n"
@str_3 db '.', 10, 0

; "Limit: "
@str_4 db 'Limit: ', 0

xseg ends
     end  main


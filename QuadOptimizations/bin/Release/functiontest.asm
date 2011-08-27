xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _functionTest_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , set, - , -"
_set_1 proc near
	push bp
	mov bp, sp
	sub sp, 0
;@2: ":=   , 42, - , x"
	mov ax, 42
	mov si, word ptr [bp+8]
	mov word ptr [si], ax
;@3: ":=   , 0, - , $$"
	mov ax, 0
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@4: "ret  , -, -, -"
	jmp @set_1
;@5: "endu , set, - , -"
@set_1 :
	mov sp, bp
	pop bp
	ret
_set_1 endp

;@6: "unit , writeTwoIntegers, - , -"
_writeTwoIntegers_2 proc near
	push bp
	mov bp, sp
	sub sp, 0
;@7: "par  , a , V , -"
	mov ax, word ptr [bp+10]
	push ax
;@8: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@9: "par  , " " , R , -"
	lea si, byte ptr @str_0
	push si
;@10: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@11: "par  , b , V , -"
	mov ax, word ptr [bp+8]
	push ax
;@12: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@13: "par  , "\n" , R , -"
	lea si, byte ptr @str_1
	push si
;@14: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@15: "endu , writeTwoIntegers, - , -"
@writeTwoIntegers_2 :
	mov sp, bp
	pop bp
	ret
_writeTwoIntegers_2 endp

;@16: "unit , functionTest, - , -"
_functionTest_0 proc near
	push bp
	mov bp, sp
	sub sp, 4
;@17: ":=   , 1, - , foo"
	mov ax, 1
	mov word ptr [bp-2], ax
;@18: "par  , foo , R , -"
	lea si, word ptr [bp-2]
	push si
;@19: "par  , $1 , RET , -"
	lea si, word ptr [bp-4]
	push si
;@20: "call , - , - , set"
	push bp
	call near ptr _set_1
	add sp, 6
;@21: "par  , $1 , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@22: "par  , foo , V , -"
	mov ax, word ptr [bp-2]
	push ax
;@23: "call , - , - , writeTwoIntegers"
	sub sp, 2
	push bp
	call near ptr _writeTwoIntegers_2
	add sp, 8
;@24: "endu , functionTest, - , -"
@functionTest_0 :
	mov sp, bp
	pop bp
	ret
_functionTest_0 endp

extrn _writeInteger : proc
extrn _writeString : proc
; " "
@str_0 db ' ', 0

; "\n"
@str_1 db 10, 0

xseg ends
     end  main


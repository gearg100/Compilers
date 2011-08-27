xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _weird_0
     mov ax, 4C00h
     int 21h
main endp

;@0: "unit , tail, - , -"
_tail_1 proc near
	push bp
	mov bp, sp
	sub sp, 10
;@1: "par  , "Entering tail with a =" , R , -"
	lea si, byte ptr @str_0
	push si
;@2: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@3: "par  , a , V , -"
	mov si, word ptr [bp+10]
	mov ax, word ptr [si]
	push ax
;@4: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@5: "par  , " and x = " , R , -"
	lea si, byte ptr @str_1
	push si
;@6: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@7: "par  , x , V , -"
	mov ax, word ptr [bp+8]
	push ax
;@8: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@9: "par  , ".\n" , R , -"
	lea si, byte ptr @str_2
	push si
;@10: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@11: ":=   , a, - , z"
	mov si, word ptr [bp+10]
	mov ax, word ptr [si]
	mov word ptr [bp-2], ax
;@12: ">    , z, x, 15"
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp+8]
	cmp ax, cx
	jg  @label15
;@13: "jump , - , - , 22"
	jmp @label22
;@14: "par  , z , R , -"
	lea si, word ptr [bp-2]
	push si
;@15: "-    , x, 1, $1"
@label15 :
	mov ax, word ptr [bp+8]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-4], ax
;@16: "par  , $1 , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@17: "par  , $2 , RET , -"
	lea si, word ptr [bp-6]
	push si
;@18: "call , - , - , tail"
	push word ptr [bp+4]
	call near ptr _tail_1
	add sp, 8
;@19: ":=   , $2, - , $$"
	mov ax, word ptr [bp-6]
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@20: "ret  , -, -, -"
	jmp @tail_1
;@21: "<    , z, x, 24"
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp+8]
	cmp ax, cx
	jl  @label24
;@22: "jump , - , - , 31"
@label22 :
	jmp @label31
;@23: "par  , z , R , -"
	lea si, word ptr [bp-2]
	push si
;@24: "+    , x, 1, $3"
@label24 :
	mov ax, word ptr [bp+8]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-8], ax
;@25: "par  , $3 , V , -"
	mov ax, word ptr [bp-8]
	push ax
;@26: "par  , $4 , RET , -"
	lea si, word ptr [bp-10]
	push si
;@27: "call , - , - , tail"
	push word ptr [bp+4]
	call near ptr _tail_1
	add sp, 8
;@28: ":=   , $4, - , $$"
	mov ax, word ptr [bp-10]
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@29: "ret  , -, -, -"
	jmp @tail_1
;@30: "par  , "Entering else part\n" , R , -"
	lea si, byte ptr @str_3
	push si
;@31: "call , - , - , writeString"
@label31 :
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@32: ":=   , 0, - , a"
	mov ax, 0
	mov si, word ptr [bp+10]
	mov word ptr [si], ax
;@33: ":=   , 0, - , $$"
	mov ax, 0
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@34: "ret  , -, -, -"
	jmp @tail_1
;@35: "endu , tail, - , -"
@tail_1 :
	mov sp, bp
	pop bp
	ret
_tail_1 endp

;@36: "unit , weird, - , -"
_weird_0 proc near
	push bp
	mov bp, sp
	sub sp, 4
;@37: ":=   , 5, - , y"
	mov ax, 5
	mov word ptr [bp-2], ax
;@38: "par  , "Result is: " , R , -"
	lea si, byte ptr @str_4
	push si
;@39: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@40: "par  , y , R , -"
	lea si, word ptr [bp-2]
	push si
;@41: "par  , 0 , V , -"
	mov ax, 0
	push ax
;@42: "par  , $5 , RET , -"
	lea si, word ptr [bp-4]
	push si
;@43: "call , - , - , tail"
	push bp
	call near ptr _tail_1
	add sp, 8
;@44: "par  , $5 , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@45: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@46: "par  , "\nAfter Call: " , R , -"
	lea si, byte ptr @str_5
	push si
;@47: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@48: "par  , y , V , -"
	mov ax, word ptr [bp-2]
	push ax
;@49: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@50: "par  , "\n" , R , -"
	lea si, byte ptr @str_6
	push si
;@51: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@52: "endu , weird, - , -"
@weird_0 :
	mov sp, bp
	pop bp
	ret
_weird_0 endp

extrn _writeString : proc
extrn _writeInteger : proc
; "Entering tail with a ="
@str_0 db 'Entering tail with a =', 0

; " and x = "
@str_1 db ' and x = ', 0

; ".\n"
@str_2 db '.', 10, 0

; "Entering else part\n"
@str_3 db 'Entering else part', 10, 0

; "Result is: "
@str_4 db 'Result is: ', 0

; "\nAfter Call: "
@str_5 db 10, 'After Call: ', 0

; "\n"
@str_6 db 10, 0

xseg ends
     end  main


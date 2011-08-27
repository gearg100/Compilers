xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _prog3_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , prog3, - , -"
_prog3_0 proc near
	push bp
	mov bp, sp
	sub sp, 8
;@2: "par  , "Give me a number:" , R , -"
	lea si, byte ptr @str_0
	push si
;@3: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@4: "par  , " " , R , -"
	lea si, byte ptr @str_1
	push si
;@5: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@6: "par  , $1 , RET , -"
	lea si, word ptr [bp-6]
	push si
;@7: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@8: ":=   , $1, - , n"
	mov ax, word ptr [bp-6]
	mov word ptr [bp-4], ax
;@9: ":=   , 1, - , i"
	mov ax, 1
	mov word ptr [bp-2], ax
;@10: "<=   , i, n, 12"
@label10 :
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp-4]
	cmp ax, cx
	jle  @label12
;@11: "jump , - , - , 19"
	jmp @label19
;@12: "par  , i , V , -"
@label12 :
	mov ax, word ptr [bp-2]
	push ax
;@13: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@14: "par  , 10 , V , -"
	mov al, 10
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@15: "call , - , - , writeChar"
	sub sp, 2
	push bp
	call near ptr _writeChar
	add sp, 5
;@16: "+    , i, 1, $2"
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-8], ax
;@17: ":=   , $2, - , i"
	mov ax, word ptr [bp-8]
	mov word ptr [bp-2], ax
;@18: "jump , - , - , 10"
	jmp @label10
;@19: "endu , prog3, - , -"
@label19 :
@prog3_0 :
	mov sp, bp
	pop bp
	ret
_prog3_0 endp

extrn _writeString : proc
extrn _readInteger : proc
extrn _writeInteger : proc
extrn _writeChar : proc
; "Give me a number:"
@str_0 db 'Give me a number:', 0

; " "
@str_1 db ' ', 0

xseg ends
     end  main


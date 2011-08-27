xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _prog4_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , prog4, - , -"
_prog4_0 proc near
	push bp
	mov bp, sp
	sub sp, 11
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
;@11: "jump , - , - , 25"
	jmp @label25
;@12: "%    , i, 2, $2"
@label12 :
	mov ax, word ptr [bp-2]
	cwd
	mov cx, 2
	idiv cx
	mov word ptr [bp-8], dx
;@13: "==   , $2, 1, 15"
	mov ax, word ptr [bp-8]
	mov cx, 1
	cmp ax, cx
	je  @label15
;@14: "jump , - , - , 22"
	jmp @label22
;@15: "par  , i , V , -"
@label15 :
	mov ax, word ptr [bp-2]
	push ax
;@16: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@17: "par  , 10 , V , -"
	mov ax, 10
	push ax
;@18: "par  , $3 , RET , -"
	lea si, byte ptr [bp-9]
	push si
;@19: "call , - , - , shrink"
	push bp
	call near ptr _shrink
	add sp, 5
;@20: "par  , $3 , V , -"
	mov al, byte ptr [bp-9]
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@21: "call , - , - , writeChar"
	sub sp, 2
	push bp
	call near ptr _writeChar
	add sp, 5
;@22: "+    , i, 1, $4"
@label22 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-11], ax
;@23: ":=   , $4, - , i"
	mov ax, word ptr [bp-11]
	mov word ptr [bp-2], ax
;@24: "jump , - , - , 10"
	jmp @label10
;@25: "endu , prog4, - , -"
@label25 :
@prog4_0 :
	mov sp, bp
	pop bp
	ret
_prog4_0 endp

extrn _writeString : proc
extrn _readInteger : proc
extrn _writeInteger : proc
extrn _shrink : proc
extrn _writeChar : proc
; "Give me a number:"
@str_0 db 'Give me a number:', 0

; " "
@str_1 db ' ', 0

xseg ends
     end  main


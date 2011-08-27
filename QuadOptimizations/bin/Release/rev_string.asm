xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _main_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , reverse, - , -"
_reverse_1 proc near
	push bp
	mov bp, sp
	sub sp, 18
;@2: "par  , s , R , -"
	mov si, word ptr [bp+8]
	push si
;@3: "par  , $1 , RET , -"
	lea si, word ptr [bp-6]
	push si
;@4: "call , - , - , strlen"
	push bp
	call near ptr _strlen
	add sp, 6
;@5: ":=   , $1, - , l"
	mov ax, word ptr [bp-6]
	mov word ptr [bp-4], ax
;@6: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-2], ax
;@7: "<    , i, l, 9"
@label7 :
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp-4]
	cmp ax, cx
	jl  @label9
;@8: "jump , - , - , 17"
	jmp @label17
;@9: "-    , l, i, $3"
@label9 :
	mov ax, word ptr [bp-4]
	mov cx, word ptr [bp-2]
	sub ax, cx
	mov word ptr [bp-10], ax
;@10: "-    , $3, 1, $4"
	mov ax, word ptr [bp-10]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-12], ax
;@11: "array, s, $4, $5"
	mov ax, word ptr [bp-12]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-14], ax
;@12: "array, r, i, $2"
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	mov si, word ptr [bp+4]
	lea cx, word ptr [si-32]
	add ax, cx
	mov word ptr [bp-8], ax
;@13: ":=   , [$5], - , [$2]"
	mov di, word ptr [bp-14]
	mov al, byte ptr [di]
	mov di, word ptr [bp-8]
	mov byte ptr [di], al
;@14: "+    , i, 1, $6"
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-16], ax
;@15: ":=   , $6, - , i"
	mov ax, word ptr [bp-16]
	mov word ptr [bp-2], ax
;@16: "jump , - , - , 7"
	jmp @label7
;@17: "array, r, i, $7"
@label17 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	mov si, word ptr [bp+4]
	lea cx, word ptr [si-32]
	add ax, cx
	mov word ptr [bp-18], ax
;@18: ":=   , 0, - , [$7]"
	mov al, 0
	mov di, word ptr [bp-18]
	mov byte ptr [di], al
;@19: "endu , reverse, - , -"
@reverse_1 :
	mov sp, bp
	pop bp
	ret
_reverse_1 endp

;@20: "unit , main, - , -"
_main_0 proc near
	push bp
	mov bp, sp
	sub sp, 32
;@21: "par  , "\n!dlrow olleH" , R , -"
	lea si, byte ptr @str_0
	push si
;@22: "call , - , - , reverse"
	sub sp, 2
	push bp
	call near ptr _reverse_1
	add sp, 6
;@23: "par  , r , R , -"
	lea si, word ptr [bp-32]
	push si
;@24: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@25: "endu , main, - , -"
@main_0 :
	mov sp, bp
	pop bp
	ret
_main_0 endp

extrn _strlen : proc
extrn _writeString : proc
; "\n!dlrow olleH"
@str_0 db 10, '!dlrow olleH', 0

xseg ends
     end  main


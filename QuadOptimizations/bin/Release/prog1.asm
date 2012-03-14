xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _prog1_0
     mov ax, 4C00h
     int 21h
main endp

;@1:	"unit , swap, - , -"
_swap_1 proc near
	push bp
	mov bp, sp
	sub sp, 2
;@2:	":=   , a, - , t"
	mov ax, word ptr [bp+10]
	mov word ptr [bp-2], ax
;@3:	":=   , b, - , a"
	mov ax, word ptr [bp+8]
	mov word ptr [bp+10], ax
;@4:	":=   , t, - , b"
	mov ax, word ptr [bp-2]
	mov word ptr [bp+8], ax
;@5:	"endu , swap, - , -"
@swap_1 :
	mov sp, bp
	pop bp
	ret
_swap_1 endp

;@6:	"unit , prog1, - , -"
_prog1_0 proc near
	push bp
	mov bp, sp
	sub sp, 34
;@7:	"+    , 1, 1, $1"
	mov ax, 1
	mov cx, 1
	add ax, cx
	mov word ptr [bp-14], ax
;@8:	"+    , $1, 1, $2"
	mov ax, word ptr [bp-14]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-16], ax
;@9:	"+    , $2, 1, $3"
	mov ax, word ptr [bp-16]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-18], ax
;@10:	"+    , $3, 1, $4"
	mov ax, word ptr [bp-18]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-20], ax
;@11:	":=   , $4, - , b"
	mov ax, word ptr [bp-20]
	mov word ptr [bp-4], ax
;@12:	"+    , 15, 2, $5"
	mov ax, 15
	mov cx, 2
	add ax, cx
	mov word ptr [bp-22], ax
;@13:	"+    , $5, 2, $6"
	mov ax, word ptr [bp-22]
	mov cx, 2
	add ax, cx
	mov word ptr [bp-24], ax
;@14:	"+    , $6, b, $7"
	mov ax, word ptr [bp-24]
	mov cx, word ptr [bp-4]
	add ax, cx
	mov word ptr [bp-26], ax
;@15:	"+    , $7, 2, $8"
	mov ax, word ptr [bp-26]
	mov cx, 2
	add ax, cx
	mov word ptr [bp-28], ax
;@16:	"+    , $8, 2, $9"
	mov ax, word ptr [bp-28]
	mov cx, 2
	add ax, cx
	mov word ptr [bp-30], ax
;@17:	"+    , $9, 1, $10"
	mov ax, word ptr [bp-30]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-32], ax
;@18:	":=   , $10, - , a"
	mov ax, word ptr [bp-32]
	mov word ptr [bp-2], ax
;@19:	"par  , a , V , -"
	mov ax, word ptr [bp-2]
	push ax
;@20:	"par  , b , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@21:	"call , - , - , swap"
	sub sp, 2
	push bp
	call near ptr _swap_1
	add sp, 8
;@22:	"<    , a, b, (23, 25)"
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp-4]
	cmp ax, cx
	jl  @label23
	jmp @label25
;@23:	":=   , 1, - , c"
@label23 :
	mov ax, 1
	mov word ptr [bp-6], ax
;@24:	"jump , - , - , 27"
	jmp @label27
;@25:	"+    , a, b, $11"
@label25 :
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp-4]
	add ax, cx
	mov word ptr [bp-34], ax
;@26:	":=   , $11, - , c"
	mov ax, word ptr [bp-34]
	mov word ptr [bp-6], ax
;@27:	"par  , c , V , -"
@label27 :
	mov ax, word ptr [bp-6]
	push ax
;@28:	"call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@29:	"endu , prog1, - , -"
@prog1_0 :
	mov sp, bp
	pop bp
	ret
_prog1_0 endp

extrn _writeInteger : proc
xseg ends
     end  main


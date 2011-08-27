xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _prog6_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , prog6, - , -"
_prog6_0 proc near
	push bp
	mov bp, sp
	sub sp, 34
;@2: "array, r, 0, $1"
	mov ax, 0
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-6]
	add ax, cx
	mov word ptr [bp-16], ax
;@3: ":=   , 100, - , [$1]"
	mov al, 100
	mov di, word ptr [bp-16]
	mov byte ptr [di], al
;@4: "array, r, 1, $2"
	mov ax, 1
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-6]
	add ax, cx
	mov word ptr [bp-18], ax
;@5: ":=   , 115, - , [$2]"
	mov al, 115
	mov di, word ptr [bp-18]
	mov byte ptr [di], al
;@6: "array, r, 2, $3"
	mov ax, 2
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-6]
	add ax, cx
	mov word ptr [bp-20], ax
;@7: ":=   , 97, - , [$3]"
	mov al, 97
	mov di, word ptr [bp-20]
	mov byte ptr [di], al
;@8: "array, r, 3, $4"
	mov ax, 3
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-6]
	add ax, cx
	mov word ptr [bp-22], ax
;@9: ":=   , 0, - , [$4]"
	mov al, 0
	mov di, word ptr [bp-22]
	mov byte ptr [di], al
;@10: "par  , r , R , -"
	lea si, word ptr [bp-6]
	push si
;@11: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@12: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-14], ax
;@13: "<    , i, 3, 15"
@label13 :
	mov ax, word ptr [bp-14]
	mov cx, 3
	cmp ax, cx
	jl  @label15
;@14: "jump , - , - , 23"
	jmp @label23
;@15: "-    , 3, i, $6"
@label15 :
	mov ax, 3
	mov cx, word ptr [bp-14]
	sub ax, cx
	mov word ptr [bp-26], ax
;@16: "-    , $6, 1, $7"
	mov ax, word ptr [bp-26]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-28], ax
;@17: "array, r, $7, $8"
	mov ax, word ptr [bp-28]
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-6]
	add ax, cx
	mov word ptr [bp-30], ax
;@18: "array, s, i, $5"
	mov ax, word ptr [bp-14]
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-12]
	add ax, cx
	mov word ptr [bp-24], ax
;@19: ":=   , [$8], - , [$5]"
	mov di, word ptr [bp-30]
	mov al, byte ptr [di]
	mov di, word ptr [bp-24]
	mov byte ptr [di], al
;@20: "+    , i, 1, $9"
	mov ax, word ptr [bp-14]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-32], ax
;@21: ":=   , $9, - , i"
	mov ax, word ptr [bp-32]
	mov word ptr [bp-14], ax
;@22: "jump , - , - , 13"
	jmp @label13
;@23: "array, s, i, $10"
@label23 :
	mov ax, word ptr [bp-14]
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-12]
	add ax, cx
	mov word ptr [bp-34], ax
;@24: ":=   , 0, - , [$10]"
	mov al, 0
	mov di, word ptr [bp-34]
	mov byte ptr [di], al
;@25: "par  , s , R , -"
	lea si, word ptr [bp-12]
	push si
;@26: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@27: "endu , prog6, - , -"
@prog6_0 :
	mov sp, bp
	pop bp
	ret
_prog6_0 endp

extrn _writeString : proc
xseg ends
     end  main


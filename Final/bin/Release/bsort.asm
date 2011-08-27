xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _main_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , swap, - , -"
_swap_2 proc near
	push bp
	mov bp, sp
	sub sp, 2
;@2: ":=   , x, - , t"
	mov si, word ptr [si+10]
	mov ax, word ptr [si]
	mov word ptr [bp-2], ax
;@3: ":=   , y, - , x"
	mov si, word ptr [si+8]
	mov ax, word ptr [si]
	mov si, word ptr [bp+10]
	mov word ptr [si], ax
;@4: ":=   , t, - , y"
	mov ax, word ptr [bp-2]
	mov si, word ptr [bp+8]
	mov word ptr [si], ax
;@5: "endu , swap, - , -"
@swap_2 :
	mov sp, bp
	pop bp
	ret
_swap_2 endp

;@6: "unit , bsort, - , -"
_bsort_1 proc near
	push bp
	mov bp, sp
	sub sp, 19
;@7: ":=   , 121, - , changed"
	mov al, 121
	mov byte ptr [bp-1], al
;@8: "==   , changed, 121, 10"
@label8 :
	mov al, byte ptr [bp-1]
	mov cl, 121
	cmp al, cl
	je  @label10
;@9: "jump , - , - , 31"
	jmp @label31
;@10: ":=   , 110, - , changed"
@label10 :
	mov al, 110
	mov byte ptr [bp-1], al
;@11: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-3], ax
;@12: "-    , n, 1, $1"
@label12 :
	mov ax, word ptr [bp+10]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-5], ax
;@13: "<    , i, $1, 15"
	mov ax, word ptr [bp-3]
	mov cx, word ptr [bp-5]
	cmp ax, cx
	jl  @label15
;@14: "jump , - , - , 30"
	jmp @label30
;@15: "array, x, i, $2"
@label15 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-7], ax
;@16: "+    , i, 1, $3"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-9], ax
;@17: "array, x, $3, $4"
	mov ax, word ptr [bp-9]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-11], ax
;@18: ">    , [$2], [$4], 20"
	mov di, word ptr [bp-7]
	mov ax, word ptr [di]
	mov di, word ptr [bp-11]
	mov cx, word ptr [di]
	cmp ax, cx
	jg  @label20
;@19: "jump , - , - , 27"
	jmp @label27
;@20: "array, x, i, $5"
@label20 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-13], ax
;@21: "par  , [$5] , R , -"
	mov si, word ptr [bp-13]
	push si
;@22: "+    , i, 1, $6"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-15], ax
;@23: "array, x, $6, $7"
	mov ax, word ptr [bp-15]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-17], ax
;@24: "par  , [$7] , R , -"
	mov si, word ptr [bp-17]
	push si
;@25: "call , - , - , swap"
	sub sp, 2
	push bp
	call near ptr _swap_2
	add sp, 8
;@26: ":=   , 121, - , changed"
	mov al, 121
	mov byte ptr [bp-1], al
;@27: "+    , i, 1, $8"
@label27 :
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-19], ax
;@28: ":=   , $8, - , i"
	mov ax, word ptr [bp-19]
	mov word ptr [bp-3], ax
;@29: "jump , - , - , 12"
	jmp @label12
;@30: "jump , - , - , 8"
@label30 :
	jmp @label8
;@31: "endu , bsort, - , -"
@label31 :
@bsort_1 :
	mov sp, bp
	pop bp
	ret
_bsort_1 endp

;@32: "unit , writeArray, - , -"
_writeArray_3 proc near
	push bp
	mov bp, sp
	sub sp, 6
;@33: "par  , msg , R , -"
	mov si, word ptr [bp+12]
	push si
;@34: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@35: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-2], ax
;@36: "<    , i, n, 38"
@label36 :
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp+10]
	cmp ax, cx
	jl  @label38
;@37: "jump , - , - , 48"
	jmp @label48
;@38: ">    , i, 0, 40"
@label38 :
	mov ax, word ptr [bp-2]
	mov cx, 0
	cmp ax, cx
	jg  @label40
;@39: "jump , - , - , 42"
	jmp @label42
;@40: "par  , ", " , R , -"
@label40 :
	lea si, byte ptr @str_0
	push si
;@41: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@42: "array, x, i, $9"
@label42 :
	mov ax, word ptr [bp-2]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-4], ax
;@43: "par  , [$9] , V , -"
	mov di, word ptr [bp-4]
	mov ax, word ptr [di]
	push ax
;@44: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@45: "+    , i, 1, $10"
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-6], ax
;@46: ":=   , $10, - , i"
	mov ax, word ptr [bp-6]
	mov word ptr [bp-2], ax
;@47: "jump , - , - , 36"
	jmp @label36
;@48: "par  , "\n" , R , -"
@label48 :
	lea si, byte ptr @str_1
	push si
;@49: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@50: "endu , writeArray, - , -"
@writeArray_3 :
	mov sp, bp
	pop bp
	ret
_writeArray_3 endp

;@51: "unit , main, - , -"
_main_0 proc near
	push bp
	mov bp, sp
	sub sp, 48
;@52: ":=   , 65, - , seed"
	mov ax, 65
	mov word ptr [bp-2], ax
;@53: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-36], ax
;@54: "<    , i, 16, 56"
@label54 :
	mov ax, word ptr [bp-36]
	mov cx, 16
	cmp ax, cx
	jl  @label56
;@55: "jump , - , - , 66"
	jmp @label66
;@56: "*    , seed, 137, $11"
@label56 :
	mov ax, word ptr [bp-2]
	mov cx, 137
	imul cx
	mov word ptr [bp-38], ax
;@57: "+    , $11, 221, $12"
	mov ax, word ptr [bp-38]
	mov cx, 221
	add ax, cx
	mov word ptr [bp-40], ax
;@58: "+    , $12, i, $13"
	mov ax, word ptr [bp-40]
	mov cx, word ptr [bp-36]
	add ax, cx
	mov word ptr [bp-42], ax
;@59: "%    , $13, 101, $14"
	mov ax, word ptr [bp-42]
	cwd
	mov cx, 101
	idiv cx
	mov word ptr [bp-44], dx
;@60: ":=   , $14, - , seed"
	mov ax, word ptr [bp-44]
	mov word ptr [bp-2], ax
;@61: "array, x, i, $15"
	mov ax, word ptr [bp-36]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-34]
	add ax, cx
	mov word ptr [bp-46], ax
;@62: ":=   , seed, - , [$15]"
	mov ax, word ptr [bp-2]
	mov di, word ptr [bp-46]
	mov word ptr [di], ax
;@63: "+    , i, 1, $16"
	mov ax, word ptr [bp-36]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-48], ax
;@64: ":=   , $16, - , i"
	mov ax, word ptr [bp-48]
	mov word ptr [bp-36], ax
;@65: "jump , - , - , 54"
	jmp @label54
;@66: "par  , "Initial array: " , R , -"
@label66 :
	lea si, byte ptr @str_2
	push si
;@67: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@68: "par  , x , R , -"
	lea si, word ptr [bp-34]
	push si
;@69: "call , - , - , writeArray"
	sub sp, 2
	push bp
	call near ptr _writeArray_3
	add sp, 10
;@70: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@71: "par  , x , R , -"
	lea si, word ptr [bp-34]
	push si
;@72: "call , - , - , bsort"
	sub sp, 2
	push bp
	call near ptr _bsort_1
	add sp, 8
;@73: "par  , "Sorted array: " , R , -"
	lea si, byte ptr @str_3
	push si
;@74: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@75: "par  , x , R , -"
	lea si, word ptr [bp-34]
	push si
;@76: "call , - , - , writeArray"
	sub sp, 2
	push bp
	call near ptr _writeArray_3
	add sp, 10
;@77: "endu , main, - , -"
@main_0 :
	mov sp, bp
	pop bp
	ret
_main_0 endp

extrn _writeString : proc
extrn _writeInteger : proc
; ", "
@str_0 db ', ', 0

; "\n"
@str_1 db 10, 0

; "Initial array: "
@str_2 db 'Initial array: ', 0

; "Sorted array: "
@str_3 db 'Sorted array: ', 0

xseg ends
     end  main


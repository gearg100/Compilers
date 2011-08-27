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
	mov si, word ptr [bp+10]
	mov ax, word ptr [si]
	mov word ptr [bp-2], ax
;@3: ":=   , y, - , x"
	mov si, word ptr [bp+8]
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
	sub sp, 31
;@7: ":=   , 121, - , changed"
	mov al, 121
	mov byte ptr [bp-1], al
;@8: "==   , changed, 121, 10"
@label8 :
	mov al, byte ptr [bp-1]
	mov cl, 121
	cmp al, cl
	je  @label10
;@9: "jump , - , - , 46"
	jmp @label46
;@10: ":=   , 110, - , changed"
@label10 :
	mov al, 110
	mov byte ptr [bp-1], al
;@11: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-3], ax
;@12: "-    , n, 1, $1"
@label12 :
	mov ax, word ptr [bp+12]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-5], ax
;@13: "<    , i, $1, 15"
	mov ax, word ptr [bp-3]
	mov cx, word ptr [bp-5]
	cmp ax, cx
	jl  @label15
;@14: "jump , - , - , 45"
	jmp @label45
;@15: "==   , choice, 1, 17"
@label15 :
	mov ax, word ptr [bp+10]
	mov cx, 1
	cmp ax, cx
	je  @label17
;@16: "jump , - , - , 30"
	jmp @label30
;@17: "array, x, i, $2"
@label17 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-7], ax
;@18: "+    , i, 1, $3"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-9], ax
;@19: "array, x, $3, $4"
	mov ax, word ptr [bp-9]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-11], ax
;@20: ">    , [$2], [$4], 22"
	mov di, word ptr [bp-7]
	mov ax, word ptr [di]
	mov di, word ptr [bp-11]
	mov cx, word ptr [di]
	cmp ax, cx
	jg  @label22
;@21: "jump , - , - , 29"
	jmp @label29
;@22: "array, x, i, $5"
@label22 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-13], ax
;@23: "par  , [$5] , R , -"
	mov si, word ptr [bp-13]
	push si
;@24: "+    , i, 1, $6"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-15], ax
;@25: "array, x, $6, $7"
	mov ax, word ptr [bp-15]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-17], ax
;@26: "par  , [$7] , R , -"
	mov si, word ptr [bp-17]
	push si
;@27: "call , - , - , swap"
	sub sp, 2
	push bp
	call near ptr _swap_2
	add sp, 8
;@28: ":=   , 121, - , changed"
	mov al, 121
	mov byte ptr [bp-1], al
;@29: "jump , - , - , 42"
@label29 :
	jmp @label42
;@30: "array, x, i, $8"
@label30 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-19], ax
;@31: "+    , i, 1, $9"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-21], ax
;@32: "array, x, $9, $10"
	mov ax, word ptr [bp-21]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-23], ax
;@33: "<    , [$8], [$10], 35"
	mov di, word ptr [bp-19]
	mov ax, word ptr [di]
	mov di, word ptr [bp-23]
	mov cx, word ptr [di]
	cmp ax, cx
	jl  @label35
;@34: "jump , - , - , 42"
	jmp @label42
;@35: "array, x, i, $11"
@label35 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-25], ax
;@36: "par  , [$11] , R , -"
	mov si, word ptr [bp-25]
	push si
;@37: "+    , i, 1, $12"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-27], ax
;@38: "array, x, $12, $13"
	mov ax, word ptr [bp-27]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-29], ax
;@39: "par  , [$13] , R , -"
	mov si, word ptr [bp-29]
	push si
;@40: "call , - , - , swap"
	sub sp, 2
	push bp
	call near ptr _swap_2
	add sp, 8
;@41: ":=   , 121, - , changed"
	mov al, 121
	mov byte ptr [bp-1], al
;@42: "+    , i, 1, $14"
@label42 :
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-31], ax
;@43: ":=   , $14, - , i"
	mov ax, word ptr [bp-31]
	mov word ptr [bp-3], ax
;@44: "jump , - , - , 12"
	jmp @label12
;@45: "jump , - , - , 8"
@label45 :
	jmp @label8
;@46: "endu , bsort, - , -"
@label46 :
@bsort_1 :
	mov sp, bp
	pop bp
	ret
_bsort_1 endp

;@47: "unit , writeArray, - , -"
_writeArray_3 proc near
	push bp
	mov bp, sp
	sub sp, 6
;@48: "par  , msg , R , -"
	mov si, word ptr [bp+12]
	push si
;@49: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@50: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-2], ax
;@51: "<    , i, n, 53"
@label51 :
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp+10]
	cmp ax, cx
	jl  @label53
;@52: "jump , - , - , 63"
	jmp @label63
;@53: ">    , i, 0, 55"
@label53 :
	mov ax, word ptr [bp-2]
	mov cx, 0
	cmp ax, cx
	jg  @label55
;@54: "jump , - , - , 57"
	jmp @label57
;@55: "par  , ", " , R , -"
@label55 :
	lea si, byte ptr @str_0
	push si
;@56: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@57: "array, x, i, $15"
@label57 :
	mov ax, word ptr [bp-2]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-4], ax
;@58: "par  , [$15] , V , -"
	mov di, word ptr [bp-4]
	mov ax, word ptr [di]
	push ax
;@59: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@60: "+    , i, 1, $16"
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-6], ax
;@61: ":=   , $16, - , i"
	mov ax, word ptr [bp-6]
	mov word ptr [bp-2], ax
;@62: "jump , - , - , 51"
	jmp @label51
;@63: "par  , "\n" , R , -"
@label63 :
	lea si, byte ptr @str_1
	push si
;@64: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@65: "endu , writeArray, - , -"
@writeArray_3 :
	mov sp, bp
	pop bp
	ret
_writeArray_3 endp

;@66: "unit , main, - , -"
_main_0 proc near
	push bp
	mov bp, sp
	sub sp, 56
;@67: "par  , "Seed :\t" , R , -"
	lea si, byte ptr @str_2
	push si
;@68: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@69: "par  , $17 , RET , -"
	lea si, word ptr [bp-40]
	push si
;@70: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@71: ":=   , $17, - , seed"
	mov ax, word ptr [bp-40]
	mov word ptr [bp-2], ax
;@72: "par  , "How do you want to sort?\n" , R , -"
	lea si, byte ptr @str_3
	push si
;@73: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@74: "par  , "How do you want to sort?\x0a" , R , -"
	lea si, byte ptr @str_4
	push si
;@75: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@76: "par  , "  1. Min to Max\n" , R , -"
	lea si, byte ptr @str_5
	push si
;@77: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@78: "par  , "  2. Max to Min\n" , R , -"
	lea si, byte ptr @str_6
	push si
;@79: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@80: "par  , "Choice : " , R , -"
	lea si, byte ptr @str_7
	push si
;@81: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@82: "par  , $18 , RET , -"
	lea si, word ptr [bp-42]
	push si
;@83: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@84: ":=   , $18, - , choice"
	mov ax, word ptr [bp-42]
	mov word ptr [bp-4], ax
;@85: "<    , choice, 1, 89"
@label85 :
	mov ax, word ptr [bp-4]
	mov cx, 1
	cmp ax, cx
	jl  @label89
;@86: "jump , - , - , 87"
	jmp @label87
;@87: ">    , choice, 2, 89"
@label87 :
	mov ax, word ptr [bp-4]
	mov cx, 2
	cmp ax, cx
	jg  @label89
;@88: "jump , - , - , 101"
	jmp @label101
;@89: "par  , "How do you want to sort?\x0a" , R , -"
@label89 :
	lea si, byte ptr @str_4
	push si
;@90: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@91: "par  , "  1. Min to Max\n" , R , -"
	lea si, byte ptr @str_5
	push si
;@92: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@93: "par  , "  2. Max to Min\n" , R , -"
	lea si, byte ptr @str_6
	push si
;@94: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@95: "par  , "Choice : " , R , -"
	lea si, byte ptr @str_7
	push si
;@96: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@97: "par  , $19 , RET , -"
	lea si, word ptr [bp-44]
	push si
;@98: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@99: ":=   , $19, - , choice"
	mov ax, word ptr [bp-44]
	mov word ptr [bp-4], ax
;@100: "jump , - , - , 85"
	jmp @label85
;@101: ":=   , 0, - , i"
@label101 :
	mov ax, 0
	mov word ptr [bp-38], ax
;@102: "<    , i, 16, 104"
@label102 :
	mov ax, word ptr [bp-38]
	mov cx, 16
	cmp ax, cx
	jl  @label104
;@103: "jump , - , - , 114"
	jmp @label114
;@104: "*    , seed, 137, $20"
@label104 :
	mov ax, word ptr [bp-2]
	mov cx, 137
	imul cx
	mov word ptr [bp-46], ax
;@105: "+    , $20, 220, $21"
	mov ax, word ptr [bp-46]
	mov cx, 220
	add ax, cx
	mov word ptr [bp-48], ax
;@106: "+    , $21, i, $22"
	mov ax, word ptr [bp-48]
	mov cx, word ptr [bp-38]
	add ax, cx
	mov word ptr [bp-50], ax
;@107: "%    , $22, 101, $23"
	mov ax, word ptr [bp-50]
	cwd
	mov cx, 101
	idiv cx
	mov word ptr [bp-52], dx
;@108: ":=   , $23, - , seed"
	mov ax, word ptr [bp-52]
	mov word ptr [bp-2], ax
;@109: "array, x, i, $24"
	mov ax, word ptr [bp-38]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-36]
	add ax, cx
	mov word ptr [bp-54], ax
;@110: ":=   , seed, - , [$24]"
	mov ax, word ptr [bp-2]
	mov di, word ptr [bp-54]
	mov word ptr [di], ax
;@111: "+    , i, 1, $25"
	mov ax, word ptr [bp-38]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-56], ax
;@112: ":=   , $25, - , i"
	mov ax, word ptr [bp-56]
	mov word ptr [bp-38], ax
;@113: "jump , - , - , 102"
	jmp @label102
;@114: "par  , "Initial array: " , R , -"
@label114 :
	lea si, byte ptr @str_8
	push si
;@115: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@116: "par  , x , R , -"
	lea si, word ptr [bp-36]
	push si
;@117: "call , - , - , writeArray"
	sub sp, 2
	push bp
	call near ptr _writeArray_3
	add sp, 10
;@118: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@119: "par  , choice , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@120: "par  , x , R , -"
	lea si, word ptr [bp-36]
	push si
;@121: "call , - , - , bsort"
	sub sp, 2
	push bp
	call near ptr _bsort_1
	add sp, 10
;@122: "par  , "Sorted array: " , R , -"
	lea si, byte ptr @str_9
	push si
;@123: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@124: "par  , x , R , -"
	lea si, word ptr [bp-36]
	push si
;@125: "call , - , - , writeArray"
	sub sp, 2
	push bp
	call near ptr _writeArray_3
	add sp, 10
;@126: "endu , main, - , -"
@main_0 :
	mov sp, bp
	pop bp
	ret
_main_0 endp

extrn _writeString : proc
extrn _writeInteger : proc
extrn _readInteger : proc
; ", "
@str_0 db ', ', 0

; "\n"
@str_1 db 10, 0

; "Seed :\t"
@str_2 db 'Seed :', 09, 0

; "How do you want to sort?\n"
@str_3 db 'How do you want to sort?', 10, 0

; "How do you want to sort?\x0a"
@str_4 db 'How do you want to sort?', 10, 0

; "  1. Min to Max\n"
@str_5 db '  1. Min to Max', 10, 0

; "  2. Max to Min\n"
@str_6 db '  2. Max to Min', 10, 0

; "Choice : "
@str_7 db 'Choice : ', 0

; "Initial array: "
@str_8 db 'Initial array: ', 0

; "Sorted array: "
@str_9 db 'Sorted array: ', 0

xseg ends
     end  main


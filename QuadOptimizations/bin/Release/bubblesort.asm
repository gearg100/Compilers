xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _main_0
     mov ax, 4C00h
     int 21h
main endp

;@1:	"unit , swap, - , -"
_swap_2 proc near
	push bp
	mov bp, sp
	sub sp, 2
;@2:	":=   , x, - , t"
	mov si, word ptr [bp+10]
	mov ax, word ptr [si]
	mov word ptr [bp-2], ax
;@3:	":=   , y, - , x"
	mov si, word ptr [bp+8]
	mov ax, word ptr [si]
	mov si, word ptr [bp+10]
	mov word ptr [si], ax
;@4:	":=   , t, - , y"
	mov ax, word ptr [bp-2]
	mov si, word ptr [bp+8]
	mov word ptr [si], ax
;@5:	"endu , swap, - , -"
@swap_2 :
	mov sp, bp
	pop bp
	ret
_swap_2 endp

;@6:	"unit , bsort, - , -"
_bsort_1 proc near
	push bp
	mov bp, sp
	sub sp, 31
;@7:	":=   , 121, - , changed"
	mov al, 121
	mov byte ptr [bp-1], al
;@8:	"==   , changed, 121, (9, 41)"
@label8 :
	mov al, byte ptr [bp-1]
	mov cl, 121
	cmp al, cl
	je  @label9
	jmp @label41
;@9:	":=   , 110, - , changed"
@label9 :
	mov al, 110
	mov byte ptr [bp-1], al
;@10:	":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-3], ax
;@11:	"-    , n, 1, $1"
@label11 :
	mov ax, word ptr [bp+12]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-5], ax
;@12:	"<    , i, $1, (13, 40)"
	mov ax, word ptr [bp-3]
	mov cx, word ptr [bp-5]
	cmp ax, cx
	jl  @label13
	jmp @label40
;@13:	"==   , choice, 1, (14, 26)"
@label13 :
	mov ax, word ptr [bp+10]
	mov cx, 1
	cmp ax, cx
	je  @label14
	jmp @label26
;@14:	"array, x, i, $2"
@label14 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-7], ax
;@15:	"+    , i, 1, $3"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-9], ax
;@16:	"array, x, $3, $4"
	mov ax, word ptr [bp-9]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-11], ax
;@17:	">    , [$2], [$4], (18, 25)"
	mov di, word ptr [bp-7]
	mov ax, word ptr [di]
	mov di, word ptr [bp-11]
	mov cx, word ptr [di]
	cmp ax, cx
	jg  @label18
	jmp @label25
;@18:	"array, x, i, $5"
@label18 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-13], ax
;@19:	"par  , [$5] , R , -"
	mov si, word ptr [bp-13]
	push si
;@20:	"+    , i, 1, $6"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-15], ax
;@21:	"array, x, $6, $7"
	mov ax, word ptr [bp-15]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-17], ax
;@22:	"par  , [$7] , R , -"
	mov si, word ptr [bp-17]
	push si
;@23:	"call , - , - , swap"
	sub sp, 2
	push bp
	call near ptr _swap_2
	add sp, 8
;@24:	":=   , 121, - , changed"
	mov al, 121
	mov byte ptr [bp-1], al
;@25:	"jump , - , - , 37"
@label25 :
	jmp @label37
;@26:	"array, x, i, $8"
@label26 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-19], ax
;@27:	"+    , i, 1, $9"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-21], ax
;@28:	"array, x, $9, $10"
	mov ax, word ptr [bp-21]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-23], ax
;@29:	"<    , [$8], [$10], (30, 37)"
	mov di, word ptr [bp-19]
	mov ax, word ptr [di]
	mov di, word ptr [bp-23]
	mov cx, word ptr [di]
	cmp ax, cx
	jl  @label30
	jmp @label37
;@30:	"array, x, i, $11"
@label30 :
	mov ax, word ptr [bp-3]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-25], ax
;@31:	"par  , [$11] , R , -"
	mov si, word ptr [bp-25]
	push si
;@32:	"+    , i, 1, $12"
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-27], ax
;@33:	"array, x, $12, $13"
	mov ax, word ptr [bp-27]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-29], ax
;@34:	"par  , [$13] , R , -"
	mov si, word ptr [bp-29]
	push si
;@35:	"call , - , - , swap"
	sub sp, 2
	push bp
	call near ptr _swap_2
	add sp, 8
;@36:	":=   , 121, - , changed"
	mov al, 121
	mov byte ptr [bp-1], al
;@37:	"+    , i, 1, $14"
@label37 :
	mov ax, word ptr [bp-3]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-31], ax
;@38:	":=   , $14, - , i"
	mov ax, word ptr [bp-31]
	mov word ptr [bp-3], ax
;@39:	"jump , - , - , 11"
	jmp @label11
;@40:	"jump , - , - , 8"
@label40 :
	jmp @label8
;@41:	"endu , bsort, - , -"
@label41 :
@bsort_1 :
	mov sp, bp
	pop bp
	ret
_bsort_1 endp

;@42:	"unit , writeArray, - , -"
_writeArray_3 proc near
	push bp
	mov bp, sp
	sub sp, 6
;@43:	"par  , msg , R , -"
	mov si, word ptr [bp+12]
	push si
;@44:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@45:	":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-2], ax
;@46:	"<    , i, n, (47, 56)"
@label46 :
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp+10]
	cmp ax, cx
	jl  @label47
	jmp @label56
;@47:	">    , i, 0, (48, 50)"
@label47 :
	mov ax, word ptr [bp-2]
	mov cx, 0
	cmp ax, cx
	jg  @label48
	jmp @label50
;@48:	"par  , ", " , R , -"
@label48 :
	lea si, byte ptr @str_1
	push si
;@49:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@50:	"array, x, i, $15"
@label50 :
	mov ax, word ptr [bp-2]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-4], ax
;@51:	"par  , [$15] , V , -"
	mov di, word ptr [bp-4]
	mov ax, word ptr [di]
	push ax
;@52:	"call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@53:	"+    , i, 1, $16"
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-6], ax
;@54:	":=   , $16, - , i"
	mov ax, word ptr [bp-6]
	mov word ptr [bp-2], ax
;@55:	"jump , - , - , 46"
	jmp @label46
;@56:	"par  , "\n" , R , -"
@label56 :
	lea si, byte ptr @str_2
	push si
;@57:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@58:	"endu , writeArray, - , -"
@writeArray_3 :
	mov sp, bp
	pop bp
	ret
_writeArray_3 endp

;@59:	"unit , main, - , -"
_main_0 proc near
	push bp
	mov bp, sp
	sub sp, 56
;@60:	"par  , "Seed :\t" , R , -"
	lea si, byte ptr @str_3
	push si
;@61:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@62:	"par  , $17 , RET , -"
	lea si, word ptr [bp-40]
	push si
;@63:	"call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@64:	":=   , $17, - , seed"
	mov ax, word ptr [bp-40]
	mov word ptr [bp-2], ax
;@65:	"par  , "How do you want to sort?\n" , R , -"
	lea si, byte ptr @str_4
	push si
;@66:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@67:	"par  , "How do you want to sort?\x0a" , R , -"
	lea si, byte ptr @str_5
	push si
;@68:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@69:	"par  , "  1. Min to Max\n" , R , -"
	lea si, byte ptr @str_6
	push si
;@70:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@71:	"par  , "  2. Max to Min\n" , R , -"
	lea si, byte ptr @str_7
	push si
;@72:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@73:	"par  , "Choice : " , R , -"
	lea si, byte ptr @str_8
	push si
;@74:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@75:	"par  , $18 , RET , -"
	lea si, word ptr [bp-42]
	push si
;@76:	"call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@77:	":=   , $18, - , choice"
	mov ax, word ptr [bp-42]
	mov word ptr [bp-4], ax
;@78:	"<    , choice, 1, (80, 79)"
@label78 :
	mov ax, word ptr [bp-4]
	mov cx, 1
	cmp ax, cx
	jl  @label80
	jmp @label79
;@79:	">    , choice, 2, (80, 92)"
@label79 :
	mov ax, word ptr [bp-4]
	mov cx, 2
	cmp ax, cx
	jg  @label80
	jmp @label92
;@80:	"par  , "How do you want to sort?\x0a" , R , -"
@label80 :
	lea si, byte ptr @str_5
	push si
;@81:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@82:	"par  , "  1. Min to Max\n" , R , -"
	lea si, byte ptr @str_6
	push si
;@83:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@84:	"par  , "  2. Max to Min\n" , R , -"
	lea si, byte ptr @str_7
	push si
;@85:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@86:	"par  , "Choice : " , R , -"
	lea si, byte ptr @str_8
	push si
;@87:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@88:	"par  , $19 , RET , -"
	lea si, word ptr [bp-44]
	push si
;@89:	"call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@90:	":=   , $19, - , choice"
	mov ax, word ptr [bp-44]
	mov word ptr [bp-4], ax
;@91:	"jump , - , - , 78"
	jmp @label78
;@92:	":=   , 0, - , i"
@label92 :
	mov ax, 0
	mov word ptr [bp-38], ax
;@93:	"<    , i, 16, (94, 104)"
@label93 :
	mov ax, word ptr [bp-38]
	mov cx, 16
	cmp ax, cx
	jl  @label94
	jmp @label104
;@94:	"*    , seed, 137, $20"
@label94 :
	mov ax, word ptr [bp-2]
	mov cx, 137
	imul cx
	mov word ptr [bp-46], ax
;@95:	"+    , $20, 220, $21"
	mov ax, word ptr [bp-46]
	mov cx, 220
	add ax, cx
	mov word ptr [bp-48], ax
;@96:	"+    , $21, i, $22"
	mov ax, word ptr [bp-48]
	mov cx, word ptr [bp-38]
	add ax, cx
	mov word ptr [bp-50], ax
;@97:	"%    , $22, 101, $23"
	mov ax, word ptr [bp-50]
	cwd
	mov cx, 101
	idiv cx
	mov word ptr [bp-52], dx
;@98:	":=   , $23, - , seed"
	mov ax, word ptr [bp-52]
	mov word ptr [bp-2], ax
;@99:	"array, x, i, $24"
	mov ax, word ptr [bp-38]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-36]
	add ax, cx
	mov word ptr [bp-54], ax
;@100:	":=   , seed, - , [$24]"
	mov ax, word ptr [bp-2]
	mov di, word ptr [bp-54]
	mov word ptr [di], ax
;@101:	"+    , i, 1, $25"
	mov ax, word ptr [bp-38]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-56], ax
;@102:	":=   , $25, - , i"
	mov ax, word ptr [bp-56]
	mov word ptr [bp-38], ax
;@103:	"jump , - , - , 93"
	jmp @label93
;@104:	"par  , "Initial array: " , R , -"
@label104 :
	lea si, byte ptr @str_9
	push si
;@105:	"par  , 16 , V , -"
	mov ax, 16
	push ax
;@106:	"par  , x , R , -"
	lea si, word ptr [bp-36]
	push si
;@107:	"call , - , - , writeArray"
	sub sp, 2
	push bp
	call near ptr _writeArray_3
	add sp, 10
;@108:	"par  , 16 , V , -"
	mov ax, 16
	push ax
;@109:	"par  , choice , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@110:	"par  , x , R , -"
	lea si, word ptr [bp-36]
	push si
;@111:	"call , - , - , bsort"
	sub sp, 2
	push bp
	call near ptr _bsort_1
	add sp, 10
;@112:	"par  , "Sorted array: " , R , -"
	lea si, byte ptr @str_10
	push si
;@113:	"par  , 16 , V , -"
	mov ax, 16
	push ax
;@114:	"par  , x , R , -"
	lea si, word ptr [bp-36]
	push si
;@115:	"call , - , - , writeArray"
	sub sp, 2
	push bp
	call near ptr _writeArray_3
	add sp, 10
;@116:	"endu , main, - , -"
@main_0 :
	mov sp, bp
	pop bp
	ret
_main_0 endp

extrn _writeString : proc
extrn _writeInteger : proc
extrn _readInteger : proc
; ", "
@str_1 db ', ', 0

; "\n"
@str_2 db 10, 0

; "Seed :\t"
@str_3 db 'Seed :', 09, 0

; "How do you want to sort?\n"
@str_4 db 'How do you want to sort?', 10, 0

; "How do you want to sort?\x0a"
@str_5 db 'How do you want to sort?', 10, 0

; "  1. Min to Max\n"
@str_6 db '  1. Min to Max', 10, 0

; "  2. Max to Min\n"
@str_7 db '  2. Max to Min', 10, 0

; "Choice : "
@str_8 db 'Choice : ', 0

; "Initial array: "
@str_9 db 'Initial array: ', 0

; "Sorted array: "
@str_10 db 'Sorted array: ', 0

xseg ends
     end  main


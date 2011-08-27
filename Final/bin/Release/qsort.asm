xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _main_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , swap, - , -"
_swap_3 proc near
	push bp
	mov bp, sp
	sub sp, 2
;@2: ":=   , a, - , t"
	mov si, word ptr [bp+10]
	mov ax, word ptr [si]
	mov word ptr [bp-2], ax
;@3: ":=   , b, - , a"
	mov si, word ptr [bp+8]
	mov ax, word ptr [si]
	mov si, word ptr [bp+10]
	mov word ptr [si], ax
;@4: ":=   , t, - , b"
	mov ax, word ptr [bp-2]
	mov si, word ptr [bp+8]
	mov word ptr [si], ax
;@5: "endu , swap, - , -"
@swap_3 :
	mov sp, bp
	pop bp
	ret
_swap_3 endp

;@6: "unit , qsort_auxil, - , -"
_qsort_auxil_2 proc near
	push bp
	mov bp, sp
	sub sp, 44
;@7: "<    , lower, upper, 9"
	mov ax, word ptr [bp+10]
	mov cx, word ptr [bp+8]
	cmp ax, cx
	jl  @label9
;@8: "jump , - , - , 76"
	jmp @label76
;@9: "+    , lower, upper, $1"
@label9 :
	mov ax, word ptr [bp+10]
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-8], ax
;@10: "/    , $1, 2, $2"
	mov ax, word ptr [bp-8]
	cwd
	mov cx, 2
	idiv cx
	mov word ptr [bp-10], ax
;@11: "array, a, $2, $3"
	mov ax, word ptr [bp-10]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-12], ax
;@12: ":=   , [$3], - , x"
	mov di, word ptr [bp-12]
	mov ax, word ptr [di]
	mov word ptr [bp-2], ax
;@13: ":=   , lower, - , i"
	mov ax, word ptr [bp+10]
	mov word ptr [bp-4], ax
;@14: ":=   , upper, - , j"
	mov ax, word ptr [bp+8]
	mov word ptr [bp-6], ax
;@15: "==   , choice, 1, 17"
	mov si, word ptr [bp+4]
	mov ax, word ptr [si+10]
	mov cx, 1
	cmp ax, cx
	je  @label17
;@16: "jump , - , - , 44"
	jmp @label44
;@17: "<=   , i, j, 19"
@label17 :
	mov ax, word ptr [bp-4]
	mov cx, word ptr [bp-6]
	cmp ax, cx
	jle  @label19
;@18: "jump , - , - , 43"
	jmp @label43
;@19: "array, a, i, $4"
@label19 :
	mov ax, word ptr [bp-4]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-14], ax
;@20: "<    , [$4], x, 22"
	mov di, word ptr [bp-14]
	mov ax, word ptr [di]
	mov cx, word ptr [bp-2]
	cmp ax, cx
	jl  @label22
;@21: "jump , - , - , 25"
	jmp @label25
;@22: "+    , i, 1, $5"
@label22 :
	mov ax, word ptr [bp-4]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-16], ax
;@23: ":=   , $5, - , i"
	mov ax, word ptr [bp-16]
	mov word ptr [bp-4], ax
;@24: "jump , - , - , 19"
	jmp @label19
;@25: "array, a, j, $6"
@label25 :
	mov ax, word ptr [bp-6]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-18], ax
;@26: ">    , [$6], x, 28"
	mov di, word ptr [bp-18]
	mov ax, word ptr [di]
	mov cx, word ptr [bp-2]
	cmp ax, cx
	jg  @label28
;@27: "jump , - , - , 31"
	jmp @label31
;@28: "-    , j, 1, $7"
@label28 :
	mov ax, word ptr [bp-6]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-20], ax
;@29: ":=   , $7, - , j"
	mov ax, word ptr [bp-20]
	mov word ptr [bp-6], ax
;@30: "jump , - , - , 25"
	jmp @label25
;@31: "<=   , i, j, 33"
@label31 :
	mov ax, word ptr [bp-4]
	mov cx, word ptr [bp-6]
	cmp ax, cx
	jle  @label33
;@32: "jump , - , - , 42"
	jmp @label42
;@33: "array, a, i, $8"
@label33 :
	mov ax, word ptr [bp-4]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-22], ax
;@34: "par  , [$8] , R , -"
	mov si, word ptr [bp-22]
	push si
;@35: "array, a, j, $9"
	mov ax, word ptr [bp-6]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-24], ax
;@36: "par  , [$9] , R , -"
	mov si, word ptr [bp-24]
	push si
;@37: "call , - , - , swap"
	sub sp, 2
	push bp
	call near ptr _swap_3
	add sp, 8
;@38: "+    , i, 1, $10"
	mov ax, word ptr [bp-4]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-26], ax
;@39: ":=   , $10, - , i"
	mov ax, word ptr [bp-26]
	mov word ptr [bp-4], ax
;@40: "-    , j, 1, $11"
	mov ax, word ptr [bp-6]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-28], ax
;@41: ":=   , $11, - , j"
	mov ax, word ptr [bp-28]
	mov word ptr [bp-6], ax
;@42: "jump , - , - , 17"
@label42 :
	jmp @label17
;@43: "jump , - , - , 70"
@label43 :
	jmp @label70
;@44: "<=   , i, j, 46"
@label44 :
	mov ax, word ptr [bp-4]
	mov cx, word ptr [bp-6]
	cmp ax, cx
	jle  @label46
;@45: "jump , - , - , 70"
	jmp @label70
;@46: "array, a, i, $12"
@label46 :
	mov ax, word ptr [bp-4]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-30], ax
;@47: ">    , [$12], x, 49"
	mov di, word ptr [bp-30]
	mov ax, word ptr [di]
	mov cx, word ptr [bp-2]
	cmp ax, cx
	jg  @label49
;@48: "jump , - , - , 52"
	jmp @label52
;@49: "+    , i, 1, $13"
@label49 :
	mov ax, word ptr [bp-4]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-32], ax
;@50: ":=   , $13, - , i"
	mov ax, word ptr [bp-32]
	mov word ptr [bp-4], ax
;@51: "jump , - , - , 46"
	jmp @label46
;@52: "array, a, j, $14"
@label52 :
	mov ax, word ptr [bp-6]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-34], ax
;@53: "<    , [$14], x, 55"
	mov di, word ptr [bp-34]
	mov ax, word ptr [di]
	mov cx, word ptr [bp-2]
	cmp ax, cx
	jl  @label55
;@54: "jump , - , - , 58"
	jmp @label58
;@55: "-    , j, 1, $15"
@label55 :
	mov ax, word ptr [bp-6]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-36], ax
;@56: ":=   , $15, - , j"
	mov ax, word ptr [bp-36]
	mov word ptr [bp-6], ax
;@57: "jump , - , - , 52"
	jmp @label52
;@58: "<=   , i, j, 60"
@label58 :
	mov ax, word ptr [bp-4]
	mov cx, word ptr [bp-6]
	cmp ax, cx
	jle  @label60
;@59: "jump , - , - , 69"
	jmp @label69
;@60: "array, a, i, $16"
@label60 :
	mov ax, word ptr [bp-4]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-38], ax
;@61: "par  , [$16] , R , -"
	mov si, word ptr [bp-38]
	push si
;@62: "array, a, j, $17"
	mov ax, word ptr [bp-6]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	mov cx, word ptr [si+8]
	add ax, cx
	mov word ptr [bp-40], ax
;@63: "par  , [$17] , R , -"
	mov si, word ptr [bp-40]
	push si
;@64: "call , - , - , swap"
	sub sp, 2
	push bp
	call near ptr _swap_3
	add sp, 8
;@65: "+    , i, 1, $18"
	mov ax, word ptr [bp-4]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-42], ax
;@66: ":=   , $18, - , i"
	mov ax, word ptr [bp-42]
	mov word ptr [bp-4], ax
;@67: "-    , j, 1, $19"
	mov ax, word ptr [bp-6]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-44], ax
;@68: ":=   , $19, - , j"
	mov ax, word ptr [bp-44]
	mov word ptr [bp-6], ax
;@69: "jump , - , - , 44"
@label69 :
	jmp @label44
;@70: "par  , lower , V , -"
@label70 :
	mov ax, word ptr [bp+10]
	push ax
;@71: "par  , j , V , -"
	mov ax, word ptr [bp-6]
	push ax
;@72: "call , - , - , qsort_auxil"
	sub sp, 2
	push word ptr [bp+4]
	call near ptr _qsort_auxil_2
	add sp, 8
;@73: "par  , i , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@74: "par  , upper , V , -"
	mov ax, word ptr [bp+8]
	push ax
;@75: "call , - , - , qsort_auxil"
	sub sp, 2
	push word ptr [bp+4]
	call near ptr _qsort_auxil_2
	add sp, 8
;@76: "endu , qsort_auxil, - , -"
@label76 :
@qsort_auxil_2 :
	mov sp, bp
	pop bp
	ret
_qsort_auxil_2 endp

;@77: "unit , qsort, - , -"
_qsort_1 proc near
	push bp
	mov bp, sp
	sub sp, 2
;@78: "par  , 0 , V , -"
	mov ax, 0
	push ax
;@79: "-    , n, 1, $20"
	mov ax, word ptr [bp+12]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-2], ax
;@80: "par  , $20 , V , -"
	mov ax, word ptr [bp-2]
	push ax
;@81: "call , - , - , qsort_auxil"
	sub sp, 2
	push bp
	call near ptr _qsort_auxil_2
	add sp, 8
;@82: "endu , qsort, - , -"
@qsort_1 :
	mov sp, bp
	pop bp
	ret
_qsort_1 endp

;@83: "unit , writeArray, - , -"
_writeArray_4 proc near
	push bp
	mov bp, sp
	sub sp, 6
;@84: "par  , msg , R , -"
	mov si, word ptr [bp+12]
	push si
;@85: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@86: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-2], ax
;@87: "<    , i, n, 89"
@label87 :
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp+10]
	cmp ax, cx
	jl  @label89
;@88: "jump , - , - , 99"
	jmp @label99
;@89: ">    , i, 0, 91"
@label89 :
	mov ax, word ptr [bp-2]
	mov cx, 0
	cmp ax, cx
	jg  @label91
;@90: "jump , - , - , 93"
	jmp @label93
;@91: "par  , ", " , R , -"
@label91 :
	lea si, byte ptr @str_0
	push si
;@92: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@93: "array, x, i, $21"
@label93 :
	mov ax, word ptr [bp-2]
	mov cx, 2
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-4], ax
;@94: "par  , [$21] , V , -"
	mov di, word ptr [bp-4]
	mov ax, word ptr [di]
	push ax
;@95: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@96: "+    , i, 1, $22"
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-6], ax
;@97: ":=   , $22, - , i"
	mov ax, word ptr [bp-6]
	mov word ptr [bp-2], ax
;@98: "jump , - , - , 87"
	jmp @label87
;@99: "par  , "\n" , R , -"
@label99 :
	lea si, byte ptr @str_1
	push si
;@100: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@101: "endu , writeArray, - , -"
@writeArray_4 :
	mov sp, bp
	pop bp
	ret
_writeArray_4 endp

;@102: "unit , main, - , -"
_main_0 proc near
	push bp
	mov bp, sp
	sub sp, 56
;@103: "par  , "Seed :\t" , R , -"
	lea si, byte ptr @str_2
	push si
;@104: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@105: "par  , $23 , RET , -"
	lea si, word ptr [bp-40]
	push si
;@106: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@107: ":=   , $23, - , seed"
	mov ax, word ptr [bp-40]
	mov word ptr [bp-2], ax
;@108: "par  , "How do you' want to sort?\n" , R , -"
	lea si, byte ptr @str_3
	push si
;@109: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@110: "par  , "How do you want to sort?\x0a" , R , -"
	lea si, byte ptr @str_4
	push si
;@111: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@112: "par  , "  1. Min to Max\n" , R , -"
	lea si, byte ptr @str_5
	push si
;@113: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@114: "par  , "  2. Max to Min\n" , R , -"
	lea si, byte ptr @str_6
	push si
;@115: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@116: "par  , "Choice : " , R , -"
	lea si, byte ptr @str_7
	push si
;@117: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@118: "par  , $24 , RET , -"
	lea si, word ptr [bp-42]
	push si
;@119: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@120: ":=   , $24, - , choice"
	mov ax, word ptr [bp-42]
	mov word ptr [bp-38], ax
;@121: "<    , choice, 1, 125"
@label121 :
	mov ax, word ptr [bp-38]
	mov cx, 1
	cmp ax, cx
	jl  @label125
;@122: "jump , - , - , 123"
	jmp @label123
;@123: ">    , choice, 2, 125"
@label123 :
	mov ax, word ptr [bp-38]
	mov cx, 2
	cmp ax, cx
	jg  @label125
;@124: "jump , - , - , 137"
	jmp @label137
;@125: "par  , "How do you want to sort?\x0a" , R , -"
@label125 :
	lea si, byte ptr @str_4
	push si
;@126: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@127: "par  , "  1. Min to Max\n" , R , -"
	lea si, byte ptr @str_5
	push si
;@128: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@129: "par  , "  2. Max to Min\n" , R , -"
	lea si, byte ptr @str_6
	push si
;@130: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@131: "par  , "Choice : " , R , -"
	lea si, byte ptr @str_7
	push si
;@132: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@133: "par  , $25 , RET , -"
	lea si, word ptr [bp-44]
	push si
;@134: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@135: ":=   , $25, - , choice"
	mov ax, word ptr [bp-44]
	mov word ptr [bp-38], ax
;@136: "jump , - , - , 121"
	jmp @label121
;@137: ":=   , 0, - , i"
@label137 :
	mov ax, 0
	mov word ptr [bp-36], ax
;@138: "<    , i, 16, 140"
@label138 :
	mov ax, word ptr [bp-36]
	mov cx, 16
	cmp ax, cx
	jl  @label140
;@139: "jump , - , - , 150"
	jmp @label150
;@140: "*    , seed, 137, $26"
@label140 :
	mov ax, word ptr [bp-2]
	mov cx, 137
	imul cx
	mov word ptr [bp-46], ax
;@141: "+    , $26, 220, $27"
	mov ax, word ptr [bp-46]
	mov cx, 220
	add ax, cx
	mov word ptr [bp-48], ax
;@142: "+    , $27, i, $28"
	mov ax, word ptr [bp-48]
	mov cx, word ptr [bp-36]
	add ax, cx
	mov word ptr [bp-50], ax
;@143: "%    , $28, 101, $29"
	mov ax, word ptr [bp-50]
	cwd
	mov cx, 101
	idiv cx
	mov word ptr [bp-52], dx
;@144: ":=   , $29, - , seed"
	mov ax, word ptr [bp-52]
	mov word ptr [bp-2], ax
;@145: "array, x, i, $30"
	mov ax, word ptr [bp-36]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-34]
	add ax, cx
	mov word ptr [bp-54], ax
;@146: ":=   , seed, - , [$30]"
	mov ax, word ptr [bp-2]
	mov di, word ptr [bp-54]
	mov word ptr [di], ax
;@147: "+    , i, 1, $31"
	mov ax, word ptr [bp-36]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-56], ax
;@148: ":=   , $31, - , i"
	mov ax, word ptr [bp-56]
	mov word ptr [bp-36], ax
;@149: "jump , - , - , 138"
	jmp @label138
;@150: "par  , "Initial array; " , R , -"
@label150 :
	lea si, byte ptr @str_8
	push si
;@151: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@152: "par  , x , R , -"
	lea si, word ptr [bp-34]
	push si
;@153: "call , - , - , writeArray"
	sub sp, 2
	push bp
	call near ptr _writeArray_4
	add sp, 10
;@154: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@155: "par  , choice , V , -"
	mov ax, word ptr [bp-38]
	push ax
;@156: "par  , x , R , -"
	lea si, word ptr [bp-34]
	push si
;@157: "call , - , - , qsort"
	sub sp, 2
	push bp
	call near ptr _qsort_1
	add sp, 10
;@158: "par  , "Sorted array: " , R , -"
	lea si, byte ptr @str_9
	push si
;@159: "par  , 16 , V , -"
	mov ax, 16
	push ax
;@160: "par  , x , R , -"
	lea si, word ptr [bp-34]
	push si
;@161: "call , - , - , writeArray"
	sub sp, 2
	push bp
	call near ptr _writeArray_4
	add sp, 10
;@162: "endu , main, - , -"
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

; "How do you' want to sort?\n"
@str_3 db 'How do you', 27, ' want to sort?', 10, 0

; "How do you want to sort?\x0a"
@str_4 db 'How do you want to sort?', 10, 0

; "  1. Min to Max\n"
@str_5 db '  1. Min to Max', 10, 0

; "  2. Max to Min\n"
@str_6 db '  2. Max to Min', 10, 0

; "Choice : "
@str_7 db 'Choice : ', 0

; "Initial array; "
@str_8 db 'Initial array; ', 0

; "Sorted array: "
@str_9 db 'Sorted array: ', 0

xseg ends
     end  main


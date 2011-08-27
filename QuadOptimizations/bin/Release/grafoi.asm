xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _grafoi_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , find_root, - , -"
_find_root_1 proc near
	push bp
	mov bp, sp
	sub sp, 6
;@2: ":=   , 1, - , i"
	mov ax, 1
	mov word ptr [bp-2], ax
;@3: "<    , i, n, 5"
@label3 :
	mov ax, word ptr [bp-2]
	mov si, word ptr [bp+4]
	mov cx, word ptr [si-8010]
	cmp ax, cx
	jl  @label5
;@4: "jump , - , - , 13"
	jmp @label13
;@5: "array, first_parent, i, $1"
@label5 :
	mov ax, word ptr [bp-2]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	lea cx, word ptr [si-6006]
	add ax, cx
	mov word ptr [bp-4], ax
;@6: "==   , [$1], 0, 8"
	mov di, word ptr [bp-4]
	mov ax, word ptr [di]
	mov cx, 0
	cmp ax, cx
	je  @label8
;@7: "jump , - , - , 10"
	jmp @label10
;@8: ":=   , i, - , $$"
@label8 :
	mov ax, word ptr [bp-2]
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@9: "ret  , -, -, -"
	jmp @find_root_1
;@10: "+    , i, 1, $2"
@label10 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-6], ax
;@11: ":=   , $2, - , i"
	mov ax, word ptr [bp-6]
	mov word ptr [bp-2], ax
;@12: "jump , - , - , 3"
	jmp @label3
;@13: "endu , find_root, - , -"
@label13 :
@find_root_1 :
	mov sp, bp
	pop bp
	ret
_find_root_1 endp

;@14: "unit , find_start_of_path, - , -"
_find_start_of_path_2 proc near
	push bp
	mov bp, sp
	sub sp, 6
;@15: "array, second_parent, node, $3"
	mov ax, word ptr [bp+8]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	lea cx, word ptr [si-8008]
	add ax, cx
	mov word ptr [bp-2], ax
;@16: ">    , [$3], 0, 18"
	mov di, word ptr [bp-2]
	mov ax, word ptr [di]
	mov cx, 0
	cmp ax, cx
	jg  @label18
;@17: "jump , - , - , 25"
	jmp @label25
;@18: "par  , node , V , -"
@label18 :
	mov ax, word ptr [bp+8]
	push ax
;@19: "array, first_child, node, $4"
	mov ax, word ptr [bp+8]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	lea cx, word ptr [si-2002]
	add ax, cx
	mov word ptr [bp-4], ax
;@20: "par  , [$4] , V , -"
	mov di, word ptr [bp-4]
	mov ax, word ptr [di]
	push ax
;@21: "par  , $5 , RET , -"
	lea si, word ptr [bp-6]
	push si
;@22: "call , - , - , find_start_of_path"
	push word ptr [bp+4]
	call near ptr _find_start_of_path_2
	add sp, 8
;@23: ":=   , $5, - , $$"
	mov ax, word ptr [bp-6]
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@24: "ret  , -, -, -"
	jmp @find_start_of_path_2
;@25: ":=   , previous, - , $$"
@label25 :
	mov ax, word ptr [bp+10]
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@26: "ret  , -, -, -"
	jmp @find_start_of_path_2
;@27: "endu , find_start_of_path, - , -"
@find_start_of_path_2 :
	mov sp, bp
	pop bp
	ret
_find_start_of_path_2 endp

;@28: "unit , find_path, - , -"
_find_path_3 proc near
	push bp
	mov bp, sp
	sub sp, 6
;@29: "par  , node , V , -"
	mov ax, word ptr [bp+8]
	push ax
;@30: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@31: "par  , " " , R , -"
	lea si, byte ptr @str_0
	push si
;@32: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@33: "array, first_child, node, $6"
	mov ax, word ptr [bp+8]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	lea cx, word ptr [si-2002]
	add ax, cx
	mov word ptr [bp-2], ax
;@34: "==   , [$6], 0, 36"
	mov di, word ptr [bp-2]
	mov ax, word ptr [di]
	mov cx, 0
	cmp ax, cx
	je  @label36
;@35: "jump , - , - , 37"
	jmp @label37
;@36: "ret  , -, -, -"
@label36 :
	jmp @find_path_3
;@37: "array, second_child, node, $7"
@label37 :
	mov ax, word ptr [bp+8]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	lea cx, word ptr [si-4004]
	add ax, cx
	mov word ptr [bp-4], ax
;@38: "==   , [$7], 0, 40"
	mov di, word ptr [bp-4]
	mov ax, word ptr [di]
	mov cx, 0
	cmp ax, cx
	je  @label40
;@39: "jump , - , - , 44"
	jmp @label44
;@40: "array, first_child, node, $8"
@label40 :
	mov ax, word ptr [bp+8]
	mov cx, 2
	imul cx
	mov si, word ptr [bp+4]
	lea cx, word ptr [si-2002]
	add ax, cx
	mov word ptr [bp-6], ax
;@41: "par  , [$8] , V , -"
	mov di, word ptr [bp-6]
	mov ax, word ptr [di]
	push ax
;@42: "call , - , - , find_path"
	sub sp, 2
	push word ptr [bp+4]
	call near ptr _find_path_3
	add sp, 6
;@43: "jump , - , - , 45"
	jmp @label45
;@44: "ret  , -, -, -"
@label44 :
	jmp @find_path_3
;@45: "endu , find_path, - , -"
@label45 :
@find_path_3 :
	mov sp, bp
	pop bp
	ret
_find_path_3 endp

;@46: "unit , grafoi, - , -"
_grafoi_0 proc near
	push bp
	mov bp, sp
	sub sp, 8060
;@47: "par  , $9 , RET , -"
	lea si, word ptr [bp-8022]
	push si
;@48: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@49: ":=   , $9, - , n"
	mov ax, word ptr [bp-8022]
	mov word ptr [bp-8010], ax
;@50: ":=   , 1, - , i"
	mov ax, 1
	mov word ptr [bp-8012], ax
;@51: "<=   , i, n, 53"
@label51 :
	mov ax, word ptr [bp-8012]
	mov cx, word ptr [bp-8010]
	cmp ax, cx
	jle  @label53
;@52: "jump , - , - , 64"
	jmp @label64
;@53: "array, first_child, i, $10"
@label53 :
	mov ax, word ptr [bp-8012]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-2002]
	add ax, cx
	mov word ptr [bp-8024], ax
;@54: ":=   , 0, - , [$10]"
	mov ax, 0
	mov di, word ptr [bp-8024]
	mov word ptr [di], ax
;@55: "array, second_child, i, $11"
	mov ax, word ptr [bp-8012]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-4004]
	add ax, cx
	mov word ptr [bp-8026], ax
;@56: ":=   , 0, - , [$11]"
	mov ax, 0
	mov di, word ptr [bp-8026]
	mov word ptr [di], ax
;@57: "array, first_parent, i, $12"
	mov ax, word ptr [bp-8012]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-6006]
	add ax, cx
	mov word ptr [bp-8028], ax
;@58: ":=   , 0, - , [$12]"
	mov ax, 0
	mov di, word ptr [bp-8028]
	mov word ptr [di], ax
;@59: "array, second_parent, i, $13"
	mov ax, word ptr [bp-8012]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-8008]
	add ax, cx
	mov word ptr [bp-8030], ax
;@60: ":=   , 0, - , [$13]"
	mov ax, 0
	mov di, word ptr [bp-8030]
	mov word ptr [di], ax
;@61: "+    , i, 1, $14"
	mov ax, word ptr [bp-8012]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-8032], ax
;@62: ":=   , $14, - , i"
	mov ax, word ptr [bp-8032]
	mov word ptr [bp-8012], ax
;@63: "jump , - , - , 51"
	jmp @label51
;@64: ":=   , 1, - , i"
@label64 :
	mov ax, 1
	mov word ptr [bp-8012], ax
;@65: "-    , n, 1, $15"
@label65 :
	mov ax, word ptr [bp-8010]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-8034], ax
;@66: "<=   , i, $15, 68"
	mov ax, word ptr [bp-8012]
	mov cx, word ptr [bp-8034]
	cmp ax, cx
	jle  @label68
;@67: "jump , - , - , 93"
	jmp @label93
;@68: "par  , $16 , RET , -"
@label68 :
	lea si, word ptr [bp-8036]
	push si
;@69: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@70: ":=   , $16, - , a"
	mov ax, word ptr [bp-8036]
	mov word ptr [bp-8014], ax
;@71: "par  , $17 , RET , -"
	lea si, word ptr [bp-8038]
	push si
;@72: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@73: ":=   , $17, - , b"
	mov ax, word ptr [bp-8038]
	mov word ptr [bp-8016], ax
;@74: "array, first_child, a, $18"
	mov ax, word ptr [bp-8014]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-2002]
	add ax, cx
	mov word ptr [bp-8040], ax
;@75: ">    , [$18], 0, 77"
	mov di, word ptr [bp-8040]
	mov ax, word ptr [di]
	mov cx, 0
	cmp ax, cx
	jg  @label77
;@76: "jump , - , - , 80"
	jmp @label80
;@77: "array, second_child, a, $19"
@label77 :
	mov ax, word ptr [bp-8014]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-4004]
	add ax, cx
	mov word ptr [bp-8042], ax
;@78: ":=   , b, - , [$19]"
	mov ax, word ptr [bp-8016]
	mov di, word ptr [bp-8042]
	mov word ptr [di], ax
;@79: "jump , - , - , 82"
	jmp @label82
;@80: "array, first_child, a, $20"
@label80 :
	mov ax, word ptr [bp-8014]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-2002]
	add ax, cx
	mov word ptr [bp-8044], ax
;@81: ":=   , b, - , [$20]"
	mov ax, word ptr [bp-8016]
	mov di, word ptr [bp-8044]
	mov word ptr [di], ax
;@82: "array, first_parent, b, $21"
@label82 :
	mov ax, word ptr [bp-8016]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-6006]
	add ax, cx
	mov word ptr [bp-8046], ax
;@83: ">    , [$21], 0, 85"
	mov di, word ptr [bp-8046]
	mov ax, word ptr [di]
	mov cx, 0
	cmp ax, cx
	jg  @label85
;@84: "jump , - , - , 88"
	jmp @label88
;@85: "array, second_parent, b, $22"
@label85 :
	mov ax, word ptr [bp-8016]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-8008]
	add ax, cx
	mov word ptr [bp-8048], ax
;@86: ":=   , a, - , [$22]"
	mov ax, word ptr [bp-8014]
	mov di, word ptr [bp-8048]
	mov word ptr [di], ax
;@87: "jump , - , - , 90"
	jmp @label90
;@88: "array, first_parent, b, $23"
@label88 :
	mov ax, word ptr [bp-8016]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-6006]
	add ax, cx
	mov word ptr [bp-8050], ax
;@89: ":=   , a, - , [$23]"
	mov ax, word ptr [bp-8014]
	mov di, word ptr [bp-8050]
	mov word ptr [di], ax
;@90: "+    , i, 1, $24"
@label90 :
	mov ax, word ptr [bp-8012]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-8052], ax
;@91: ":=   , $24, - , i"
	mov ax, word ptr [bp-8052]
	mov word ptr [bp-8012], ax
;@92: "jump , - , - , 65"
	jmp @label65
;@93: "par  , $25 , RET , -"
@label93 :
	lea si, word ptr [bp-8054]
	push si
;@94: "call , - , - , find_root"
	push bp
	call near ptr _find_root_1
	add sp, 4
;@95: ":=   , $25, - , start"
	mov ax, word ptr [bp-8054]
	mov word ptr [bp-8018], ax
;@96: "array, first_child, start, $26"
	mov ax, word ptr [bp-8018]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-2002]
	add ax, cx
	mov word ptr [bp-8056], ax
;@97: "==   , [$26], 0, 99"
	mov di, word ptr [bp-8056]
	mov ax, word ptr [di]
	mov cx, 0
	cmp ax, cx
	je  @label99
;@98: "jump , - , - , 104"
	jmp @label104
;@99: "par  , 0 , V , -"
@label99 :
	mov ax, 0
	push ax
;@100: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@101: "par  , "\n" , R , -"
	lea si, byte ptr @str_1
	push si
;@102: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@103: "ret  , -, -, -"
	jmp @grafoi_0
;@104: "par  , start , V , -"
@label104 :
	mov ax, word ptr [bp-8018]
	push ax
;@105: "array, first_child, start, $27"
	mov ax, word ptr [bp-8018]
	mov cx, 2
	imul cx
	lea cx, word ptr [bp-2002]
	add ax, cx
	mov word ptr [bp-8058], ax
;@106: "par  , [$27] , V , -"
	mov di, word ptr [bp-8058]
	mov ax, word ptr [di]
	push ax
;@107: "par  , $28 , RET , -"
	lea si, word ptr [bp-8060]
	push si
;@108: "call , - , - , find_start_of_path"
	push bp
	call near ptr _find_start_of_path_2
	add sp, 8
;@109: ":=   , $28, - , start"
	mov ax, word ptr [bp-8060]
	mov word ptr [bp-8018], ax
;@110: "par  , start , V , -"
	mov ax, word ptr [bp-8018]
	push ax
;@111: "call , - , - , find_path"
	sub sp, 2
	push bp
	call near ptr _find_path_3
	add sp, 6
;@112: "endu , grafoi, - , -"
@grafoi_0 :
	mov sp, bp
	pop bp
	ret
_grafoi_0 endp

extrn _writeInteger : proc
extrn _writeString : proc
extrn _readInteger : proc
; " "
@str_0 db ' ', 0

; "\n"
@str_1 db 10, 0

xseg ends
     end  main


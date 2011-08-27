xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _cancer_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , is_it, - , -"
_is_it_1 proc near
	push bp
	mov bp, sp
	sub sp, 22
;@2: "-    , n, 1, $1"
	mov ax, word ptr [bp+10]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-4], ax
;@3: ":=   , $1, - , n"
	mov ax, word ptr [bp-4]
	mov word ptr [bp+10], ax
;@4: ":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-2], ax
;@5: "/    , n, 2, $2"
@label5 :
	mov ax, word ptr [bp+10]
	cwd
	mov cx, 2
	idiv cx
	mov word ptr [bp-6], ax
;@6: "+    , $2, 1, $3"
	mov ax, word ptr [bp-6]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-8], ax
;@7: "<    , i, $3, 9"
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp-8]
	cmp ax, cx
	jl  @label9
;@8: "jump , - , - , 32"
	jmp @label32
;@9: "array, source, i, $4"
@label9 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-10], ax
;@10: "-    , n, i, $5"
	mov ax, word ptr [bp+10]
	mov cx, word ptr [bp-2]
	sub ax, cx
	mov word ptr [bp-12], ax
;@11: "array, source, $5, $6"
	mov ax, word ptr [bp-12]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-14], ax
;@12: "!=   , [$4], [$6], 14"
	mov di, word ptr [bp-10]
	mov al, byte ptr [di]
	mov di, word ptr [bp-14]
	mov cl, byte ptr [di]
	cmp al, cl
	jne  @label14
;@13: "jump , - , - , 29"
	jmp @label29
;@14: "array, source, i, $7"
@label14 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-16], ax
;@15: "par  , [$7] , V , -"
	mov di, word ptr [bp-16]
	mov al, byte ptr [di]
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@16: "call , - , - , writeChar"
	sub sp, 2
	push bp
	call near ptr _writeChar
	add sp, 5
;@17: "-    , n, i, $8"
	mov ax, word ptr [bp+10]
	mov cx, word ptr [bp-2]
	sub ax, cx
	mov word ptr [bp-18], ax
;@18: "array, source, $8, $9"
	mov ax, word ptr [bp-18]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-20], ax
;@19: "par  , [$9] , V , -"
	mov di, word ptr [bp-20]
	mov al, byte ptr [di]
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@20: "call , - , - , writeChar"
	sub sp, 2
	push bp
	call near ptr _writeChar
	add sp, 5
;@21: "par  , "The incarcinity was at position " , R , -"
	lea si, byte ptr @str_0
	push si
;@22: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@23: "par  , i , V , -"
	mov ax, word ptr [bp-2]
	push ax
;@24: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@25: "par  , ".\n" , R , -"
	lea si, byte ptr @str_1
	push si
;@26: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@27: ":=   , 1, - , $$"
	mov ax, 1
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@28: "ret  , -, -, -"
	jmp @is_it_1
;@29: "+    , i, 1, $10"
@label29 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-22], ax
;@30: ":=   , $10, - , i"
	mov ax, word ptr [bp-22]
	mov word ptr [bp-2], ax
;@31: "jump , - , - , 5"
	jmp @label5
;@32: ":=   , 0, - , $$"
@label32 :
	mov ax, 0
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@33: "ret  , -, -, -"
	jmp @is_it_1
;@34: "endu , is_it, - , -"
@is_it_1 :
	mov sp, bp
	pop bp
	ret
_is_it_1 endp

;@35: "unit , cancer, - , -"
_cancer_0 proc near
	push bp
	mov bp, sp
	sub sp, 39
;@36: "par  , "Give a string with maximum length 30: " , R , -"
	lea si, byte ptr @str_2
	push si
;@37: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@38: "par  , 30 , V , -"
	mov ax, 30
	push ax
;@39: "par  , source , R , -"
	lea si, word ptr [bp-33]
	push si
;@40: "call , - , - , readString"
	sub sp, 2
	push bp
	call near ptr _readString
	add sp, 8
;@41: ":=   , 0, - , n"
	mov ax, 0
	mov word ptr [bp-2], ax
;@42: "array, source, n, $11"
@label42 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-33]
	add ax, cx
	mov word ptr [bp-35], ax
;@43: "!=   , [$11], 0, 45"
	mov di, word ptr [bp-35]
	mov al, byte ptr [di]
	mov cl, 0
	cmp al, cl
	jne  @label45
;@44: "jump , - , - , 48"
	jmp @label48
;@45: "+    , n, 1, $12"
@label45 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-37], ax
;@46: ":=   , $12, - , n"
	mov ax, word ptr [bp-37]
	mov word ptr [bp-2], ax
;@47: "jump , - , - , 42"
	jmp @label42
;@48: "par  , n , V , -"
@label48 :
	mov ax, word ptr [bp-2]
	push ax
;@49: "par  , source , R , -"
	lea si, word ptr [bp-33]
	push si
;@50: "par  , $13 , RET , -"
	lea si, word ptr [bp-39]
	push si
;@51: "call , - , - , is_it"
	push bp
	call near ptr _is_it_1
	add sp, 8
;@52: "==   , $13, 1, 54"
	mov ax, word ptr [bp-39]
	mov cx, 1
	cmp ax, cx
	je  @label54
;@53: "jump , - , - , 57"
	jmp @label57
;@54: "par  , "\nIs not cancern..." , R , -"
@label54 :
	lea si, byte ptr @str_3
	push si
;@55: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@56: "jump , - , - , 59"
	jmp @label59
;@57: "par  , "\nIs cancern..." , R , -"
@label57 :
	lea si, byte ptr @str_4
	push si
;@58: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@59: "endu , cancer, - , -"
@label59 :
@cancer_0 :
	mov sp, bp
	pop bp
	ret
_cancer_0 endp

extrn _writeChar : proc
extrn _writeString : proc
extrn _writeInteger : proc
extrn _readString : proc
; "The incarcinity was at position "
@str_0 db 'The incarcinity was at position ', 0

; ".\n"
@str_1 db '.', 10, 0

; "Give a string with maximum length 30: "
@str_2 db 'Give a string with maximum length 30: ', 0

; "\nIs not cancern..."
@str_3 db 10, 'Is not cancern...', 0

; "\nIs cancern..."
@str_4 db 10, 'Is cancern...', 0

xseg ends
     end  main


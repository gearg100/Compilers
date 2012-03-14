xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _cancer_0
     mov ax, 4C00h
     int 21h
main endp

;@1:	"unit , is_it, - , -"
_is_it_1 proc near
	push bp
	mov bp, sp
	sub sp, 22
;@2:	"-    , n, 1, $1"
	mov ax, word ptr [bp+10]
	mov cx, 1
	sub ax, cx
	mov word ptr [bp-4], ax
;@3:	":=   , $1, - , n"
	mov ax, word ptr [bp-4]
	mov word ptr [bp+10], ax
;@4:	":=   , 0, - , i"
	mov ax, 0
	mov word ptr [bp-2], ax
;@5:	"/    , n, 2, $2"
@label5 :
	mov ax, word ptr [bp+10]
	cwd
	mov cx, 2
	idiv cx
	mov word ptr [bp-6], ax
;@6:	"+    , $2, 1, $3"
	mov ax, word ptr [bp-6]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-8], ax
;@7:	"<    , i, $3, (8, 30)"
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp-8]
	cmp ax, cx
	jl  @label8
	jmp @label30
;@8:	"array, source, i, $4"
@label8 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-10], ax
;@9:	"-    , n, i, $5"
	mov ax, word ptr [bp+10]
	mov cx, word ptr [bp-2]
	sub ax, cx
	mov word ptr [bp-12], ax
;@10:	"array, source, $5, $6"
	mov ax, word ptr [bp-12]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-14], ax
;@11:	"!=   , [$4], [$6], (12, 27)"
	mov di, word ptr [bp-10]
	mov al, byte ptr [di]
	mov di, word ptr [bp-14]
	mov cl, byte ptr [di]
	cmp al, cl
	jne  @label12
	jmp @label27
;@12:	"array, source, i, $7"
@label12 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-16], ax
;@13:	"par  , [$7] , V , -"
	mov di, word ptr [bp-16]
	mov al, byte ptr [di]
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@14:	"call , - , - , writeChar"
	sub sp, 2
	push bp
	call near ptr _writeChar
	add sp, 5
;@15:	"-    , n, i, $8"
	mov ax, word ptr [bp+10]
	mov cx, word ptr [bp-2]
	sub ax, cx
	mov word ptr [bp-18], ax
;@16:	"array, source, $8, $9"
	mov ax, word ptr [bp-18]
	mov cx, 1
	imul cx
	mov cx, word ptr [bp+8]
	add ax, cx
	mov word ptr [bp-20], ax
;@17:	"par  , [$9] , V , -"
	mov di, word ptr [bp-20]
	mov al, byte ptr [di]
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@18:	"call , - , - , writeChar"
	sub sp, 2
	push bp
	call near ptr _writeChar
	add sp, 5
;@19:	"par  , "The incarcinity was at position " , R , -"
	lea si, byte ptr @str_1
	push si
;@20:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@21:	"par  , i , V , -"
	mov ax, word ptr [bp-2]
	push ax
;@22:	"call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@23:	"par  , ".\n" , R , -"
	lea si, byte ptr @str_2
	push si
;@24:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@25:	":=   , 1, - , $$"
	mov ax, 1
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@26:	"ret  , -, -, -"
	jmp @is_it_1
;@27:	"+    , i, 1, $10"
@label27 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-22], ax
;@28:	":=   , $10, - , i"
	mov ax, word ptr [bp-22]
	mov word ptr [bp-2], ax
;@29:	"jump , - , - , 5"
	jmp @label5
;@30:	":=   , 0, - , $$"
@label30 :
	mov ax, 0
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@31:	"ret  , -, -, -"
	jmp @is_it_1
;@32:	"endu , is_it, - , -"
@is_it_1 :
	mov sp, bp
	pop bp
	ret
_is_it_1 endp

;@33:	"unit , cancer, - , -"
_cancer_0 proc near
	push bp
	mov bp, sp
	sub sp, 39
;@34:	"par  , "Give a string with maximum length 30: " , R , -"
	lea si, byte ptr @str_3
	push si
;@35:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@36:	"par  , 30 , V , -"
	mov ax, 30
	push ax
;@37:	"par  , source , R , -"
	lea si, word ptr [bp-33]
	push si
;@38:	"call , - , - , readString"
	sub sp, 2
	push bp
	call near ptr _readString
	add sp, 8
;@39:	":=   , 0, - , n"
	mov ax, 0
	mov word ptr [bp-2], ax
;@40:	"array, source, n, $11"
@label40 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-33]
	add ax, cx
	mov word ptr [bp-35], ax
;@41:	"!=   , [$11], 0, (42, 45)"
	mov di, word ptr [bp-35]
	mov al, byte ptr [di]
	mov cl, 0
	cmp al, cl
	jne  @label42
	jmp @label45
;@42:	"+    , n, 1, $12"
@label42 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-37], ax
;@43:	":=   , $12, - , n"
	mov ax, word ptr [bp-37]
	mov word ptr [bp-2], ax
;@44:	"jump , - , - , 40"
	jmp @label40
;@45:	"par  , n , V , -"
@label45 :
	mov ax, word ptr [bp-2]
	push ax
;@46:	"par  , source , R , -"
	lea si, word ptr [bp-33]
	push si
;@47:	"par  , $13 , RET , -"
	lea si, word ptr [bp-39]
	push si
;@48:	"call , - , - , is_it"
	push bp
	call near ptr _is_it_1
	add sp, 8
;@49:	"==   , $13, 1, (50, 53)"
	mov ax, word ptr [bp-39]
	mov cx, 1
	cmp ax, cx
	je  @label50
	jmp @label53
;@50:	"par  , "\nIs not cancern..." , R , -"
@label50 :
	lea si, byte ptr @str_4
	push si
;@51:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@52:	"jump , - , - , 55"
	jmp @label55
;@53:	"par  , "\nIs cancern..." , R , -"
@label53 :
	lea si, byte ptr @str_5
	push si
;@54:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@55:	"endu , cancer, - , -"
@label55 :
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
@str_1 db 'The incarcinity was at position ', 0

; ".\n"
@str_2 db '.', 10, 0

; "Give a string with maximum length 30: "
@str_3 db 'Give a string with maximum length 30: ', 0

; "\nIs not cancern..."
@str_4 db 10, 'Is not cancern...', 0

; "\nIs cancern..."
@str_5 db 10, 'Is cancern...', 0

xseg ends
     end  main


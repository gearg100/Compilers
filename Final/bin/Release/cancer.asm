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
	sub sp, 16
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
;@8: "jump , - , - , 19"
	jmp @label19
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
;@13: "jump , - , - , 16"
	jmp @label16
;@14: ":=   , 1, - , $$"
@label14 :
	mov ax, 1
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@15: "ret  , -, -, -"
	jmp @is_it_1
;@16: "+    , i, 1, $7"
@label16 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-16], ax
;@17: ":=   , $7, - , i"
	mov ax, word ptr [bp-16]
	mov word ptr [bp-2], ax
;@18: "jump , - , - , 5"
	jmp @label5
;@19: ":=   , 0, - , $$"
@label19 :
	mov ax, 0
	mov si, word ptr [bp+6]
	mov word ptr [si], ax
;@20: "ret  , -, -, -"
	jmp @is_it_1
;@21: "endu , is_it, - , -"
@is_it_1 :
	mov sp, bp
	pop bp
	ret
_is_it_1 endp

;@22: "unit , cancer, - , -"
_cancer_0 proc near
	push bp
	mov bp, sp
	sub sp, 39
;@23: "par  , "Give a string with maximum length 30: " , R , -"
	lea si, byte ptr @str_0
	push si
;@24: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@25: "par  , 30 , V , -"
	mov ax, 30
	push ax
;@26: "par  , source , R , -"
	lea si, word ptr [bp-33]
	push si
;@27: "call , - , - , readString"
	sub sp, 2
	push bp
	call near ptr _readString
	add sp, 8
;@28: ":=   , 0, - , n"
	mov ax, 0
	mov word ptr [bp-2], ax
;@29: "array, source, n, $8"
@label29 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	imul cx
	lea cx, word ptr [bp-33]
	add ax, cx
	mov word ptr [bp-35], ax
;@30: "!=   , [$8], 0, 32"
	mov di, word ptr [bp-35]
	mov al, byte ptr [di]
	mov cl, 0
	cmp al, cl
	jne  @label32
;@31: "jump , - , - , 35"
	jmp @label35
;@32: "+    , n, 1, $9"
@label32 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-37], ax
;@33: ":=   , $9, - , n"
	mov ax, word ptr [bp-37]
	mov word ptr [bp-2], ax
;@34: "jump , - , - , 29"
	jmp @label29
;@35: "par  , n , V , -"
@label35 :
	mov ax, word ptr [bp-2]
	push ax
;@36: "par  , source , R , -"
	lea si, word ptr [bp-33]
	push si
;@37: "par  , $10 , RET , -"
	lea si, word ptr [bp-39]
	push si
;@38: "call , - , - , is_it"
	push bp
	call near ptr _is_it_1
	add sp, 8
;@39: "==   , $10, 1, 41"
	mov ax, word ptr [bp-39]
	mov cx, 1
	cmp ax, cx
	je  @label41
;@40: "jump , - , - , 44"
	jmp @label44
;@41: "par  , "\nIs not cancern..." , R , -"
@label41 :
	lea si, byte ptr @str_1
	push si
;@42: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@43: "jump , - , - , 46"
	jmp @label46
;@44: "par  , "\nIs cancern..." , R , -"
@label44 :
	lea si, byte ptr @str_2
	push si
;@45: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@46: "endu , cancer, - , -"
@label46 :
@cancer_0 :
	mov sp, bp
	pop bp
	ret
_cancer_0 endp

extrn _writeString : proc
extrn _readString : proc
; "Give a string with maximum length 30: "
@str_0 db 'Give a string with maximum length 30: ', 0

; "\nIs not cancern..."
@str_1 db 10, 'Is not cancern...', 0

; "\nIs cancern..."
@str_2 db 10, 'Is cancern...', 0

xseg ends
     end  main


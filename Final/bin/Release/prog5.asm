xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _prog5_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , prog5, - , -"
_prog5_0 proc near
	push bp
	mov bp, sp
	sub sp, 14
;@2: "par  , "Give me a number:" , R , -"
	lea si, byte ptr @str_0
	push si
;@3: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@4: "par  , " " , R , -"
	lea si, byte ptr @str_1
	push si
;@5: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@6: "par  , $1 , RET , -"
	lea si, word ptr [bp-5]
	push si
;@7: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@8: ":=   , $1, - , n"
	mov ax, word ptr [bp-5]
	mov word ptr [bp-3], ax
;@9: "par  , 10 , V , -"
	mov ax, 10
	push ax
;@10: "par  , $2 , RET , -"
	lea si, byte ptr [bp-6]
	push si
;@11: "call , - , - , shrink"
	push bp
	call near ptr _shrink
	add sp, 6
;@12: "par  , $2 , V , -"
	mov al, byte ptr [bp-6]
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@13: "call , - , - , writeChar"
	sub sp, 2
	push bp
	call near ptr _writeChar
	add sp, 5
;@14: ":=   , 0, - , i"
	mov al, 0
	mov byte ptr [bp-1], al
;@15: "par  , n , V , -"
@label15 :
	mov ax, word ptr [bp-3]
	push ax
;@16: "par  , $3 , RET , -"
	lea si, byte ptr [bp-7]
	push si
;@17: "call , - , - , shrink"
	push bp
	call near ptr _shrink
	add sp, 6
;@18: "<=   , i, $3, 20"
	mov al, byte ptr [bp-1]
	mov cl, byte ptr [bp-7]
	cmp al, cl
	jle  @label20
;@19: "jump , - , - , 38"
	jmp @label38
;@20: "%    , i, 2, $4"
@label20 :
	mov al, byte ptr [bp-1]
	cbw
	mov cl, 2
	idiv cl
	mov byte ptr [bp-8], ah
;@21: "==   , $4, 0, 23"
	mov al, byte ptr [bp-8]
	mov cl, 0
	cmp al, cl
	je  @label23
;@22: "jump , - , - , 35"
	jmp @label35
;@23: "*    , i, 2, $5"
@label23 :
	mov al, byte ptr [bp-1]
	mov cl, 2
	imul cl
	mov byte ptr [bp-9], al
;@24: "/    , $5, 3, $6"
	mov al, byte ptr [bp-9]
	cbw
	mov cl, 3
	idiv cl
	mov byte ptr [bp-10], al
;@25: "par  , $6 , V , -"
	mov al, byte ptr [bp-10]
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@26: "par  , $7 , RET , -"
	lea si, word ptr [bp-12]
	push si
;@27: "call , - , - , extend"
	push bp
	call near ptr _extend
	add sp, 5
;@28: "par  , $7 , V , -"
	mov ax, word ptr [bp-12]
	push ax
;@29: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@30: "par  , 10 , V , -"
	mov ax, 10
	push ax
;@31: "par  , $8 , RET , -"
	lea si, byte ptr [bp-13]
	push si
;@32: "call , - , - , shrink"
	push bp
	call near ptr _shrink
	add sp, 6
;@33: "par  , $8 , V , -"
	mov al, byte ptr [bp-13]
	sub sp, 1
	mov si, sp
	mov byte ptr [si], al
;@34: "call , - , - , writeChar"
	sub sp, 2
	push bp
	call near ptr _writeChar
	add sp, 5
;@35: "+    , i, 1, $9"
@label35 :
	mov al, byte ptr [bp-1]
	mov cl, 1
	add al, cl
	mov byte ptr [bp-14], al
;@36: ":=   , $9, - , i"
	mov al, byte ptr [bp-14]
	mov byte ptr [bp-1], al
;@37: "jump , - , - , 15"
	jmp @label15
;@38: "endu , prog5, - , -"
@label38 :
@prog5_0 :
	mov sp, bp
	pop bp
	ret
_prog5_0 endp

extrn _writeString : proc
extrn _readInteger : proc
extrn _shrink : proc
extrn _writeChar : proc
extrn _extend : proc
extrn _writeInteger : proc
; "Give me a number:"
@str_0 db 'Give me a number:', 0

; " "
@str_1 db ' ', 0

xseg ends
     end  main


xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _gorosort_0
     mov ax, 4C00h
     int 21h
main endp

;@1:	"unit , gorosort, - , -"
_gorosort_0 proc near
	push bp
	mov bp, sp
	sub sp, 24
;@2:	"par  , $1 , RET , -"
	lea si, word ptr [bp-12]
	push si
;@3:	"call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@4:	":=   , $1, - , t"
	mov ax, word ptr [bp-12]
	mov word ptr [bp-6], ax
;@5:	":=   , 1, - , case"
	mov ax, 1
	mov word ptr [bp-8], ax
;@6:	"<=   , case, t, (7, 35)"
@label6 :
	mov ax, word ptr [bp-8]
	mov cx, word ptr [bp-6]
	cmp ax, cx
	jle  @label7
	jmp @label35
;@7:	"par  , $2 , RET , -"
@label7 :
	lea si, word ptr [bp-14]
	push si
;@8:	"call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@9:	":=   , $2, - , n"
	mov ax, word ptr [bp-14]
	mov word ptr [bp-4], ax
;@10:	":=   , 0, - , inPlace"
	mov ax, 0
	mov word ptr [bp-2], ax
;@11:	":=   , 1, - , i"
	mov ax, 1
	mov word ptr [bp-10], ax
;@12:	"<=   , i, n, (13, 21)"
@label12 :
	mov ax, word ptr [bp-10]
	mov cx, word ptr [bp-4]
	cmp ax, cx
	jle  @label13
	jmp @label21
;@13:	"par  , $3 , RET , -"
@label13 :
	lea si, word ptr [bp-16]
	push si
;@14:	"call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@15:	"==   , i, $3, (16, 18)"
	mov ax, word ptr [bp-10]
	mov cx, word ptr [bp-16]
	cmp ax, cx
	je  @label16
	jmp @label18
;@16:	"+    , inPlace, 1, $4"
@label16 :
	mov ax, word ptr [bp-2]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-18], ax
;@17:	":=   , $4, - , inPlace"
	mov ax, word ptr [bp-18]
	mov word ptr [bp-2], ax
;@18:	"+    , i, 1, $5"
@label18 :
	mov ax, word ptr [bp-10]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-20], ax
;@19:	":=   , $5, - , i"
	mov ax, word ptr [bp-20]
	mov word ptr [bp-10], ax
;@20:	"jump , - , - , 12"
	jmp @label12
;@21:	"par  , "Case #" , R , -"
@label21 :
	lea si, byte ptr @str_1
	push si
;@22:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@23:	"par  , case , V , -"
	mov ax, word ptr [bp-8]
	push ax
;@24:	"call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@25:	"par  , ": " , R , -"
	lea si, byte ptr @str_2
	push si
;@26:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@27:	"-    , n, inPlace, $6"
	mov ax, word ptr [bp-4]
	mov cx, word ptr [bp-2]
	sub ax, cx
	mov word ptr [bp-22], ax
;@28:	"par  , $6 , V , -"
	mov ax, word ptr [bp-22]
	push ax
;@29:	"call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@30:	"par  , ".000000\n" , R , -"
	lea si, byte ptr @str_3
	push si
;@31:	"call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@32:	"+    , case, 1, $7"
	mov ax, word ptr [bp-8]
	mov cx, 1
	add ax, cx
	mov word ptr [bp-24], ax
;@33:	":=   , $7, - , case"
	mov ax, word ptr [bp-24]
	mov word ptr [bp-8], ax
;@34:	"jump , - , - , 6"
	jmp @label6
;@35:	"endu , gorosort, - , -"
@label35 :
@gorosort_0 :
	mov sp, bp
	pop bp
	ret
_gorosort_0 endp

extrn _readInteger : proc
extrn _writeString : proc
extrn _writeInteger : proc
; "Case #"
@str_1 db 'Case #', 0

; ": "
@str_2 db ': ', 0

; ".000000\n"
@str_3 db '.000000', 10, 0

xseg ends
     end  main


xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _prog2_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , prog2, - , -"
_prog2_0 proc near
	push bp
	mov bp, sp
	sub sp, 10
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
	lea si, word ptr [bp-8]
	push si
;@7: "call , - , - , readInteger"
	push bp
	call near ptr _readInteger
	add sp, 4
;@8: ":=   , $1, - , b"
	mov ax, word ptr [bp-8]
	mov word ptr [bp-4], ax
;@9: ":=   , 15, - , a"
	mov ax, 15
	mov word ptr [bp-2], ax
;@10: "par  , b , V , -"
	mov ax, word ptr [bp-4]
	push ax
;@11: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@12: "par  , " + 15 = " , R , -"
	lea si, byte ptr @str_2
	push si
;@13: "call , - , - , writeString"
	sub sp, 2
	push bp
	call near ptr _writeString
	add sp, 6
;@14: "+    , a, b, $2"
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp-4]
	add ax, cx
	mov word ptr [bp-10], ax
;@15: ":=   , $2, - , c"
	mov ax, word ptr [bp-10]
	mov word ptr [bp-6], ax
;@16: "par  , c , V , -"
	mov ax, word ptr [bp-6]
	push ax
;@17: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@18: "endu , prog2, - , -"
@prog2_0 :
	mov sp, bp
	pop bp
	ret
_prog2_0 endp

extrn _writeString : proc
extrn _readInteger : proc
extrn _writeInteger : proc
; "Give me a number:"
@str_0 db 'Give me a number:', 0

; " "
@str_1 db ' ', 0

; " + 15 = "
@str_2 db ' + 15 = ', 0

xseg ends
     end  main


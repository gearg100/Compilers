xseg segment public 'code'
     assume cs : xseg, ds : xseg, ss : xseg
     org 100h
main proc near
     call near ptr _prog1_0
     mov ax, 4C00h
     int 21h
main endp

;@1: "unit , prog1, - , -"
_prog1_0 proc near
	push bp
	mov bp, sp
	sub sp, 8
;@2: ":=   , 1, - , b"
	mov ax, 1
	mov word ptr [bp-4], ax
;@3: ":=   , 15, - , a"
	mov ax, 15
	mov word ptr [bp-2], ax
;@4: "+    , a, b, $1"
	mov ax, word ptr [bp-2]
	mov cx, word ptr [bp-4]
	add ax, cx
	mov word ptr [bp-8], ax
;@5: ":=   , $1, - , c"
	mov ax, word ptr [bp-8]
	mov word ptr [bp-6], ax
;@6: "par  , c , V , -"
	mov ax, word ptr [bp-6]
	push ax
;@7: "call , - , - , writeInteger"
	sub sp, 2
	push bp
	call near ptr _writeInteger
	add sp, 6
;@8: "endu , prog1, - , -"
@prog1_0 :
	mov sp, bp
	pop bp
	ret
_prog1_0 endp

extrn _writeInteger : proc
xseg ends
     end  main


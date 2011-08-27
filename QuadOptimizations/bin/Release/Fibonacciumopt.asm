xseg	segment	public 'code'
	assume	cs : xseg, ds : xseg, ss : xseg
	org	100h
main	proc	near
	call	near ptr _fibonacci_2
	mov	ax, 4C00h
	int	21h
main endp
_tailRecursive_1	proc	near
	push	bp
	mov	bp, sp
	sub	sp, 4
@1__tailRecursive_1:
	mov	ax, word ptr [bp+8]
	mov	si, word ptr [bp+4]
	mov	cx, word ptr [si-2]
	cmp	ax,cx
	jg	@2__tailRecursive_1
	jmp	@3__tailRecursive_1
@2__tailRecursive_1:
	jmp	@_tailRecursive_1
@3__tailRecursive_1:
	lea	si, byte ptr @str1
	push	si
	sub	sp, 2
	push	bp
	call	near ptr _writeString
	add	sp, 6
	mov	ax, word ptr [bp+8]
	push	ax
	sub	sp, 2
	push	bp
	call	near ptr _writeInteger
	add	sp, 6
	lea	si, byte ptr @str2
	push	si
	sub	sp, 2
	push	bp
	call	near ptr _writeString
	add	sp, 6
	mov	ax, word ptr [bp+10]
	push	ax
	sub	sp, 2
	push	bp
	call	near ptr _writeInteger
	add	sp, 6
	lea	si, byte ptr @str3
	push	si
	sub	sp, 2
	push	bp
	call	near ptr _writeString
	add	sp, 6
	mov	ax, word ptr [bp+12]
	mov	cx, word ptr [bp+10]
	add	ax, cx
	mov	word ptr [bp-2], ax
	mov	ax, word ptr [bp+8]
	mov	cx, 1
	add	ax, cx
	mov	word ptr [bp-4], ax
	mov	ax, word ptr [bp+10]
	push	ax
	mov	ax, word ptr [bp-2]
	push	ax
	mov	ax, word ptr [bp-4]
	push	ax
	sub	sp, 2
	mov	ax, word ptr [bp+4]
	push	ax
	call	near ptr _tailRecursive_1
	add	sp, 10
@4__tailRecursive_1:
@_tailRecursive_1:
	mov	sp, bp
	pop	bp
	ret
_tailRecursive_1	endp
_fibonacci_2	proc	near
	push	bp
	mov	bp, sp
	sub	sp, 4
@1__fibonacci_2:
	lea	si, byte ptr @str4
	push	si
	sub	sp, 2
	push	bp
	call	near ptr _writeString
	add	sp, 6
	lea	si, word ptr [bp-4]
	push	si
	sub	sp, 0
	push	bp
	call	near ptr _readInteger
	add	sp, 4
	mov	ax, word ptr [bp-4]
	mov	word ptr [bp-2], ax
	mov	ax, 0
	push	ax
	mov	ax, 1
	push	ax
	mov	ax, 1
	push	ax
	sub	sp, 2
	push	bp
	call	near ptr _tailRecursive_1
	add	sp, 10
@2__fibonacci_2:
@_fibonacci_2:
	mov	sp, bp
	pop	bp
	ret
_fibonacci_2	endp
@str1	db 'Fibonacci number '
	db 0
@str2	db ' is '
	db 0
@str3	db '.'
	db 10
	db 0
@str4	db 'Limit: '
	db 0
	extrn	_readInteger	: proc
	extrn	_writeInteger	: proc
	extrn	_writeString	: proc
xseg ends
	end  main

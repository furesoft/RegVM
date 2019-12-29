.meta version "1.0.0.0"
.meta name "HelloWorld"

.db msg "hello world from VM"
.db 0x2a
.db 0xC0FFEE

increment:
	inc $a
	call 0xC0FFEE
	int 0x123

load $a, 0x2a

jmp @increment
jmp @increment
jmp @increment
jmp @increment


int 0x6A03E
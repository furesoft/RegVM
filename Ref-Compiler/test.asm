﻿.meta version "1.0.0.0"
.meta name "HelloWorld"

.db msg "hello world from VM"
.db 0x2a
.db 0xC0FFEE

load $a, 0x2a
increment:
	inc $a

jmp @increment

int 0x123
int 0x6A03E
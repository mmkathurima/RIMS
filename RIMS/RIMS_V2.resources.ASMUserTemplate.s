.ent main
main:
.frame $sp,0,$ra
.set noreorder
.cpload $25
.set reorder
#implements B = A0 && !A1
BEGIN_WHILE:
lb $t0, A0 #need to load A0 into a register
lb $t1, A1 #need to load A1 into a register
and $t2, $t0, $t1 #and A0 with A1, store result in a register
sb $t2,B0 #store result of and in B0
END_WHILE:
j BEGIN_WHILE
EXITMAIN:
j $ra
.end main
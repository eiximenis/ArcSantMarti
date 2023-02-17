  ld b,0                         ; Initialising the partial sum
  ld a,($1000)                   ; Reading the first number into the accumulator
  add a,b                        ; Adding it to the sum
  ld b,a                         ; Writing the sum back to B
  ld a,($1001)                   ; Reading the second number into the accumulator
  add a,b                        ; Adding it to the sum
  ld b,a                         ; Writing the sum back to B
  ld a,($1002)                   ; Adding the 3rd value
  add a,b
  ld b,a
  ld a,($1003)                   ; Adding the 4th value
  add a,b
  ld b,a
  ld a,($1004)                   ; And adding the 5th value, too
  add a,b
  ld b,a
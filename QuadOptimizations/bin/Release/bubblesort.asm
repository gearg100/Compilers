BlockName 1
Starts at @1 and ends at @1
1:	unit , swap, - , -
BlockName 4
Starts at @2 and ends at @4
2:	:=   , x, - , t
3:	:=   , y, - , x
4:	:=   , t, - , y
BlockName 5
Starts at @5 and ends at @5
5:	endu , swap, - , -
BlockName 6
Starts at @6 and ends at @6
6:	unit , bsort, - , -
BlockName 7
Starts at @7 and ends at @7
7:	:=   , 121, - , changed
BlockName 9
Starts at @8 and ends at @9
8:	==   , changed, 121, 10
9:	jump , - , - , 46
BlockName 11
Starts at @10 and ends at @11
10:	:=   , 110, - , changed
11:	:=   , 0, - , i
BlockName 14
Starts at @12 and ends at @14
12:	-    , n, 1, $1
13:	<    , i, $1, 15
14:	jump , - , - , 45
BlockName 16
Starts at @15 and ends at @16
15:	==   , choice, 1, 17
16:	jump , - , - , 30
BlockName 21
Starts at @17 and ends at @21
17:	array, x, i, $2
18:	+    , i, 1, $3
19:	array, x, $3, $4
20:	>    , [$2], [$4], 22
21:	jump , - , - , 29
BlockName 26
Starts at @22 and ends at @26
22:	array, x, i, $5
23:	par  , [$5] , R , -
24:	+    , i, 1, $6
25:	array, x, $6, $7
26:	par  , [$7] , R , -
BlockName 29
Starts at @27 and ends at @29
27:	call , - , - , swap
28:	:=   , 121, - , changed
29:	jump , - , - , 42
BlockName 34
Starts at @30 and ends at @34
30:	array, x, i, $8
31:	+    , i, 1, $9
32:	array, x, $9, $10
33:	<    , [$8], [$10], 35
34:	jump , - , - , 42
BlockName 39
Starts at @35 and ends at @39
35:	array, x, i, $11
36:	par  , [$11] , R , -
37:	+    , i, 1, $12
38:	array, x, $12, $13
39:	par  , [$13] , R , -
BlockName 41
Starts at @40 and ends at @41
40:	call , - , - , swap
41:	:=   , 121, - , changed
BlockName 44
Starts at @42 and ends at @44
42:	+    , i, 1, $14
43:	:=   , $14, - , i
44:	jump , - , - , 12
BlockName 45
Starts at @45 and ends at @45
45:	jump , - , - , 8
BlockName 46
Starts at @46 and ends at @46
46:	endu , bsort, - , -
BlockName 47
Starts at @47 and ends at @47
47:	unit , writeArray, - , -
BlockName 48
Starts at @48 and ends at @48
48:	par  , msg , R , -
BlockName 50
Starts at @49 and ends at @50
49:	call , - , - , writeString
50:	:=   , 0, - , i
BlockName 52
Starts at @51 and ends at @52
51:	<    , i, n, 53
52:	jump , - , - , 63
BlockName 54
Starts at @53 and ends at @54
53:	>    , i, 0, 55
54:	jump , - , - , 57
BlockName 55
Starts at @55 and ends at @55
55:	par  , ", " , R , -
BlockName 56
Starts at @56 and ends at @56
56:	call , - , - , writeString
BlockName 58
Starts at @57 and ends at @58
57:	array, x, i, $15
58:	par  , [$15] , V , -
BlockName 62
Starts at @59 and ends at @62
59:	call , - , - , writeInteger
60:	+    , i, 1, $16
61:	:=   , $16, - , i
62:	jump , - , - , 51
BlockName 63
Starts at @63 and ends at @63
63:	par  , "\n" , R , -
BlockName 64
Starts at @64 and ends at @64
64:	call , - , - , writeString
BlockName 65
Starts at @65 and ends at @65
65:	endu , writeArray, - , -
BlockName 66
Starts at @66 and ends at @66
66:	unit , main, - , -
BlockName 67
Starts at @67 and ends at @67
67:	par  , "Seed :\t" , R , -
BlockName 69
Starts at @68 and ends at @69
68:	call , - , - , writeString
69:	par  , $17 , RET , -
BlockName 72
Starts at @70 and ends at @72
70:	call , - , - , readInteger
71:	:=   , $17, - , seed
72:	par  , "How do you want to sort?\n" , R , -
BlockName 74
Starts at @73 and ends at @74
73:	call , - , - , writeString
74:	par  , "How do you want to sort?\x0a" , R , -
BlockName 76
Starts at @75 and ends at @76
75:	call , - , - , writeString
76:	par  , "  1. Min to Max\n" , R , -
BlockName 78
Starts at @77 and ends at @78
77:	call , - , - , writeString
78:	par  , "  2. Max to Min\n" , R , -
BlockName 80
Starts at @79 and ends at @80
79:	call , - , - , writeString
80:	par  , "Choice : " , R , -
BlockName 82
Starts at @81 and ends at @82
81:	call , - , - , writeString
82:	par  , $18 , RET , -
BlockName 84
Starts at @83 and ends at @84
83:	call , - , - , readInteger
84:	:=   , $18, - , choice
BlockName 86
Starts at @85 and ends at @86
85:	<    , choice, 1, 89
86:	jump , - , - , 87
BlockName 88
Starts at @87 and ends at @88
87:	>    , choice, 2, 89
88:	jump , - , - , 101
BlockName 89
Starts at @89 and ends at @89
89:	par  , "How do you want to sort?\x0a" , R , -
BlockName 91
Starts at @90 and ends at @91
90:	call , - , - , writeString
91:	par  , "  1. Min to Max\n" , R , -
BlockName 93
Starts at @92 and ends at @93
92:	call , - , - , writeString
93:	par  , "  2. Max to Min\n" , R , -
BlockName 95
Starts at @94 and ends at @95
94:	call , - , - , writeString
95:	par  , "Choice : " , R , -
BlockName 97
Starts at @96 and ends at @97
96:	call , - , - , writeString
97:	par  , $19 , RET , -
BlockName 100
Starts at @98 and ends at @100
98:	call , - , - , readInteger
99:	:=   , $19, - , choice
100:	jump , - , - , 85
BlockName 101
Starts at @101 and ends at @101
101:	:=   , 0, - , i
BlockName 103
Starts at @102 and ends at @103
102:	<    , i, 16, 104
103:	jump , - , - , 114
BlockName 113
Starts at @104 and ends at @113
104:	*    , seed, 137, $20
105:	+    , $20, 220, $21
106:	+    , $21, i, $22
107:	%    , $22, 101, $23
108:	:=   , $23, - , seed
109:	array, x, i, $24
110:	:=   , seed, - , [$24]
111:	+    , i, 1, $25
112:	:=   , $25, - , i
113:	jump , - , - , 102
BlockName 116
Starts at @114 and ends at @116
114:	par  , "Initial array: " , R , -
115:	par  , 16 , V , -
116:	par  , x , R , -
BlockName 120
Starts at @117 and ends at @120
117:	call , - , - , writeArray
118:	par  , 16 , V , -
119:	par  , choice , V , -
120:	par  , x , R , -
BlockName 124
Starts at @121 and ends at @124
121:	call , - , - , bsort
122:	par  , "Sorted array: " , R , -
123:	par  , 16 , V , -
124:	par  , x , R , -
BlockName 125
Starts at @125 and ends at @125
125:	call , - , - , writeArray
BlockName 126
Starts at @126 and ends at @126
126:	endu , main, - , -

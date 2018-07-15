def grey(<value>) as rgb(value, value, value);
def yellow(<wert>, <wertA>, <wertB>, <wertC>) as print('lol');
def green(<wert>, <wertA>, 'Hallo', <wertB>, <wertC>) as noPattern();
def gray(>v) as rgb(v, v, v);
def yallow(>w, >A, >B, >C) as print('lol');
def graan(>w, >A, 'Hallo', >B, >C) as noPattern();

func foo():
	grey(57);
	yellow('Hy', 6.7, 99, 0);
	green('Why', 7.6, 'Hallo', 99, 0);
	green('Why', 7.6, 'Hello', 99, 0);
	gray(957);
	yallow('9Hy', 96.7, 999, 90);
	graan('9Why', 97.6, 'Hallo', 999, 90);
	graan('9Why', 97.6, 'Hello', 999, 90);
endfunc

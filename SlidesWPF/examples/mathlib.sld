{
lib math:
    func();
endlib

const pi: 3.14;

//is a bad example.. better use c# code
func sin(x):
    ret 0.3 * x;
endfunc

func abs(x)
    if(x < 0)
        ret x* -1;
    else
        ret x;
    endif
endfunc
}
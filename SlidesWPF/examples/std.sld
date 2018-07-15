{
import font('Arial', 'Times New Roman') as arial;
import lib('libexample.cs');
import lib(math) as m;

//<> defines Variable; Won't replace <Value>
def grey(<value>) as rgb(value, value, value);
//> may be used if Variable is only one char big
def alphagrey(>v, >a) as rgba(v, v, v, a);
def plainGray as grey(90);
const plainGrey: rgb(90, 90, 90);

//Would replace Anything in text
//i.e. 'Hi you, i think you're pretty' becomes
//'Hello World, i think you're pretty'
def @'Hi you' as 'Hello World';
//Would replace just 'Hi you'
def 'Hi you' as 'Hello World';
//Redefining Keywords currently not supported
//def keyword::next as step;

style std:
    background: plainGrey;
    //background: img('Path/To/Image.png');
    foreground: black;
    font: arial;
endstyle

//Slides
slide 'yourslide':
    pattern(std);
    title.text = 'Hello World!';
endslide

slide 'yoursecondslide':
    pattern(pic);
endslide

slide 'funnycharts':
    chart: new PieChart();
    csv: loadCSV('path');
    j: for(0 to len(csv))
        chart.add(csv[j][0], csv[j][1]);
    endfor
    c: foreach(csv)
        chart.add(c[0], c[1], c[3]) //Name, percentage, HexColor
    endforeach
endslide

func foo():
    arr: [2];       //Array
    larr: [];       //Liste/Unbegrenztes Array
    drr: [5][7];    //Zweidimensionales Array
    lia: [3][];     //Listen in Array
    ail: [][3];     //Arrays in Liste
    arr[0]: m.pi;
    larr.add(m.pi);
    drr[0][0]: m.pi;
    lia[0].add(m.pi);
    ail.add();
    ail[0][0]: m.pi;
endfunc

}
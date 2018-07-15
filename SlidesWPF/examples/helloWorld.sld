import font('Arial', 'non-serif') as arial;
import lib('snowflakes') as snow;
import lib('example') as trans;

style std:
    background: rgb(90,90,90);
endstyle

style addBigRedBorder():
    this.border: red;
    this.borderThickness: 20px;
endstyle

//style test():
//    c.apply(this);
//endstyle

transition std:
    oldSlide.fadeOut(200); //awaitFadeOut(200)?
    newSlide.fadeIn(200);
endtransition


func moveAway(direction, rotate):
    dir: new Vector(0, 0);
    switch(direction):
        case Direction.Up:
            dir.Y: -1;
        case Direction.Right:
            dir.X: 1;
        case Direction.Down:
            dir.Y: 1;
        case Direciton.Left:
            dir.X: -1;
    endswitch
    this.pos: this.pos + dir;
endfunc

pattern std:
    title: new Title('Standard-Title');
    title.orientation: Horizontal.Center;
    title.orientation: Vertical.Top;
endpattern

pattern bigImage:
    image: new Image('...');
    center(image);
    lblSource: new Label('(c) Standard-Source');
    lblSource.orientation: Horizontal.Center;
    lblSource.orientation: Vertical.Bottom;
endpattern

slide 'Hello World':
    pattern(std);
    intro(fight); //transition???
    
    stack: new Stack('v');
    title: new Title('Hello World o/');
    addBigRedBorder(title);
    title.addSubtitle('Just Testing __Slides__');
    title.colorTitle: rgb(230, 230, 230);
    title.colorSubtitle: rgb(200, 200, 200);
    stack.add(title);
    stack.add(subTitle);
    center(stack);
    center(stack.content)
endslide //optional

slide 'pic':
    pattern(bigImage);
    transition(fight);
endslide

slide 'Lists':
    noPattern();
    pattern(null);
    stack: new Stack('v');
    stack.add(new Title('List-Title:'));
    stack.add(new Label('- Reasons why nothing works'));
    stack.add(new Label('- Because we don\'t use Lists right!'));
    leftHalf(stack);
    margin(stack, 10px);

    

    loop(10fps, 5s): background: hsv(time, 1,1); 
endslide

//Todo: Implement
slide 'betterLists':
    noPattern();
    ulist: new List(Type.Unordered);
    ulist.symbol: '-';
    goods: [4];
    goods[0]: 'Apples';     
    goods[1]: 'Bananas';
    goods[2]: 'Tomatoes';
    goods[3]: 'Cucumber';
    ulist.fill(goods);
    // - Apples
    // - Bananas
    // - Tomatoes
    // - Cucumber

    olist: new List(Type.Ordered);
    olist.symbol: '1.';
    olist.fill(goods);
    // 1. Apples
    // 2. Bananas
    // 3. Tomatoes
    // 4. Cucumber
    olist.noSpace: true; //Default: false
    // 1.Apples
    // 2.Bananas
    // 3.Tomatoes
    // 4.Cucumber

    slist: new List(Type.Ordered);
    slist.symbol: '1.a';
    goods[1]: '\tBananas';
    goods[2]: '\tTomatoes';
    slist.fill(goods);
    // 1. Apples
    // 1.a Bananas
    // 1.b Tomatoes
    // 2. Cucumber
    slist.intend: true; //What should be default?
    // 1. Apples
    //  1.a Bananas
    //  1.b Tomatoes
    // 2. Cucumber
    
endslide

slide 'Stuff':
    background: image(@'.\nicelittlekittens.jpg');

    snow.letItSnow();
endslide

slide 'Videos':
    video: youtube('tSM2cJvw12w');
    gif: giphy('l4pT4huXptkHYJQE8');
    web: iframe('url');
    mp3: music(@'.\lol.mp3');
    mp3.play();
    video.play();
    pic: svg(@'.\demo.svg');
    tweet: twitter('971637246339309569');
    profile: twitter('@der_Wert007');   
    rightBottomQuarter(pic);
    margin(pic, 5%);

endslide

slide 'Hello':
    hide(); //invisible();

    title: new Title('Helloooo');
endslide

slide 'funcs':
    invisible();
    j: for(0 to 10 in 1):
        text: new Label(j + '');
    endfor
    if(name == 'funcs'):
        r: rect(50, 60);
        r.fill: red;
    endif
    #pos
    if(rand(20) == 0):
        goto pos;
    endif
    while(slideTime < 90h):
        //doSomethingRandom()
    endwhile
    nextSlide();
endslide

slide 'Clusterfuck':
    subSlide: slide('Hello');
    leftTopQuarter(subSlide);
endslide

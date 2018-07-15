import font('Arial', 'Times New Roman', 'serif') as arial;

//This specific Libraryfile may declare styles
//and patterns. But functions shall not 
//be declared!
lib example:
    style();
    pattern();
    transition();
    importLibs();   //this lib must allow at least 
                    //as much as imported lib
    //noFunc();
endlib


style std:
    background: white;
    //background: img('Path/To/Image.png');
    foreground: black;
    font: arial;
endstyle

//Patterns
pattern std:
    title: new Title('Insert Title');
    text: new Label('Insert Text');
    bottomMargin(title, 90%);
    topMargin(text, 10%);
endpattern

pattern pic:
    picture: new Image('Insert image');
    center(picture)
endpattern

//Transitions
transition fight:
    fight: new Label('FIGHT!');
    fight.size: 80%;
    oldSlide.add(fight); //Better way?
    //like using a special canvas just for items 
    //of transition? Would mean, that it stays as 
    //long as the transition itself
    //And Transition becomes like a "Zwischenslide"
    wait(500);
    oldSlide.hide();
    newSlide.show();
endtransition
}
import font('Comic Sans MS') as sans;

style std:
	background: rgb(51, 51, 51);
	foreground: rgb(200, 200, 200);
	label.fontsize: 20px;
	//TODO: label.font: arial;
	//padding(slide, 3%);
endstyle

pattern std:
    title: new Title('Standard-Title');
    title.orientation: Horizontal.Right;
    title.orientation: Vertical.Bottom;
endpattern

//TODO? Show when a variable is instantiated. Like some strange symbol in static methods, since you can't see the new Blabla() there.
//Differentiate between UI and data? MVC?

slide 'Test':
	pattern(std);
	
	img: image('C:\Users\Wert007\Desktop\lastreets.jpg');
	background: img;

	title.text: 'Hello World';
	title.orientation: Horizontal.Right;
	title.addSubtitle('A little Introduction to the modern fairytale na'); //...mely IT and friends
	title.colorTitle: rgb(230, 230, 230);
	title.colorSubtitle: rgb(200, 200, 200);
	title.fontsizeTitle(90);
	title.fontsizeSubtitle(25);

	step:
		frame: new Image(img);
		frame.scale: Scale.Fit;
		rightTopQuarter(frame);
		center(frame);
		margin(frame, 5%);

        vid: youtube('lA_S-FX2v44');
        leftHalf(vid);
endslide



slide 'Hey':
	background: rgb(51, 51, 99);
	title: new Label('Hey');
	title.font: sans;
	topHalf(title);
	title.orientation: Vertical.Top;
	title.orientation: Horizontal.Right;

	step 'img':
		img: image('C:\Users\Wert007\Desktop\kitten.jpg');
		frame: new Image(img);
		frame.scale: Scale.Fit;
		margin(frame, 20%, 0px, 0px, 0px);
endslide
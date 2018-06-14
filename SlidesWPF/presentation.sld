import font('Comic Sans MS') as sans;

style std:
	background: rgb(51, 51, 51);
	foreground: rgb(200, 200, 200);
	label.fontsize: 30px;
	//TODO: label.font: arial;
	padding(slide, 3%);
endstyle

style done():
	foreground: rgb(0, 255, 0);
	fontsize: 25px;
endstyle

pattern std:
    title: new Title('Standard-Title');
    title.orientation: Horizontal.Right;
    title.orientation: Vertical.Bottom;
endpattern

slide 'dev':
	todo: new Stack('v');
	todo.add(new Label('- Hotloading')).done();
	todo.add(new Label('- variable creating commands... and the others')).done();
	todo.add(new Label('- Transition'));
	todo.add(new Label('- Animations'));
	todo.add(new Label('- List-Control'));
	todo.add(new Label('- Fonts in styles'));
	todo.add(new Label('- Commandsrefactoring'));
	todo.add(new Label('- GoogleFonts'));
	todo.add(new Label('- Compilerrefactoring'));
	todo.add(new Label('- svg support'));
	todo.add(new Label('- time support'));
	todo.add(new Label('- ODP-Support'));
	todo.foreground: rgb(255, 0, 0);
	center(todo);
endslide

slide 'Test':
	pattern(std);
	
	img: image('C:\Users\Wert007\Desktop\lastreets.jpg');
	background: img;

	title.text: 'Hello World';
	title.orientation: Horizontal.Right;
	title.addSubtitle('A little Introduction to the modern fairytale namely IT and friends');
	title.colorTitle: rgb(230, 230, 230);
	title.colorSubtitle: rgb(200, 200, 200);
	title.fontsizeTitle(90);
	title.fontsizeSubtitle(25);

	stack: new Stack('v');
	leftHalf(stack);
	loop(2fps): stack.add(new Label('Hey'));

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
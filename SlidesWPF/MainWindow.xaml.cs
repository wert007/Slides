using Slides;
using Slides.Interactives.Types;
using SlidesToReact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SlidesWPF
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		CodeReader reader;
		public MainWindow()
		{
			InitializeComponent();

			reader = CodeReader.FromFile(@".\examples\deftest.sld");
			reader.FileChanged += () => { Create(); };
			Create();
		}

		public void Create()
		{
			var presentation = Parser.Parse(reader);
			presentation.Run();
			grid.InvalidateMeasure();
			grid.Children.Add(new PresentationWPF(presentation));
			var react = new PresentationReact(presentation, @"C:\Users\Wert007\Desktop\reactslides\presentation.html");
			react.Parse();
		}
	}
}

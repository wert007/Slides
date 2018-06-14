using DocumentFormat.OpenXml.Presentation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

namespace WpfTest
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string path;
		public MainWindow()
		{
			InitializeComponent();

			Presentation p = new Presentation();
			Slide s = new Slide();
			s.Transition = new Transition();
			s.Transition.
		}

		private void BtnLoad_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if(ofd.ShowDialog() == true)
			{
				path = ofd.FileName;
			}

			LoadText();
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			if(sfd.ShowDialog() == true)
			{
				using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
				using (StreamWriter sw = new StreamWriter(fs))
				{
					sw.Write(txtBoxEdit.Text);
				}
			}
		}

		private void BtnUnzip_Click(object sender, RoutedEventArgs e)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open))
			using (GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress))
			using (StreamReader sr = new StreamReader(gzip))
			{
				txtBoxEdit.Text = sr.ReadToEnd();
			}
		
		}

		private void BtnZip_Click(object sender, RoutedEventArgs e)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open))
			using (GZipStream gzip = new GZipStream(fs, CompressionMode.Compress))
			using (StreamReader sr = new StreamReader(gzip))
			{
				txtBoxEdit.Text = sr.ReadToEnd();
			}
		}

		public void LoadText()
		{
			if (path == null)
				return;
			using (FileStream fs = new FileStream(path, FileMode.Open))
			using (StreamReader sr = new StreamReader(fs))
			{
				txtBoxOrig.Text = sr.ReadToEnd();
			}
		}

	}
}

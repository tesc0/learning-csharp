/*
 * Created by SharpDevelop.
 * User: Zsolt
 * Date: 2017.04.14.
 * Time: 15:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace hazi1
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		List<string> origNames = new List<string>(); //eredeti név lista
		int linenumber = 0;
		string[] namesToSort;
		string[] names;
		string[] namesCSorted;
		Stopwatch stopwatch_bubble;
		Stopwatch stopwatch_sorting;
		string[] sortedNames1; //buborékosan rendezett tömb
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//string[] sortedNames1; //rendezett nevek - string tömb
			//string[] sortedNames2; //rendezett tömb - csharp függvénnyel
			//string[] namesTemp; //ideiglenes tartalom
			int namelength = origNames.Count;
			names = new string[linenumber];
			//string[] names = new string[linenumber]; //feltöltött nevek - string tömb
			
			//nevek feltöltése
			if(origNames.Count > 0) {
				for(int j = 0; j < origNames.Count; j++) {				
					names[j] = origNames[j];
				}
			
			
				//kiírás
				this.label2.Text = "Eredeti:" + Environment.NewLine;
				for(int i = 0; i < names.Length; i++) {
					this.label2.Text +=	names[i] + Environment.NewLine;
				}
			
			
				
				//beépített rendezés
				//egyeztetéshez
				namesToSort = names;
				this.button1.Enabled = false;
				this.button2.Enabled = false;
				this.button3.Enabled = false;
				
				//első szálon futó csharp rendezés
				backgroundWorker_rendezes.RunWorkerAsync();			
		
				//második szálon futó buborék rendezés
				Array.Sort(namesToSort);	
				//összehasonlításhoz
				namesCSorted = namesToSort;	
				backgroundWorker_buborek.RunWorkerAsync();
				
			
			} else {
				label3.Text = "Nincsenek nevek!";
			}
		}
		
		public static string[] sortNames(string[] names) {
			
			
			string temp;
			
			for(int i = 0; i < names.Length - 1; i++) {
				//System.Diagnostics.Debug.WriteLine(i);
				if (string.Compare(names[i], names[i + 1]) > 0) //if first number is greater then second then swap
		        {
		            //csere
		            temp = names[i];
		            names[i] = names[i + 1];
		            names[i + 1] = temp;
				} else {
					//ha nem cserélt
					//System.Diagnostics.Debug.WriteLine(names[i] + "--" + names[i+1]);
				}
			}
			
			return names;
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			// fájlablak tulajdonságai és a választott fájl feldolgozása 
			openFileDialog1.FileName = "";
			openFileDialog1.Filter = "TXT files|*.txt";
			//openFileDialog1.InitialDirectory = "C:\\Users\\Public\\Documents";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
            	string filename = openFileDialog1.FileName;
        		foreach (string line in File.ReadLines(@filename))
				{
				    origNames.Add(line);
        			//System.Diagnostics.Debug.WriteLine(line);
        			linenumber++; //sorok száma
				}
        		label3.Text = "Nevek betöltve.";
        		label3.ForeColor = Color.Black;
        		
            }
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			
			//System.Diagnostics.Debug.WriteLine(string.Compare("szabó gyula", "horvát anna"));
			//return;
			
			if (textBox1.Text.Equals(""))
    		{
				//legyen tartalom
        		label3.Text = "Add meg a fájlnevet!";
			} else {
				label3.Text = "";
				// Example #1: Write an array of strings to a file.
		        // Create a string array that consists of three lines.
		        string[] lines = { "kat ilona", "szabó gyula", "kis mária", "3nagy ernő", "kovács géza", "horvát anna", "jenőcsik zoltán" };
		        // WriteAllLines creates a file, writes a collection of strings to the file,
		        // and then closes the file.  You do NOT need to call Flush() or Close().
		        System.IO.File.WriteAllLines(@"C:\Users\Public\Documents\" + textBox1.Text + ".txt", lines);
		        label3.Text = "Fájl mentve";
		        label3.ForeColor = Color.Black;
			}
		}
		void Label3Click(object sender, EventArgs e)
		{
	
		}
		void OpenFileDialog1FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
	
		}
		void TextBox1TextChanged(object sender, EventArgs e)
		{
	
		}
		
		void BackgroundWorker_buborekDoWork(object sender, DoWorkEventArgs e)
		{
			
			
			BackgroundWorker helperBW = sender as BackgroundWorker;
		   
		    e.Result = bubbleSorting(helperBW);
		    if (helperBW.CancellationPending)
		    {
		        e.Cancel = true;
		    }
		}
		
		void BackgroundWorker_rendezesDoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker helperBW = sender as BackgroundWorker;
		   
		    e.Result = csharpSorting(helperBW, namesToSort);
		    if (helperBW.CancellationPending)
		    {
		        e.Cancel = true;
		    }
		}
		
		string[] csharpSorting(BackgroundWorker bw, string[] namesArray) {
			
			//futási idő mérése
			stopwatch_sorting = Stopwatch.StartNew();
			//rendezés
			Array.Sort(namesArray);	
			stopwatch_sorting.Stop();
			
			//összehasonlításhoz
			namesCSorted = namesArray;		
			return namesArray;
		}
		
		string[] bubbleSorting(BackgroundWorker bw) {
					    
			//futási idő mérése
			stopwatch_bubble = Stopwatch.StartNew();
			//rendezés
			sortedNames1 = sortNames(names);			
			while(!Enumerable.SequenceEqual(sortedNames1, namesCSorted)) {
				sortedNames1 = sortNames(names);
			}
			stopwatch_bubble.Stop();
			return sortedNames1;
		}
		
		private void BackgroundWorker_buborekRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			/*
		    if (e.Cancelled) MessageBox.Show("Operation was canceled");
		    else if (e.Error != null) MessageBox.Show(e.Error.Message);
		    else MessageBox.Show(e.Result.ToString());
		    */
		    
		    if (e.Error != null) {
				
			} else {
			    //buborékos kiírás
				this.label1.Text = "Buborékosan rendezett (2. szál):" + Environment.NewLine;
				this.label1.Text += "Futási idő: " + stopwatch_bubble.ElapsedMilliseconds + " ms" + Environment.NewLine;
				for(int i = 0; i < sortedNames1.Length; i++) {
					this.label1.Text +=	sortedNames1[i] + Environment.NewLine;
				}
				
				if(!this.button1.Enabled) {
					this.button1.Enabled = true;
					this.button2.Enabled = true;
					this.button3.Enabled = true;
				}
		    }
		}  
		
		private void BackgroundWorker_rendezesRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			/*
		    if (e.Cancelled) MessageBox.Show("Operation was canceled");
		    else if (e.Error != null) MessageBox.Show(e.Error.Message);
		    else MessageBox.Show(e.Result.ToString());
		    */
		   
		   System.Diagnostics.Debug.WriteLine("eee");
		   if (e.Error != null) {
		   	
		   	} else {
				//csharp rendezés kiírása
				this.label4.Text = "C# array.sort rendezés (1. szál):" + Environment.NewLine;
				this.label4.Text += "Futási idő: " + stopwatch_sorting.ElapsedMilliseconds + " ms" + Environment.NewLine;
				for(int i = 0; i < namesCSorted.Length; i++) {
					this.label4.Text +=	namesCSorted[i] + Environment.NewLine;
				}
				
				if(!this.button1.Enabled) {
					this.button1.Enabled = true;
					this.button2.Enabled = true;
					this.button3.Enabled = true;
				}
		   }
		    
		    
		}
		
		 
	}
}

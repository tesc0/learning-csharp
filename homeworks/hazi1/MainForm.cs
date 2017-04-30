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

namespace hazi1
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		List<string> origNames = new List<string>(); //eredeti név lista
		int linenumber = 0;
		
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
			string[] sortedNames1; //rendezett nevek - string tömb
			string[] sortedNames2; //rendezett tömb - csharp függvénnyel
			string[] namesTemp; //ideiglenes tartalom
			int namelength = origNames.Count;
			string[] names = new string[linenumber]; //feltöltött nevek - string tömb
			
			//nevek feltöltése
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
			namesTemp = names;
			Array.Sort(namesTemp);
			sortedNames2 = namesTemp;
			
			//rendezés
			sortedNames1 = sortNames(names);			
			while(!Enumerable.SequenceEqual(sortedNames1, sortedNames2)) {
				sortedNames1 = sortNames(names);
			}
			
			
			
			//kiírás
			this.label1.Text = "Buborékosan rendezett:" + Environment.NewLine;
			for(int i = 0; i < sortedNames1.Length; i++) {
				this.label1.Text +=	sortedNames1[i] + Environment.NewLine;
			}
			
			
			//csharp rendezés kiírása
			this.label4.Text = "C# array.sort rendezés:" + Environment.NewLine;
			for(int i = 0; i < sortedNames2.Length; i++) {
				this.label4.Text +=	sortedNames2[i] + Environment.NewLine;
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
		
		 
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASTHierarchy;
using GeoObjects;
using System.Windows.Forms;
using Compiling;
namespace GeoWallE
{
    public class ApplicationManager : IApplicationManager
    {
        Scanner scanner;
        Painter painter;
        Form1 formApp;
        public ApplicationManager(Form1 formApp)
        {
            scanner = new Scanner();
            painter = new Painter(formApp.canvas1);
            this.formApp = formApp;
        }

        public bool Paint(GeoObject shape, string color, string label)
        {
           return  painter.Paint(shape, color, label);
        }

        public void Scan(Type TypeToScan, out GeoObject ToScan, string label)
        {
            scanner.Scan(TypeToScan, out ToScan,label);
        }

        public void ThrowException(CodeLocation location,string Message)
        {
            string exception = "Exception in line: " + location.Line.ToString() + " " + Message; 
            MessageBox.Show(exception,"Exception" ,MessageBoxButtons.OK, MessageBoxIcon.Stop);
            formApp.Close();
        }

       
    }
}

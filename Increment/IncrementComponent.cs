using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Rhino.Display;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using GH_IO;
using GH_IO.Serialization;

namespace Increment
{
    public class IncrementComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public IncrementComponent()
          : base("Increment", "++",
            "Increments interger values",
            "Params", "Util")
        {
        }

        //This region overrides the typical component layout
        public override void CreateAttributes()
        {
            m_attributes = new CustomUI.ButtonUIAttributes(this, handler);
        }
        public static int oldStart = 0;
        public static int increment = 1;
        public static int currentValue;
        public static bool recomputeFlag = false;
        public static GH_Document doc;

        public delegate void Function(int i);
        public static void DelegateMethod(int i)
        {
            switch (i)
            {
                case 0:
                    currentValue += increment;
                    //Rhino.RhinoApp.WriteLine("currentValue = {0}", currentValue);
                    break;
                case 1:
                    currentValue -= increment;
                    break;
                case 2:
                    currentValue = oldStart;
                    break;
            }
            recomputeFlag = true;
        }
        Function handler = DelegateMethod;



        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Beginning Number", "B", "Start point for incrementing", GH_ParamAccess.item, 0);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("Step Size", "S", "Step Size", GH_ParamAccess.item, 1);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Output", "O", "Current value of incrementor", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int startIn = 0;
            DA.GetData(0, ref startIn);
            DA.GetData(1, ref increment);
            doc = OnPingDocument();
            if (startIn != oldStart)
            {
                currentValue = startIn;
                oldStart = startIn;
            }

            if (doc != null)

                doc.ScheduleSolution(1, ScheduleCallback);

            RhinoApp.WriteLine("currentValue = {0}", currentValue.ToString());

            DA.SetData(0, currentValue);

            //this.ExpireSolution(false);

        }

        public void ScheduleCallback(GH_Document document)
        {
            doc.ScheduleSolution(1, ScheduleCallback);
            if (recomputeFlag)
            {
                recomputeFlag = false;
                this.ExpireSolution(false);
            }
        }
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                return Increment.Properties.Resources.icon;
                //return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("2B200D7D-5168-4532-A099-76DC9263F304");
    }
}
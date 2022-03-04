using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Increment
{
    public class IncrementInfo : GH_AssemblyInfo
    {
        public override string Name => "Increment";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("6E1178F1-BBB7-4A11-B2DB-C4FE64FC205C");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}
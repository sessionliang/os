using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BaiRong.Controls
{
	public class Skin : BaiRong.Controls.TemplatedWebControl
	{
        protected override string SkinFolder
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string SkinPath
        {
            get
            {
                return this.Path;
            }
        }

        protected override void AttachChildControls()
        {
            
        }

        public virtual String Path
        {
            get
            {
                string state = (string)ViewState["Path"];
                return state;
            }
            set
            {
                ViewState["Path"] = value;
            }
        }
	}

}

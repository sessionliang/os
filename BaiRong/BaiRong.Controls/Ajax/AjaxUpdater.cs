using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;

namespace BaiRong.Controls
{
	public class AjaxUpdater : Control
	{
        public AjaxUpdater()
		{
		}

		public string File
		{
			get 
			{
                Object state = ViewState["File"];
				if ( state != null ) 
				{
					return (string)state;
				}
				return string.Empty;
			}
			set 
			{
                ViewState["File"] = value;
			}
		}

        public string For
        {
            get
            {
                Object state = ViewState["For"];
                if (state != null)
                {
                    return (string)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["For"] = value;
            }
        }

        public string Updater
        {
            get
            {
                Object state = ViewState["Updater"];
                if (state != null)
                {
                    return (string)state;
                }
                return "window";
            }
            set
            {
                ViewState["Updater"] = value;
            }
        }

        public string Event
        {
            get
            {
                Object state = ViewState["Event"];
                if (state != null)
                {
                    return (string)state;
                }
                return "load";
            }
            set
            {
                ViewState["Event"] = value;
            }
        }

        //public bool RegisterScript
        //{
        //    get
        //    {
        //        Object state = ViewState["RegisterScript"];
        //        if (state != null)
        //        {
        //            return (bool)state;
        //        }
        //        return true;
        //    }
        //    set
        //    {
        //        ViewState["RegisterScript"] = value;
        //    }
        //}

		protected override void Render(HtmlTextWriter writer)
		{
            if (!string.IsNullOrEmpty(this.File) && !string.IsNullOrEmpty(this.For))
            {
                //if (this.RegisterScript)
                //{
                //    JsUtils.RegisterPrototype(base.Page);
                //}

                int updaterID = StringUtils.GetRandomInt(1, 10000);
                string ajaxUrl = PageUtils.ParseNavigationUrl(this.File);
                string script = string.Format(@"
	<script type=""text/javascript"" language=""javascript"">		
		function bairong_updater_{0}() {{
			var url = ""{1}"";
			var option = {{
				method:'get',
				onSuccess:function(){{

				}},
				onFailure:function(){{
					$('{2}').innerHTML = ""Õ¯¬Á∑±√¶£¨«Î…‘∫Û‘Ÿ ‘...."";
				}}
			}};
            $('{2}').style.display = """";
			new Ajax.Updater ({{success:'{2}'}}, url, option);
		}}
	</script>
	<script type=""text/javascript"" language=""javascript"">
		Event.observe({3}, '{4}', bairong_updater_{0}, false);
	</script>", updaterID, ajaxUrl, this.For, this.Updater, this.Event);
                base.Page.RegisterStartupScript(string.Format("BaiRong.Controls.Updater_{0}_{1}", this.For, this.Event), script);
            }
		}

	}
}

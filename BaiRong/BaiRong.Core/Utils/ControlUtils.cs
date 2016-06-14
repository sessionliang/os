using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BaiRong.Model;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Web;
using BaiRong.Core.Web.Controls;
using BaiRong.Core;
using System.Collections.Generic;

namespace BaiRong.Core
{
	/// <summary>
	/// �Կؼ��İ�����
	/// </summary>
	public class ControlUtils
	{
		private ControlUtils()
		{

		}

		/// <summary>
		/// �õ�����ؼ���HTML����
		/// </summary>
		/// <param name="control">�ؼ�</param>
		/// <returns></returns>
		public static string GetControlRenderHtml(System.Web.UI.Control control)
		{
			StringBuilder builder = new StringBuilder();
            if (control != null)
            {
                System.IO.StringWriter sw = new System.IO.StringWriter(builder);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                control.RenderControl(htw);
            }
			return builder.ToString();
		}

        public static string GetControlRenderHtml(System.Web.UI.Control control, Page page)
        {
            StringBuilder builder = new StringBuilder();
            if (control != null)
            {
                control.Page = page;
                control.DataBind();
                System.IO.StringWriter sw = new System.IO.StringWriter(builder);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                control.RenderControl(htw);
            }
            return builder.ToString();
        }

		/// <summary>
		/// ����˿ؼ������ڴ����ԣ���������ӵ��ؼ���
		/// </summary>
		/// <param name="accessor">�ؼ�</param>
		/// <param name="attributes">���Լ���</param>
		public static void AddAttributesIfNotExists(IAttributeAccessor accessor, StringDictionary attributes)
		{
			if (accessor != null && attributes != null)
			{
				foreach (string key in attributes.Keys)
				{
					if (accessor.GetAttribute(key) == null)
					{
						accessor.SetAttribute(key, attributes[key]);
					}
				}
			}
		}

		/// <summary>
		/// ����˿ؼ������ڴ����ԣ���������ӵ��ؼ���
		/// </summary>
		/// <param name="accessor"></param>
		/// <param name="attributeName"></param>
		/// <param name="attributeValue"></param>
		public static void AddAttributeIfNotExists(IAttributeAccessor accessor, string attributeName, string attributeValue)
		{
			if (accessor != null && attributeName != null)
			{
				if (accessor.GetAttribute(attributeName) == null)
				{
					accessor.SetAttribute(attributeName, attributeValue);
				}
			}
		}


		/// <summary>
		/// ��������ӵ��ؼ���
		/// </summary>
		/// <param name="accessor">�ؼ�</param>
		/// <param name="attributes">���Լ���</param>
		public static void AddAttributes(IAttributeAccessor accessor, StringDictionary attributes)
		{
			if (accessor != null && attributes != null)
			{
				foreach (string key in attributes.Keys)
				{
					accessor.SetAttribute(key, attributes[key]);
				}
			}
		}

		/// <summary>
		/// ��������ӵ��ؼ���
		/// </summary>
		/// <param name="accessor"></param>
		/// <param name="attributeName"></param>
		/// <param name="attributeValue"></param>
		public static void AddAttribute(IAttributeAccessor accessor, string attributeName, string attributeValue)
		{
			if (accessor != null && attributeName != null)
			{
				accessor.SetAttribute(attributeName, attributeValue);
			}
		}

        public static void AddListControlItems(ListControl listControl, List<string> list)
        {
            if (listControl != null)
            {
                foreach (string value in list)
                {
                    ListItem item = new ListItem(value, value);
                    listControl.Items.Add(item);
                }
            }
        }


		public static string[] GetSelectedListControlValueArray(ListControl listControl)
		{
			ArrayList arraylist = new ArrayList();
			if (listControl != null)
			{
				foreach (ListItem item in listControl.Items)
				{
					if (item.Selected)
					{
						arraylist.Add(item.Value);
					}
				}
			}
			string[] retval = new string[arraylist.Count];
			arraylist.CopyTo(retval);
			return retval;
		}

        public static string GetSelectedListControlValueCollection(ListControl listControl)
        {
            ArrayList arraylist = new ArrayList();
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    if (item.Selected)
                    {
                        arraylist.Add(item.Value);
                    }
                }
            }
            return TranslateUtils.ObjectCollectionToString(arraylist);
        }

        public static ArrayList GetSelectedListControlValueArrayList(ListControl listControl)
        {
            ArrayList arraylist = new ArrayList();
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    if (item.Selected)
                    {
                        arraylist.Add(item.Value);
                    }
                }
            }
            return arraylist;
        }

        public static ArrayList GetSelectedListControlValueIntArrayList(ListControl listControl)
        {
            ArrayList arraylist = new ArrayList();
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    if (item.Selected)
                    {
                        arraylist.Add(TranslateUtils.ToInt(item.Value));
                    }
                }
            }
            return arraylist;
        }

        public static List<int> GetSelectedListControlValueIntList(ListControl listControl)
        {
            List<int> list = new List<int>();
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    if (item.Selected)
                    {
                        list.Add(TranslateUtils.ToInt(item.Value));
                    }
                }
            }
            return list;
        }

		public static string[] GetListControlValues(ListControl listControl)
		{
			ArrayList arraylist = new ArrayList();
			if (listControl != null)
			{
				foreach (ListItem item in listControl.Items)
				{
					arraylist.Add(item.Value);
				}
			}
			string[] retval = new string[arraylist.Count];
			arraylist.CopyTo(retval);
			return retval;
		}

		/// <summary>
		/// ���б�ؼ���ļ���������ѡ���ͬʱ��������ѡ�����
		/// </summary>
		/// <param name="listControl"></param>
		/// <param name="values"></param>
		public static void SelectListItems(ListControl listControl, params string[] values)
		{
			if (listControl != null)
			{
				foreach (ListItem item in listControl.Items)
				{
					item.Selected = false;
				}
				foreach (ListItem item in listControl.Items)
				{
					foreach (string value in values)
					{
						if (string.Equals(item.Value, value))
						{
							item.Selected = true;
                            break;
						}
					}
				}
			}
		}

        public static void SelectListItems(ListControl listControl, ICollection collection)
        {
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    item.Selected = false;
                }
                foreach (ListItem item in listControl.Items)
                {
                    foreach (string value in collection)
                    {
                        if (string.Equals(item.Value, value))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        public static void SelectListItems(ListControl listControl, List<int> collection)
        {
            if (listControl != null)
            {
                foreach (ListItem item in listControl.Items)
                {
                    item.Selected = false;
                }
                foreach (ListItem item in listControl.Items)
                {
                    foreach (int intVal in collection)
                    {
                        if (string.Equals(item.Value, intVal.ToString()))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

		public static void SelectListItemsIgnoreCase(ListControl listControl, params string[] values)
		{
			if (listControl != null)
			{
				foreach (ListItem item in listControl.Items)
				{
					item.Selected = false;
				}
				foreach (ListItem item in listControl.Items)
				{
					foreach (string value in values)
					{
						if (string.Equals(item.Value.ToLower(), value.ToLower()))
						{
							item.Selected = true;
                            break;
						}
					}
				}
			}
		}


		public static string SelectedItemsValueToStringCollection(ListItemCollection items)
		{
			StringBuilder builder = new StringBuilder();
			if (items!= null)
			{
				foreach (ListItem item in items)
				{
					if (item.Selected)
					{
						builder.Append(item.Value).Append(",");
					}
				}
				if (builder.Length != 0)
					builder.Remove(builder.Length - 1, 1);
			}
			return builder.ToString();
		}

        public static Control FindControlBySelfAndChildren(string id, Control baseControl)
        {
            Control ctrl = null;
            if (baseControl != null)
            {
                ctrl = baseControl.FindControl(id);
                if (ctrl == baseControl) ctrl = null;//DropDownList��FindControl����������
                if (ctrl == null && baseControl.HasControls())
                {
                    ctrl = FindControlByChildren(id, baseControl.Controls);
                }
            }            
            return ctrl;
        }

        public static Control FindControlByChildren(string id, ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                Control ctrl = FindControlBySelfAndChildren(id, control);
                if (ctrl != null)
                {
                    return ctrl;
                }
            }
            return null;
        }

        public static string GetInputValue(Control containerControl, string inputName)
        {
            Control control = null;
            return GetInputValue(containerControl, inputName, out control);
        }

        //TODO:�˷�����Ҫ��������
        /// <summary>
        /// ����null����δ�ҵ��ؼ���
        /// </summary>
        public static string GetInputValue(Control containerControl, string inputName, out Control control)
        {
            string value = null;

            control = ControlUtils.FindControlBySelfAndChildren(inputName, containerControl);
            if (control != null)
            {
                value = string.Empty;
                if (control is TextBox)
                {
                    TextBox input = (TextBox)control;
                    value = input.Text;
                }
                else if (control is HtmlInputControl)
                {
                    HtmlInputControl input = (HtmlInputControl)control;
                    value = input.Value;
                }
                else if (control is HtmlTextArea)
                {
                    HtmlTextArea input = (HtmlTextArea)control;
                    value = input.Value;
                }
                else if (control is TextEditorBase)
                {
                    TextEditorBase input = (TextEditorBase)control;
                    value = input.Text;
                }
                else if (control is ListControl)
                {
                    ListControl select = (ListControl)control;
                    ArrayList arraylist = ControlUtils.GetSelectedListControlValueArrayList(select);
                    value = TranslateUtils.ObjectCollectionToString(arraylist);
                }
                else if (control is HtmlSelect)
                {
                    HtmlSelect select = (HtmlSelect)control;
                    ArrayList arraylist = new ArrayList();
                    foreach (ListItem item in select.Items)
                    {
                        if (item.Selected)
                        {
                            arraylist.Add(item.Value);
                        }
                    }
                    value = TranslateUtils.ObjectCollectionToString(arraylist);
                }
                else if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    value = checkBox.Checked.ToString();
                }
            }
            return value;
        }

        //TODO:�˷�����Ҫ��������
        public static void SetInputValue(Control containerControl, string inputName, TableStyleInfo styleInfo)
        {
            Control control = ControlUtils.FindControlBySelfAndChildren(inputName, containerControl);

            if (control != null)
            {
                if (control is TextBox)
                {
                    TextBox input = (TextBox)control;
                    if (string.IsNullOrEmpty(input.Text) && !string.IsNullOrEmpty(styleInfo.DefaultValue))
                    {
                        input.Text = styleInfo.DefaultValue;
                    }
                }
                else if (control is HtmlInputControl)
                {
                    HtmlInputControl input = (HtmlInputControl)control;
                    if (string.IsNullOrEmpty(input.Value) && !string.IsNullOrEmpty(styleInfo.DefaultValue))
                    {
                        input.Value = styleInfo.DefaultValue;
                    }
                }
                else if (control is HtmlTextArea)
                {
                    HtmlTextArea input = (HtmlTextArea)control;
                    if (string.IsNullOrEmpty(input.Value) && !string.IsNullOrEmpty(styleInfo.DefaultValue))
                    {
                        input.Value = styleInfo.DefaultValue;
                    }
                }
                else if (control is TextEditorBase)
                {
                    TextEditorBase input = (TextEditorBase)control;
                    if (string.IsNullOrEmpty(input.Text) && !string.IsNullOrEmpty(styleInfo.DefaultValue))
                    {
                        input.Text = styleInfo.DefaultValue;
                    }
                }
                else if (control is ListControl)
                {
                    ListControl select = (ListControl)control;
                    if (select.Items.Count == 0)
                    {
                        ArrayList tableStyleItemInfoArrayList = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                        if (tableStyleItemInfoArrayList != null && tableStyleItemInfoArrayList.Count > 0)
                        {
                            foreach (TableStyleItemInfo styleItemInfo in tableStyleItemInfoArrayList)
                            {
                                ListItem listItem = new ListItem(styleItemInfo.ItemTitle, styleItemInfo.ItemValue);
                                listItem.Selected = styleItemInfo.IsSelected;
                                select.Items.Add(listItem);
                            }
                        }
                    }
                }
                else if (control is HtmlSelect)
                {
                    HtmlSelect select = (HtmlSelect)control;
                    if (select.Items.Count == 0)
                    {
                        ArrayList tableStyleItemInfoArrayList = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                        if (tableStyleItemInfoArrayList != null && tableStyleItemInfoArrayList.Count > 0)
                        {
                            foreach (TableStyleItemInfo styleItemInfo in tableStyleItemInfoArrayList)
                            {
                                ListItem listItem = new ListItem(styleItemInfo.ItemTitle, styleItemInfo.ItemValue);
                                listItem.Selected = styleItemInfo.IsSelected;
                                select.Items.Add(listItem);
                            }
                        }
                    }
                }
            }
        }


        public static void SetInputValue(Control containerControl, string inputName, string value, TableStyleInfo styleInfo)
        {
            Control control = ControlUtils.FindControlBySelfAndChildren(inputName, containerControl);

            if (control != null)
            {
                if (control is TextBox)
                {
                    TextBox input = (TextBox)control;
                    input.Text = value;
                }
                else if (control is HtmlInputControl)
                {
                    HtmlInputControl input = (HtmlInputControl)control;
                    input.Value = value;
                }
                else if (control is HtmlTextArea)
                {
                    HtmlTextArea input = (HtmlTextArea)control;
                    input.Value = value;
                }
                else if (control is TextEditorBase)
                {
                    TextEditorBase input = (TextEditorBase)control;
                    input.Text = value;
                }
                else if (control is ListControl)
                {
                    ListControl select = (ListControl)control;
                    if (select.Items.Count == 0)
                    {
                        ArrayList tableStyleItemInfoArrayList = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                        if (tableStyleItemInfoArrayList != null && tableStyleItemInfoArrayList.Count > 0)
                        {
                            foreach (TableStyleItemInfo styleItemInfo in tableStyleItemInfoArrayList)
                            {
                                ListItem listItem = new ListItem(styleItemInfo.ItemTitle, styleItemInfo.ItemValue);
                                listItem.Selected = styleItemInfo.IsSelected;
                                select.Items.Add(listItem);
                            }
                        }
                    }
                }
                else if (control is HtmlSelect)
                {
                    HtmlSelect select = (HtmlSelect)control;
                    if (select.Items.Count == 0)
                    {
                        ArrayList tableStyleItemInfoArrayList = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                        if (tableStyleItemInfoArrayList != null && tableStyleItemInfoArrayList.Count > 0)
                        {
                            foreach (TableStyleItemInfo styleItemInfo in tableStyleItemInfoArrayList)
                            {
                                ListItem listItem = new ListItem(styleItemInfo.ItemTitle, styleItemInfo.ItemValue);
                                listItem.Selected = styleItemInfo.IsSelected;
                                select.Items.Add(listItem);
                            }
                        }
                    }
                }
            }
        }


        //TODO:�˷�����Ҫ��������
        public static void SetLabelText(Control containerControl, string labelName, string text)
        {
            Control control = ControlUtils.FindControlBySelfAndChildren(labelName, containerControl);

            if (control != null)
            {
                if (control is Label)
                {
                    Label label = (Label)control;
                    label.Text = text;
				}
				else if (control is Literal)
				{
					Literal label = (Literal)control;
					label.Text = text;
				}
				else if (control is LiteralControl)
				{
					LiteralControl label = (LiteralControl)control;
					label.Text = text;
				}
                else if (control is HtmlContainerControl)
                {
                    HtmlContainerControl label = (HtmlContainerControl)control;
                    label.InnerHtml = text;
                }
				else if (control is HtmlGenericControl)
                {
                    HtmlGenericControl label = (HtmlGenericControl)control;
                    label.InnerHtml = text;
                }
            }
        }
	}
}

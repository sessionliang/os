// TemplatedList.cs.

namespace BaiRong.Controls 
{

	using System;
	using System.ComponentModel;
	using System.ComponentModel.Design;
	using System.ComponentModel.Design.Serialization;
	using System.Collections;
	using System.Diagnostics;
	using System.Web.UI;
	using System.Web.UI.WebControls;

	[
	DefaultEvent("SelectedIndexChanged"),
	DefaultProperty("DataSource"),
	Designer("CustomControls.Design.TemplatedListDesigner, CustomControls.Design", typeof(IDesigner))
	]
	public class TemplatedList : WebControl, INamingContainer 
	{

		#region Statics and Constants
		private static readonly object EventSelectedIndexChanged = new object();
		private static readonly object EventItemCreated = new object();
		private static readonly object EventItemDataBound = new object();
		private static readonly object EventItemCommand = new object();
		#endregion

		#region Member variables
		private IEnumerable dataSource;
		private TableItemStyle itemStyle;
		private TableItemStyle alternatingItemStyle;
		private TableItemStyle selectedItemStyle;
		private ITemplate itemTemplate;
		#endregion

		#region Properties
		[
		Category("Style"),
		Description("The style to be applied to alternate items."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerProperty),
		]
		public virtual TableItemStyle AlternatingItemStyle 
		{
			get 
			{
				if (alternatingItemStyle == null) 
				{
					alternatingItemStyle = new TableItemStyle();
					if (IsTrackingViewState)
						((IStateManager)alternatingItemStyle).TrackViewState();
				}
				return alternatingItemStyle;
			}
		}

		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(-1),
		Description("The cell padding of the rendered table.")
		]
		public virtual int CellPadding 
		{
			get 
			{
				if (ControlStyleCreated == false) 
				{
					return -1;
				}
				return ((TableStyle)ControlStyle).CellPadding;
			}
			set 
			{
				((TableStyle)ControlStyle).CellPadding = value;
			}
		}

		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(0),
		Description("The cell spacing of the rendered table.")
		]
		public virtual int CellSpacing 
		{
			get 
			{
				if (ControlStyleCreated == false) 
				{
					return 0;
				}
				return ((TableStyle)ControlStyle).CellSpacing;
			}
			set 
			{
				((TableStyle)ControlStyle).CellSpacing = value;
			}
		}

		[
		Bindable(true),
		Category("Data"),
		DefaultValue(null),
		Description("The data source used to build up the control."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public IEnumerable DataSource 
		{
			get 
			{
				return dataSource;
			}
			set 
			{
				dataSource = value;
			}
		}

		[
		Bindable(true),
		Category("Appearance"),
		DefaultValue(GridLines.None),
		Description("The grid lines to be shown in the rendered table.")
		]
		public virtual GridLines GridLines 
		{
			get 
			{
				if (ControlStyleCreated == false) 
				{
					return GridLines.None;
				}
				return ((TableStyle)ControlStyle).GridLines;
			}
			set 
			{
				((TableStyle)ControlStyle).GridLines = value;
			}
		}

		[
		Category("Style"),
		Description("The style to be applied to all items."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerProperty),
		]
		public virtual TableItemStyle ItemStyle 
		{
			get 
			{
				if (itemStyle == null) 
				{
					itemStyle = new TableItemStyle();
					if (IsTrackingViewState)
						((IStateManager)itemStyle).TrackViewState();
				}
				return itemStyle;
			}
		}

		[
		Browsable(false),
		DefaultValue(null),
		Description("The content to be shown in each item."),
		PersistenceMode(PersistenceMode.InnerProperty),
		TemplateContainer(typeof(TemplatedListItem))
		]
		public virtual ITemplate ItemTemplate 
		{
			get 
			{
				return itemTemplate;
			}
			set 
			{
				itemTemplate = value;
			}
		}

		[
		Bindable(true),
		DefaultValue(-1),
		Description("The index of the selected item.")
		]
		public virtual int SelectedIndex 
		{
			get 
			{
				object o = ViewState["SelectedIndex"];
				if (o != null)
					return(int)o;
				return -1;
			}
			set 
			{
				if (value < -1) 
				{
					throw new ArgumentOutOfRangeException();
				}
				int oldSelectedIndex = SelectedIndex;
				ViewState["SelectedIndex"] = value;

				if (HasControls()) 
				{
					Table table = (Table)Controls[0];
					TemplatedListItem item;

					if ((oldSelectedIndex != -1) && (table.Rows.Count > oldSelectedIndex)) 
					{
						item = (TemplatedListItem)table.Rows[oldSelectedIndex];

						if (item.ItemType != ListItemType.EditItem) 
						{
							ListItemType itemType = ListItemType.Item;
							if (oldSelectedIndex % 2 != 0)
								itemType = ListItemType.AlternatingItem;
							item.SetItemType(itemType);
						}
					}
					if ((value != -1) && (table.Rows.Count > value)) 
					{
						item = (TemplatedListItem)table.Rows[value];
						item.SetItemType(ListItemType.SelectedItem);
					}
				}
			}
		}

		[
		Category("Style"),
		Description("The style to be applied to the selected item."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerProperty),
		]
		public virtual TableItemStyle SelectedItemStyle 
		{
			get 
			{
				if (selectedItemStyle == null) 
				{
					selectedItemStyle = new TableItemStyle();
					if (IsTrackingViewState)
						((IStateManager)selectedItemStyle).TrackViewState();
				}
				return selectedItemStyle;
			}
		}
		#endregion

		#region Events
		protected virtual void OnItemCommand(TemplatedListCommandEventArgs e) 
		{
			TemplatedListCommandEventHandler onItemCommandHandler = (TemplatedListCommandEventHandler)Events[EventItemCommand];
			if (onItemCommandHandler != null) onItemCommandHandler(this, e);
		}

		protected virtual void OnItemCreated(TemplatedListItemEventArgs e) 
		{
			TemplatedListItemEventHandler onItemCreatedHandler = (TemplatedListItemEventHandler)Events[EventItemCreated];
			if (onItemCreatedHandler != null) onItemCreatedHandler(this, e);
		}

		protected virtual void OnItemDataBound(TemplatedListItemEventArgs e) 
		{
			TemplatedListItemEventHandler onItemDataBoundHandler = (TemplatedListItemEventHandler)Events[EventItemDataBound];
			if (onItemDataBoundHandler != null) onItemDataBoundHandler(this, e);
		}

		protected virtual void OnSelectedIndexChanged(EventArgs e) 
		{
			EventHandler handler = (EventHandler)Events[EventSelectedIndexChanged];
			if (handler != null) handler(this, e);
		}

		[
		Category("Action"),
		Description("Raised when a CommandEvent occurs within an item.")
		]
		public event TemplatedListCommandEventHandler ItemCommand 
		{
			add 
			{
				Events.AddHandler(EventItemCommand, value);
			}
			remove 
			{
				Events.RemoveHandler(EventItemCommand, value);
			}
		}

		[
		Category("Behavior"),
		Description("Raised when an item is created and is ready for customization.")
		]
		public event TemplatedListItemEventHandler ItemCreated 
		{
			add 
			{
				Events.AddHandler(EventItemCreated, value);
			}
			remove 
			{
				Events.RemoveHandler(EventItemCreated, value);
			}
		}

		[
		Category("Behavior"),
		Description("Raised when an item is data-bound.")
		]
		public event TemplatedListItemEventHandler ItemDataBound 
		{
			add 
			{
				Events.AddHandler(EventItemDataBound, value);
			}
			remove 
			{
				Events.RemoveHandler(EventItemDataBound, value);
			}
		}

		[
		Category("Action"),
		Description("Raised when the SelectedIndex property has changed.")
		]
		public event EventHandler SelectedIndexChanged 
		{
			add 
			{
				Events.AddHandler(EventSelectedIndexChanged, value);
			}
			remove 
			{
				Events.RemoveHandler(EventSelectedIndexChanged, value);
			}
		}
		#endregion

		#region Methods and Implementation
		protected override void CreateChildControls() 
		{
			Controls.Clear();

			if (ViewState["ItemCount"] != null) 
			{
				// Create the control hierarchy using the view state, 
				// not the data source.
				CreateControlHierarchy(false);
			}
		}

		private void CreateControlHierarchy(bool useDataSource) 
		{
			IEnumerable dataSource = null;
			int count = -1;

			if (useDataSource == false) 
			{
				// ViewState must have a non-null value for ItemCount because this is checked 
				//  by CreateChildControls.
				count = (int)ViewState["ItemCount"];
				if (count != -1) 
				{
					dataSource = new DummyDataSource(count);
				}
			}
			else 
			{
				dataSource = this.dataSource;
			}

			if (dataSource != null) 
			{
				Table table = new Table();
				Controls.Add(table);

				int selectedItemIndex = SelectedIndex;
				int index = 0;

				count = 0;
				foreach (object dataItem in dataSource) 
				{
					ListItemType itemType = ListItemType.Item;
					if (index == selectedItemIndex) 
					{
						itemType = ListItemType.SelectedItem;
					}
					else if (index % 2 != 0) 
					{
						itemType = ListItemType.AlternatingItem;
					}

					CreateItem(table, index, itemType, useDataSource, dataItem);
					count++;
					index++;
				}
			}

			if (useDataSource) 
			{
				// Save the number of items contained for use in round trips.
				ViewState["ItemCount"] = ((dataSource != null) ? count : -1);
			}
		}

		protected override System.Web.UI.WebControls.Style CreateControlStyle() 
		{
			// Since the TemplatedList control renders an HTML table, 
			// an instance of the TableStyle class is used as the control style.

			TableStyle style = new TableStyle(ViewState);

			// Set up default initial state.
			style.CellSpacing = 0;

			return style;
		}

		private TemplatedListItem CreateItem(Table table, int itemIndex, ListItemType itemType, bool dataBind, object dataItem) 
		{
			TemplatedListItem item = new TemplatedListItem(itemIndex, itemType);
			TemplatedListItemEventArgs e = new TemplatedListItemEventArgs(item);

			if (itemTemplate != null) 
			{
				itemTemplate.InstantiateIn(item.Cells[0]);
			}
			if (dataBind) 
			{
				item.DataItem = dataItem;
			}
			OnItemCreated(e);
			table.Rows.Add(item);

			if (dataBind) 
			{
				item.DataBind();
				OnItemDataBound(e);

				item.DataItem = null;
			}

			return item;
		}

		public override void DataBind() 
		{
			// Controls with a data-source property perform their custom data binding
			// by overriding DataBind.

			// Evaluate any data-binding expressions on the control itself.
			base.OnDataBinding(EventArgs.Empty);

			// Reset the control state.
			Controls.Clear();
			ClearChildViewState();

			//  Create the control hierarchy using the data source.
			CreateControlHierarchy(true);
			ChildControlsCreated = true;

			TrackViewState();
		}

		protected override void LoadViewState(object savedState) 
		{
			// Customize state management to handle saving state of contained objects.

			if (savedState != null) 
			{
				object[] myState = (object[])savedState;

				if (myState[0] != null)
					base.LoadViewState(myState[0]);
				if (myState[1] != null)
					((IStateManager)ItemStyle).LoadViewState(myState[1]);
				if (myState[2] != null)
					((IStateManager)SelectedItemStyle).LoadViewState(myState[2]);
				if (myState[3] != null)
					((IStateManager)AlternatingItemStyle).LoadViewState(myState[3]);
			}
		}

		protected override bool OnBubbleEvent(object source, EventArgs e) 
		{
			// Handle events raised by children by overriding OnBubbleEvent.

			bool handled = false;

			if (e is TemplatedListCommandEventArgs) 
			{
				TemplatedListCommandEventArgs ce = (TemplatedListCommandEventArgs)e;

				OnItemCommand(ce);
				handled = true;

				if (String.Compare(ce.CommandName, "Select", true) == 0) 
				{
					SelectedIndex = ce.Item.ItemIndex;
					OnSelectedIndexChanged(EventArgs.Empty);
				}
			}

			return handled;
		}

		private void PrepareControlHierarchy() 
		{
			if (HasControls() == false) 
			{
				return;
			}

			Debug.Assert(Controls[0] is Table);
			Table table = (Table)Controls[0];

			table.CopyBaseAttributes(this);
			if (ControlStyleCreated) 
			{
				table.ApplyStyle(ControlStyle);
			}

			// The composite alternating item style; do just one
			// merge style on the actual item.
			System.Web.UI.WebControls.Style altItemStyle = null;
			if (alternatingItemStyle != null) 
			{
				altItemStyle = new TableItemStyle();
				altItemStyle.CopyFrom(itemStyle);
				altItemStyle.CopyFrom(alternatingItemStyle);
			}
			else 
			{
				altItemStyle = itemStyle;
			}

			int rowCount = table.Rows.Count;
			for (int i = 0; i < rowCount; i++) 
			{
				TemplatedListItem item = (TemplatedListItem)table.Rows[i];
				System.Web.UI.WebControls.Style compositeStyle = null;

				switch (item.ItemType) 
				{
					case ListItemType.Item:
						compositeStyle = itemStyle;
						break;

					case ListItemType.AlternatingItem:
						compositeStyle = altItemStyle;
						break;

					case ListItemType.SelectedItem: 
					{
						compositeStyle = new TableItemStyle();

						if (item.ItemIndex % 2 != 0)
							compositeStyle.CopyFrom(altItemStyle);
						else
							compositeStyle.CopyFrom(itemStyle);
						compositeStyle.CopyFrom(selectedItemStyle);
					}
						break;
				}

				if (compositeStyle != null) 
				{
					item.MergeStyle(compositeStyle);
				}
			}
		}

		protected override void Render(HtmlTextWriter writer) 
		{
			// Apply styles to the control hierarchy
			// and then render it out.

			// Apply styles during render phase, so the user can change styles
			// after calling DataBind without the property changes ending
			// up in view state.
			PrepareControlHierarchy();

			RenderContents(writer);
		}

		protected override object SaveViewState() 
		{
			// Customized state management to handle saving state of contained objects such as styles.

			object baseState = base.SaveViewState();
			object itemStyleState = (itemStyle != null) ? ((IStateManager)itemStyle).SaveViewState() : null;
			object selectedItemStyleState = (selectedItemStyle != null) ? ((IStateManager)selectedItemStyle).SaveViewState() : null;
			object alternatingItemStyleState = (alternatingItemStyle != null) ? ((IStateManager)alternatingItemStyle).SaveViewState() : null;

			object[] myState = new object[4];
			myState[0] = baseState;
			myState[1] = itemStyleState;
			myState[2] = selectedItemStyleState;
			myState[3] = alternatingItemStyleState;

			return myState;
		}

		protected override void TrackViewState() 
		{
			// Customized state management to handle saving state of contained objects such as styles.

			base.TrackViewState();

			if (itemStyle != null)
				((IStateManager)itemStyle).TrackViewState();
			if (selectedItemStyle != null)
				((IStateManager)selectedItemStyle).TrackViewState();
			if (alternatingItemStyle != null)
				((IStateManager)alternatingItemStyle).TrackViewState();
		}
		#endregion
	}


	public class TemplatedListItem : TableRow, INamingContainer 
	{
		private int itemIndex;
		private ListItemType itemType;
		private object dataItem;

		public TemplatedListItem(int itemIndex, ListItemType itemType) 
		{
			this.itemIndex = itemIndex;
			this.itemType = itemType;

			Cells.Add(new TableCell());
		}

		public virtual object DataItem 
		{
			get 
			{
				return dataItem;
			}
			set 
			{
				dataItem = value;
			}
		}

		public virtual int ItemIndex 
		{
			get 
			{
				return itemIndex;
			}
		}

		public virtual ListItemType ItemType 
		{
			get 
			{
				return itemType;
			}
		}

		protected override bool OnBubbleEvent(object source, EventArgs e) 
		{
			if (e is CommandEventArgs) 
			{
				// Add the information about Item to CommandEvent.

				TemplatedListCommandEventArgs args =
					new TemplatedListCommandEventArgs(this, source, (CommandEventArgs)e);

				RaiseBubbleEvent(this, args);
				return true;
			}
			return false;
		}

		internal void SetItemType(ListItemType itemType) 
		{
			this.itemType = itemType;
		}
	}

	public sealed class TemplatedListCommandEventArgs : CommandEventArgs 
	{

		private TemplatedListItem item;
		private object commandSource;

		public TemplatedListCommandEventArgs(TemplatedListItem item, object commandSource, CommandEventArgs originalArgs) :
			base(originalArgs) 
		{
			this.item = item;
			this.commandSource = commandSource;
		}

		public TemplatedListItem Item 
		{
			get 
			{
				return item;
			}
		}

		public object CommandSource 
		{
			get 
			{
				return commandSource;
			}
		}
	}

	public delegate void TemplatedListCommandEventHandler(object source, TemplatedListCommandEventArgs e);

	public sealed class TemplatedListItemEventArgs : EventArgs 
	{

		private TemplatedListItem item;

		public TemplatedListItemEventArgs(TemplatedListItem item) 
		{
			this.item = item;
		}

		public TemplatedListItem Item 
		{
			get 
			{
				return item;
			}
		}
	}

	public delegate void TemplatedListItemEventHandler(object sender, TemplatedListItemEventArgs e);

	internal sealed class DummyDataSource : ICollection 
	{

		private int dataItemCount;

		public DummyDataSource(int dataItemCount) 
		{
			this.dataItemCount = dataItemCount;
		}

		public int Count 
		{
			get 
			{
				return dataItemCount;
			}
		}

		public bool IsReadOnly 
		{
			get 
			{
				return false;
			}
		}

		public bool IsSynchronized 
		{
			get 
			{
				return false;
			}
		}

		public object SyncRoot 
		{
			get 
			{
				return this;
			}
		}

		public void CopyTo(Array array, int index) 
		{
			for (IEnumerator e = this.GetEnumerator(); e.MoveNext();)
				array.SetValue(e.Current, index++);
		}

		public IEnumerator GetEnumerator() 
		{
			return new DummyDataSourceEnumerator(dataItemCount);
		}


		private class DummyDataSourceEnumerator : IEnumerator 
		{

			private int count;
			private int index;

			public DummyDataSourceEnumerator(int count) 
			{
				this.count = count;
				this.index = -1;
			}

			public object Current 
			{
				get 
				{
					return null;
				}
			}

			public bool MoveNext() 
			{
				index++;
				return index < count;
			}

			public void Reset() 
			{
				this.index = -1;
			}
		}
	}
}

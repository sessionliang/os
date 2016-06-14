using System;
using System.Xml.Serialization;

namespace BaiRong.Core
{
	/// <summary>
	/// Tab is a container object which represents a singe tab
	/// </summary>
	[Serializable]
	public class Tab
	{
		#region Private Members
        private string _id;
		private string _text;
		private string _href;
		private string _name;
		private string _permissions;
		private bool _enable = true;
        private bool _isOwner = false;
        private bool _isPlatform = false;
		private Tab[] _children;
		private bool _keepQueryString = false;
		private bool _selected = false;
		private string _target;
		private string _iconUrl;
        private string _ban;
		private string _addtionalString;
		#endregion

		#region Public Properties

        /// <summary>
        /// Property Text (string)
        /// </summary>
        [XmlAttribute("id")]
        public string ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

		/// <summary>
		/// Property Text (string)
		/// </summary>
		[XmlAttribute("text")]
		public string Text
		{
			get {  return this._text; }
			set {  this._text = value; }
		}

        
		/// <summary>
		/// Property Href (string)
		/// </summary>
		[XmlAttribute("href")]
		public string Href
		{
			get {  return this._href; }
			set {  this._href = value; }
		}

		/// <summary>
		/// Property Name (string)
		/// </summary>
		[XmlAttribute("name")]
		public string Name
		{
			get {  return this._name; }
			set {  this._name = value; }
		}

		/// <summary>
		/// Property Permissions (string)
		/// </summary>
		[XmlAttribute("permissions")]
		public string Permissions
		{
			get {  return this._permissions; }
			set {  this._permissions = value; }
		}

		/// <summary>
		/// Property Enable (bool)
		/// </summary>
		[XmlAttribute("enabled")]
		public bool Enabled
		{
			get {  return this._enable; }
			set {  this._enable = value; }
		}

        [XmlAttribute("isowner")]
        public bool IsOwner
        {
            get { return this._isOwner; }
            set { this._isOwner = value; }
        }

        [XmlAttribute("isplatform")]
        public bool IsPlatform
        {
            get { return this._isPlatform; }
            set { this._isPlatform = value; }
        }

		/// <summary>
		/// Property KeepQueryString (bool)
		/// </summary>
		[XmlAttribute("keepQueryString")]
		public bool KeepQueryString
		{
			get {  return this._keepQueryString; }
			set {  this._keepQueryString = value; }
		}

		/// <summary>
		/// Property Selected (bool)
		/// </summary>
		[XmlAttribute("selected")]
		public bool Selected
		{
			get {  return this._selected; }
			set {  this._selected = value; }
		}

		/// <summary>
		/// Property Target (string)
		/// </summary>
		[XmlAttribute("target")]
		public string Target
		{
			get {  return this._target; }
			set {  this._target = value; }
		}

		/// <summary>
		/// Property IconUrl (string)
		/// </summary>
		[XmlAttribute("iconUrl")]
		public string IconUrl
		{
			get {  return this._iconUrl; }
			set {  this._iconUrl = value; }
		}

        /// <summary>
        /// Property AddtionalString (string)
        /// </summary>
        [XmlAttribute("ban")]
        public string Ban
        {
            get { return this._ban; }
            set { this._ban = value; }
        }

		/// <summary>
		/// Property AddtionalString (string)
		/// </summary>
		[XmlAttribute("addtionalString")]
		public string AddtionalString
		{
			get {  return this._addtionalString; }
			set {  this._addtionalString = value; }
		}


		/// <summary>
		/// Property Children (Tab[])
		/// </summary>
		[XmlArray("SubTabs")]
		public Tab[] Children
		{
			get {  return this._children; }
			set {  this._children = value; }
		}

		#endregion

		#region Has Helpers
		public bool HasChildren
		{
			get{return this.Children != null && this.Children.Length > 0;}
		}

		public bool HasPermissions
		{
			get{return !string.IsNullOrEmpty(this.Permissions);}
		}

		public bool HasHref
		{
			get{return !string.IsNullOrEmpty(this.Href);}
		}

		#endregion

        public Tab Clone()
        {
            return this.MemberwiseClone() as Tab;
        }

	}
}

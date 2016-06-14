FCKConfig.FontNames= '宋体;新宋体;黑体;仿宋_GB2312;楷体_GB2312;隶书;幼圆;Arial;Comic Sans MS;Courier New;Tahoma;Times New Roman;Verdana' ;
FCKConfig.FontSizes= 'smaller;larger;xx-small;x-small;small;medium;large;x-large;xx-large' ;

FCKConfig.ToolbarSets['MyToolbarSet'] = [
	['Source','-','Preview','-','Templates'],
	['Cut','Copy','Paste','PasteText','PasteWord','-','Print','SpellCheck'],
	['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
	['ShowBlocks','FitWindow'],
	'/',
	['Image','Flash','flvPlayer','Rule','Smiley','SpecialChar'],
	['Table','TableInsertRowAfter','TableDeleteRows','TableInsertColumnAfter','TableDeleteColumns','TableInsertCellAfter','TableDeleteCells','TableHorizontalSplitCell','TableCellProp'],
	['TextColor','BGColor','TypeSetting'],
	'/',
	['Bold','Italic','Underline','StrikeThrough','-','Subscript','Superscript'],
	['OrderedList','UnorderedList','-','Outdent','Indent','Blockquote'],
	['JustifyLeft','JustifyCenter','JustifyRight','JustifyFull'],
	['Link','Unlink','NextPage'],
	'/',
	['Style','FontFormat','FontName','FontSize']
] ;

var sOtherPluginPath = FCKConfig.BasePath.substr(0, FCKConfig.BasePath.length - 7) + 'editor/plugins/' ;
FCKConfig.Plugins.Add( 'tablecommands', null, sOtherPluginPath ) ;
FCKConfig.Plugins.Add( 'simplecommands', null, sOtherPluginPath ) ;
FCKConfig.Plugins.Add( 'nextpage', null, sOtherPluginPath ) ;
FCKConfig.Plugins.Add( 'typesetting', null, sOtherPluginPath ) ;
FCKConfig.Plugins.Add( 'flvPlayer',null, sOtherPluginPath) ;
var FCKNextPage = function(name)  
{
this.Name = name;
}
 
FCKNextPage.prototype.Execute = function()  
{  
	var oSpan = FCK.InsertElement( 'span' ) ;
	oSpan.innerHTML = '[SITESERVER_PAGE]' ;

	oSpan.style.backgroundColor = '#ffff00' ;
	oSpan.style.color = '#000000' ;
	
	oSpan._fcknextpage = true ;
	oSpan.contentEditable = false ;

	// To avoid it to be resized.
	oSpan.onresizestart = function()
	{
		FCK.EditorWindow.event.returnValue = false ;
		return false ;
	}
}  
 
// manage the plugins' button behavior  
FCKNextPage.prototype.GetState = function()  
{  
return FCK_TRISTATE_OFF;  
}  
 
FCKCommands.RegisterCommand( 'NextPage', new FCKNextPage('NextPage')) ;  
 
// Create the toolbar button. 
var oNextPageItem = new FCKToolbarButton( 'NextPage', FCKLang.NextPageBtn ) ; 
oNextPageItem.IconPath = FCKPlugins.Items['nextpage'].Path + 'nextpage.gif' ;
FCKToolbarItems.RegisterItem( 'NextPage', oNextPageItem ) ;

FCKNextPage._SetupClickListener = function()
{
	FCKNextPage._ClickListener = function( e )
	{
		if ( e.target.tagName == 'SPAN' && e.target._fcknextpage )
			FCKSelection.SelectNode( e.target ) ;
	}

	FCK.EditorDocument.addEventListener( 'click', FCKNextPage._ClickListener, true ) ;
}

if ( FCKBrowserInfo.IsIE )
{
	FCKNextPage.Redraw = function()
	{
		if ( FCK.EditMode != FCK_EDITMODE_WYSIWYG )
			return ;

		var aNextPages = FCK.EditorDocument.body.innerText.match( /\[\SITESERVER_PAGE\]/g ) ;
		if ( !aNextPages )
			return ;

		var oRange = FCK.EditorDocument.body.createTextRange() ;

		for ( var i = 0 ; i < aNextPages.length ; i++ )
		{
			if ( oRange.findText( aNextPages[i] ) )
			{
				oRange.pasteHTML( '<span style="color: #000000; background-color: #ffff00" contenteditable="false" _fcknextpage="true">' + aNextPages[i] + '</span>' ) ;
			}
		}
	}
	
	FCK.Events.AttachEvent( 'OnAfterSetHTML', FCKNextPage.Redraw ) ;
}

FCKXHtml.TagProcessors['span'] = function( node, htmlNode )
{
	if ( htmlNode._fcknextpage )
		node = FCKXHtml.XML.createTextNode( '[SITESERVER_PAGE]' ) ;
	else
		FCKXHtml._AppendChildNodes( node, htmlNode, false ) ;

	return node ;
}
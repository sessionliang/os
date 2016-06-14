CKEDITOR.plugins.add( 'typesetting',
{
	init : function( editor )
	{
		var mytrim = function (text){
			var oldlen = text.length;
			var result="";
			var begin=0;
			//去掉段落前面的空格，回车，TAB
			for(var i=0;i<oldlen;i++){
				var c1 = text.charAt(i);
				if (begin==0){
					if ((c1!='\r')&&(c1!='\n')&&(c1!=' ')&&(c1!='　')&&(c1!='\t')){
						result +=c1;
						begin=1;
					}
				}else{
					result +=c1;
				}
			}
		
			
			//去掉段落前面的 &nbsp;
			begin=0;
			var pos = result.indexOf("&nbsp;");
			while(begin==0 && pos==0){
				result = result.substr(6);
				pos = result.indexOf("&nbsp;");
				if (pos>0)begin=1;
			}
		
			text = result;
			oldlen = text.length;
			begin=0;
			result = "";
			//去掉段落后面的空格，回车，TAB
			for(var i=oldlen-1;i>=0;i--){
				var c1 = text.charAt(i);
				if (begin==0){
					if ((c1!='\r')&&(c1!='\n')&&(c1!=' ')&&(c1!='　')&&(c1!='\t')){
						result =c1 + result;
						begin=1;
					}
				}else{
					result =c1 + result;
				}
			}
		
			if(result!="")
				return("<p>　　" + result + "</p>");
			else
				return("");
		};

		var formatText = function (text){
			
			var naivete_array =text.split("<br>");
			if (naivete_array && naivete_array.length >=0){
				text="";
				for (loop=0; loop < naivete_array.length;loop++){
					text = text + "<p>" + naivete_array[loop];
				}
			}
			
			naivete_array =text.split("<BR>");
			if (naivete_array && naivete_array.length >=0){
				text="";
				for (loop=0; loop < naivete_array.length;loop++){
					text = text + "<p>" + naivete_array[loop];
				}
			}
			
			naivete_array =text.split("<P>");
			if (naivete_array && naivete_array.length >=0){
				text="";
				for (loop=0; loop < naivete_array.length;loop++){
					text = text + "<p>" + naivete_array[loop];
				}
			}
			
			naivete_array =text.split("</P>");
			if (naivete_array && naivete_array.length >=0){
				text="";
				for (loop=0; loop < naivete_array.length;loop++){
					text = text + "<p>" + naivete_array[loop];
				}
			}
			
			naivete_array =text.split("</p>");
			if (naivete_array && naivete_array.length >=0){
				text="";
				for (loop=0; loop < naivete_array.length;loop++){
					text = text + "<p>" + naivete_array[loop];
				}
			}
			
			naivete_array =text.split("<p>");
			if (naivete_array && naivete_array.length >=0){
				text="";
				for (loop=0; loop < naivete_array.length;loop++){
					text = text + mytrim(naivete_array[loop]);
				}
			}
			
			return text;
		};
		
		var cmd = {
			exec:function(editor){				
				editor.setData(formatText(editor.getData()));
				alert('自动排版成功');
			}
		};

		editor.addCommand( 'typesetting', cmd);
		editor.ui.addButton( 'TypeSetting',
			{
				label : '自动排版',
				icon : this.path+'typesetting.gif',
				command : 'typesetting'
			} );
	},
 
	requires : [ 'fakeobjects' ]
} );
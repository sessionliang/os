function stlLoading(){
	$('.stl-element .bar').width($('.stl-element .bar').width() + 100);
}

var isStlChangeRunning = false;

function stlChange(ui, current){
	if (isStlChangeRunning) return;
	isStlChangeRunning = true;
	setTimeout('isStlChangeRunning = false;', 1000);

	var $this = $(current);
	var stlSequence = ui.item.attr('stl-sequence');
	var stlSequenceCollection = $this.sortable("toArray", {attribute: 'stl-sequence'}).join(",");
	var stlElementIndex = ui.item.attr('stl-index');
	var stlDivIndex = $this.attr('stl-index');

	if (stlSequence == '-1'){
		var stlElementToAdd = ui.item.find('.spec_view').html();
		var stlModal = ui.item.attr('stl-modal');
		var stlIsContainer = ui.item.attr('stl-is-container');
		if ($this.hasClass('stl-element-container')){
			stlIsContainer = 'false';
		}

		if (stlModal){
			stlElementToAdd = encodeURIComponent(stlElementToAdd.replace(/[\r\n]/g, "__R_A_N__"));
			stlModal = stlModal.replace('PLACE_HOLDER=', 'stlIsContainer=' + stlIsContainer + '&stlSequenceCollection=' + stlSequenceCollection + '&stlDivIndex=' + stlDivIndex + '&stlElementToAdd=' + stlElementToAdd).replace('return false;', '');
			eval(stlModal);
		}else{
			$.post($pageInfo.stlTemplateUrl, 
				{
					operation: 'Insert',
	  				stlSequence: stlSequence,
	  				stlSequenceCollection: stlSequenceCollection,
	  				stlElementIndex:stlElementIndex,
	  				stlDivIndex: stlDivIndex,
	  				stlIsContainer: stlIsContainer,
	  				stlElementToAdd: stlElementToAdd
				}, 
				function(data, textStatus) {
		      var obj = $.parseJSON(data);
		      if(obj.success == 'false'){
		      	alert(obj.errorMessage);
		      }else{
		      	setInterval('location.reload(true);', 1000);
		      }
				}
			);
		}

	}else{

		$.post($pageInfo.stlTemplateUrl, 
			{
				operation: 'Move',
				stlSequence: stlSequence,
				stlSequenceCollection: stlSequenceCollection,
				stlElementIndex:stlElementIndex,
				stlDivIndex: stlDivIndex,
			}, 
			function(data, textStatus) {
	      var obj = $.parseJSON(data);
	      if(obj.success == 'false'){
	      	alert(obj.errorMessage);
	      }else{
	      	location.reload(true);
	      }
			}
		);
	}
}

$(document).ready(function(){

	// $(".nav-header").click(function() {
	// 	$(".sidebar-nav .boxes, .sidebar-nav .rows").hide();
	// 	$(this).next().slideDown();
	// });

	var containers = $(".stl-wrapper").parent().closest('div');
	containers = containers.add($(".stl-hidden").parent().closest('div'));
	containers = containers.add("div.stl-container");
	containers = containers.add("div.stl-element-container");
	containers = containers.add(containers.siblings());
	containers.addClass("stl-container");

	containers.sortable({
	    connectWith: containers,
	    items: ".stl-element",
	    handle: ".stl-drag",
	    placeholder: "stl-highlight",
	    opacity: .35,
	    start: function (event, ui) {
	    	$('.stl-highlight').height(ui.item.height());
	    },
	    update: function (event, ui) {
			if (!ui.sender && this === ui.item.parent().closest('div')[0]){
				stlChange(ui, this);
			}

	    	var stlModal = ui.item.attr('stl-modal');
			if (stlModal){
				ui.item.html('');
			}else{
				ui.item.html($('#stl-loading').html());
				setInterval(stlLoading, 60);
			}
	    },
	    receive: function (event, ui) {
	  		stlChange(ui, this);
	    }
  	}).disableSelection();

  	$(".spec_mim_tools li ").draggable({
		connectToSortable: containers,
		helper: "clone",
		handle: ".stl-drag",
		drag: function(event, ui) {
			ui.helper.width(400);
		}
	});

	if ($("#stl-guideline").hasClass('spec_top_cutnbtn')){
		containers.addClass("stl-guideline");
	}

	$("#stl-guideline").click(function() {
		var isGuideLine = $(this).hasClass('spec_top_cutnbtn');
		if (isGuideLine){
			containers.removeClass("stl-guideline");
			$(this).removeClass("spec_top_cutnbtn");
		}else{
			containers.addClass("stl-guideline");
			$(this).addClass("spec_top_cutnbtn");
		}
		$.post($pageInfo.stlTemplateUrl, 
			{
				operation: 'GuideLine',
				isGuideLine: !isGuideLine
			},
			function(data, textStatus) {
	      
			}
		);
	});

});
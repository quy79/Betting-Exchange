// JavaScript Document

/* Favourites selector */



function submitSearchForm(){
	document.catSelForm.submit();
}

function submitJoinCompetition(){
	document.joincompetition.submit();
}

var doInit = true;
function initSearchInput(){
	if(doInit){
		$('searchinput').value = '';
		doInit = false;
	}
}

/* category tree */

function submitCategories(){
	var catRequestParam = '';
	$$('.category_tree .inputhidden').each(function(item){
		if(item.value == 'on')
			catRequestParam += item.name + '_';
	});
	linkToOddsMultipleCategories(catRequestParam);
}

function getRequestParameters(){
	return $('requestParameters').value;
	
}

function linkToOddsMultipleCategories(categoryCodes){

	var url = UserUtils.getLocaleDir() + '/sports/odds.do';
	if(!categoryCodes==''){
		url = url + '?categoryCodes='+categoryCodes+getRequestParameters();
	}

	top.document.location = url;
}

function linkToOdds(category, livebetting){

	var url = "";

    if(category==''){
        category = "%25";
    }

    if (livebetting) {
        url = UserUtils.getLocaleDir() + '/livebetting/lobby.do?categoryCode='+category;
    } else {
        url = UserUtils.getLocaleDir() + '/sports/odds.do';
        url = url + '?categoryCode='+category+getRequestParameters();
    }
	
	top.document.location = url;
}

function linkToOddsPage(element){
	var hiddenInput = element.down('.inputhidden',0);
	var category = hiddenInput.name;
	
	if(element.id == 'cat_ALL'){
		category = '';
	}
	linkToOdds(category);
}

/* title click functionality */
function titleClick(id){
	var element = jQuery('.' + id);
	if(element.hasClassName('level_1') && id!='cat_ALL'){
		var s = jQuery(id+'sub');
		//instead of toggle function, fixes ie style problems.
		if(s.style.display=="none") {
			s.style.display = "inline-block";
		} else {
			s.style.display="none";
		}
	}
	if (id=='cat_ALL'){
		//linkToOddsPage(element);
	}
}

/* end title click functionality */
 
/* select box click funtionality */
 
function toggleSelect(id,level1parent){
	
	var element = $(id);
	var selected = isSelected(element);
	setSelected(element, !selected);
	if(element.id == 'cat_ALL'){
		toggleAllCategories(!selected);
	}else if(element.hasClassName('level_1')){
		toggleSubCategories(element, !selected);
	}else if(element.hasClassName('level_2')){
		toggleBottomCategories(id, !selected);
	}
	/*
	if(element.id!='cat_ALL' && selected==true) {
		setSelected($('cat_ALL'),false);
	}
	 */
	selectParentIfChildrenSelected();
}

function selectParentIfChildrenSelected() {
	selectParentLevelIfChildrenSelected('li.level_2');
	selectParentLevelIfChildrenSelected('li.level_1');
}

function selectParentLevelIfChildrenSelected(level) {
	window.shouldSelectAll = false;
	if (level == 'li.level_1') {
		window.shouldSelectAll = true;
	}
	$$(level).each(function(parent) {
		if (parent.id != 'cat_ALL') {
			window.shouldSelect = isSelected(parent);
			var children = $$('li.childto_' + parent.id);
			if (children.length > 0) {
				window.shouldSelect = true;
				children.each(function(child) {
					if (window.shouldSelect && !isSelected(child)) {
						window.shouldSelect = false;
					}
				});
			}
			setSelected(parent, window.shouldSelect);
			if (!window.shouldSelect) {
				window.shouldSelectAll = false;
			}
		}
	});
	if (level == 'li.level_1') {
		setSelected($('cat_ALL'), window.shouldSelectAll);
	}
}

function toggleAllCategories(selected){
	
	$$('.level_1').each(function(element){
		setSelected(element, selected);
		toggleSubCategories(element, selected);
	});
}

function toggleSubCategories( element, selected){
	var ul = $(element.id+'subUl');
	if(ul==null)
		return;
	ul.childElements().each(function(li){
		
		setSelected(li, selected);
	});
	
}

function toggleBottomCategories(id, selected){
	$$('.category_tree .childto_'+id).each(function(element){
		
		setSelected(element, selected);
		
	});
}

function isSelected(element){
	var image = element.down('img.select_img',0); 
	return image.src.indexOf('selecton')>0;
}

function setSelected(element, selected){
	var image = element.down('img.select_img',0); 
	if(image==null)
		return;
	if(selected){
		image.src = image.src.replace('selectoff','selecton');	
		image.next('input').value= 'on';
	}else{
		image.src = image.src.replace('selecton','selectoff');
		image.next('input').value = 'off';
	}
}




/* 
	end select box click funtionality
*/

function tabswitch(active, number, tab_prefix, content_prefix, active_class)
{
	var num = number+1;
	for(var i =1; i < num; i++) {
		
		 $(content_prefix + i).style.display = 'none';
		 $(tab_prefix+i).className = tab_prefix+i;
	}
	$(content_prefix+active).style.display = '';  
	$(tab_prefix+active).className = active_class;
	
}

var newtWind = null;
function tattersallInfo(){
	if(newtWind && newtWind.closed){
		newtWind.close();
	}
	newtWind = window.open('/pagecontentpopup.do?pageid=273', 'info', 'scrollbars,resizable,height=400,width=600,top=0,left='+(screen.availWidth-620));
}

var newaWind = null;
function asianhelpwin() {
	if(newaWind && !newaWind.closed) {
			newaWind.close();
	}
	newaWind = window.open('/pagecontentpopup.do?pageid=20', 'info', 'scrollbars,resizable,height=400,width=600,top=0,left='+(screen.availWidth-620));
}

function hideinfotooltip(id){
	$('tooltip_'+id).hide();
}

function showinfotooltip(id){
	$('tooltip_'+id).show();
}

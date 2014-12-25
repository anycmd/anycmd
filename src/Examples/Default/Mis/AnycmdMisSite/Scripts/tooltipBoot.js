/// <reference path="libs/qtip2/jquery.qtip.js" />
// Create the tooltips only on document load
$(document).ready(function () {
	// Make sure to only match links to wikipedia with a rel tag
	$('a.fieldTooltip[rel]').each(function () {
		// We make use of the .each() loop to gain access to each element via the "this" keyword...
	    $(this).qtip(
		{
		    content: {
		        text: '<img class="throbber" src="/Content/img/throbber.gif" alt="Loading..." />',
		        ajax: {
		            url: $(this).attr('rel') // Use the rel attribute of each element for the url to load
		        },
		        title: {
		            text: '帮助 - ' + $(this).attr('title'),
		            button: '关闭'
		        }
		    },
		    position: {
		        at: 'bottom center', // Position the tooltip above the link
		        my: 'top center',
		        viewport: $(window), // Keep the tooltip on-screen at all times
		        effect: false // Disable positioning animation
		    },
		    show: {
		        event: 'click',
		        solo: true // Only show one tooltip at a time
		    },
		    hide: 'unfocus',
		    style: {
		        classes: 'qtip-fieldHelp qtip-shadow'
		    }
		});
	}).click(function (event) { event.preventDefault(); });
	$('a.pageTooltip[rel]').each(function () {
	    // We make use of the .each() loop to gain access to each element via the "this" keyword...
	    $(this).qtip(
		{
		    content: {
		        text: '<img class="throbber" src="/Content/img/throbber.gif" alt="Loading..." />',
		        ajax: {
		            url: $(this).attr('rel') // Use the rel attribute of each element for the url to load
		        },
		        title: {
		            text: '帮助 - ' + $(this).attr('title'),
		            button: '关闭'
		        }
		    },
		    position: {
		        at: 'bottom center', // Position the tooltip above the link
		        my: 'top center',
		        viewport: $(window), // Keep the tooltip on-screen at all times
		        effect: false // Disable positioning animation
		    },
		    show: {
		        event: 'click',
		        solo: true // Only show one tooltip at a time
		    },
		    hide: 'unfocus',
		    style: {
		        classes: 'qtip-pageHelp qtip-shadow'
		    }
		});
	}).click(function (event) { event.preventDefault(); });
});
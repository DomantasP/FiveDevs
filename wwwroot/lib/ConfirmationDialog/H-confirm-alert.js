(function($){
	$(function(){
		var $confirm = $("<div class='confirmContent'>"+
				"<div class='confirmContentInner'>"+
					"<div class='confirm' >"+
						"<h4 class='title' ><i class='fa fa-warning'></i>&nbsp;Patvirtinimo forma</h4><hr/>"+
						"<p ></p>"+
						"<div class='confirmButtonContent' >"+
							"<button class='Hbtn Hbtn-success confirmYES' ></button>"+
							"<button class='Hbtn Hbtn-danger confirmNO'></button>"+
						"</div>"+
					"</div>"+
				"</div>"+
			"</div>");
		$("body").append($confirm);
		$confirm.hide();
		$(".confirmContent").css("height",$(window).height()+"px");
		$(window).on("resize",function(){
			$(".confirmContent").css("height",$(window).height()+"px");
		})
	})
	
	$.confirm = (function myConfirm(){
		var defaults = {
			"hideYes":false,
			"yesText":"Taip",
			"hideNo":false,
			"noText":"Atšaukti",
			"yes":undefined,
			"no":undefined,
			"message":"confirm",
			"type":"default"
		}
		myConfirm.show = function(options){
			var settings = $.extend({},defaults,options);
			settings.hideNo == true?$(".confirmContent").find(".confirmNO").hide():$(".confirmContent").find(".confirmNO").show();
			settings.hideYes == true?$(".confirmContent").find(".confirmYES").hide():$(".confirmContent").find(".confirmYES").show();
			$(".confirmContent").find("button.confirmYES").html("<i class='fa fa-check-circle-o'></i>&nbsp;"+settings.yesText+"&nbsp;");
			$(".confirmContent").find("button.confirmNO").html("<i class='fa fa-times-circle-o'></i>&nbsp;"+settings.noText+"&nbsp;");
			$(".confirmContent").find("p").html(settings.message);
			switch (settings.type){
				case "success":
					$(".confirmContent").find("h4.title").css("color","#33CCCC")
					break;
				case "danger":
					$(".confirmContent").find("h4.title").css("color","#CC3333")
					break;
				case "warning":
					$(".confirmContent").find("h4.title").css("color","#FF9933")
					break;
				default:
					$(".confirmContent").find("h4.title").css("color","#0099FF")
					break;
			}
           

			$(".confirmContent").show();
			$(".confirmContent").animate({opacity:'1'},"fast","linear")
			
			$(".confirmContent").find(".confirmYES").unbind("click").on("click",function(){
				$(".confirmContent").animate({opacity:'0'},"fast","linear",function(){
                    $(".confirmContent").hide();  
                    options.passFunction(options.passParam);
					/*if(settings.yes){
						settings.yes();
					}*/
				})
			});
			$(".confirmContent").find(".confirmNO").unbind("click").on("click",function(){
				$(".confirmContent").animate({opacity:'0'},"fast","linear",function(){
					$(".confirmContent").hide();
					if(settings.no){
						settings.no();
					}
				})
			})
		}
		return myConfirm;
	})()
})(jQuery)

/*White Navigation*/
$(function(){
    
    function showHideNav(){
        if($(window).scrollTop()>=0){         
            $("nav").addClass("white-nav-top");
            $(".navbar-brand img").attr("src","Images/img/logo.png");   
            $("#back-to-top").fadeIn();
        }
        else{
            $("nav").removeClass("white-nav-top");
            $(".navbar-brand img").attr("src","Images/img/top-logo.png");   
            $("#back-to-top").fadeOut();
        }
    }
    showHideNav();
    
    $(window).scroll(function(){
        showHideNav();
    });
    
});


/*Mobile menu*/
$(function(){
    $("#mobile-nav-open-btn").click(function() {
        $("#mobile-nav").css("height","100%");
    });
    $("#mobile-nav-close-btn, #mobile-nav a").click(function() {
        $("#mobile-nav").css("height","0%");
    });
});


$(".toggle-password").click(function() {

  $(this).toggleClass("fa-eye fa-eye-slash");
  var input = $($(this).attr("toggle"));
  if (input.attr("type") == "password") {
    input.attr("type", "text");
  } else {
    input.attr("type", "password");
  }
});

$(".sold-table-list tr td").mousemove(function(e){
    $('.popup').css('top', (e.clientY + 10) + 'px').css('left', (e.clientX + 10) + 'px').show();
    $(".popup").show();
}).mouseout(function(){
    $(".popup").hide();
});






/*Mobile menu*/
$(function(){
    $("#mobile-nav-open-btn").click(function() {
        $("#mobile-nav").css("height","100%");
    });
    $("#mobile-nav-close-btn, #mobile-nav a").click(function() {
        $("#mobile-nav").css("height","0%");
    });
});

/*Smooth Scrolling*/
$(function () {
    $("a.smooth-scroll").click(function (event) {

        event.preventDefault();
        var section_id = $(this).attr("href");

        $("html,body").animate({
            scrollTop: $(section_id).offset().top - 64
        }, 1250, "easeInOutExpo");
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


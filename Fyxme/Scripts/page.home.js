// Author:      Ghislain Tremblay
// Date:        2016-02-01
// Description: JS functions required on home page

// Set the opacity style of the top menu
$(window).scroll(function () {

    var scrollTop = $(document).scrollTop();
    var opacity = scrollTop / 100;

    if (opacity > 0.8) { opacity = 0.8; }

    $("#top_bg_color").css("opacity", opacity);

    // Show Get Started button
    if (scrollTop > 700) { $(".get_started_box").show(); }
    else {
        $(".get_started_box").hide();
    }
});

$(document).ready(function () {

    var resWidth = $(window).width();

    // Show Fyxme phone
    if (resWidth < 460) { $("#mobPhone").hide(); }
    else { $("#mobPhone").show(); }


    $('.bxslider').bxSlider({
        minSlides: 1,
        maxSlides: 1,
        slideWidth: 700,
        autoHover: true,
        auto: true,
        pause: 10000,
    });
});

$(window).resize(function () {

    var resWidth = $(window).width();

    // Show Fyxme phone
    if (resWidth < 460) { $("#mobPhone").hide(); }
    else { $("#mobPhone").show(); }
});

$(function () {

    // Mobile menu open / close
    $("#btnMenuForMobile").click(function () {
        if ($("#mob_top_bg_color").css("display") == "none") {
            var fadeIn = { opacity: 1, transition: 'opacity 1s' };
            $("#mob_top_bg_color").css(fadeIn).slideDown("slow", function () {
                $("#mob_options").fadeIn("slow");
            });
        }
        else {
            $("#mob_options").fadeOut("slow", function () {
                var fadeOut = { opacity: 0, transition: 'opacity 1s' };
                $("#mob_top_bg_color").css(fadeOut).slideUp("slow");
            });
        }
    });

    // Main form open / close
    $("#btn_get_started").mouseover(function () {
        if ($("#form").css("display") == "none") {
            $("#get_started_form").css("border", "2px solid #0071bc");
        }
    });
    $("#btn_get_started").mouseout(function () {
        $("#get_started_form").css("border", "2px solid #139deb");
    });

    $("#btn_get_started").click(function () {

        $("#mob_options").fadeOut("slow", function () {
            var fadeOut = { opacity: 0, transition: 'opacity 1s' };
            $("#mob_top_bg_color").css(fadeOut).slideUp("slow");
        });

        if ($("#form").css("display") == "none") {
            $("#btn_get_started").attr("class", "btn-get-started-white");
            $("#get_started_form").css("border", "2px solid #139deb");
            $("#a1_get_started").attr("class", "a1_get_started_on");
            $("#form").slideDown("slow");
        } else {
            $("#btn_get_started").attr("class", "btn-get-started");
            $("#form").slideUp("slow");
        }

        $('html, body').animate({
            scrollTop: $($("#a1_get_started").attr('href')).offset().top - 90
        }, 1000);
    });

    // Add picture to upload in form
    var fadePic = { opacity: 1, transition: 'opacity 0.5s' };
    $("#btnAddPic1").click(function () { $("#upload_pic_2").css(fadePic).slideDown("slow"); });
    $("#btnAddPic2").click(function () { $("#upload_pic_3").css(fadePic).slideDown("slow"); });
    $("#btnAddPic3").click(function () { $("#upload_pic_4").css(fadePic).slideDown("slow"); });

    // Open / hide technician form
    $("#btn-tech").click(function () {
        if ($("#tech-form").css("display") == "none") {
            $("#tech-form").slideDown("slow");
        } else {
            $("#tech-form").slideUp("slow");
        }
    });

    // Anchors nav
    $("#a1_how_it_works").click(function () {
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1000);
        return false;
    });

    $("#a2_how_it_works").click(function () {
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1000);
        return false;
    });

    $("#a3_how_it_works").click(function () {
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1000);
        return false;
    });

    $("#a1_why_use_fyxme").click(function () {
        $("#mob_options").fadeOut("slow", function () {
            var fadeOut = { opacity: 0, transition: 'opacity 1s' };
            $("#mob_top_bg_color").css(fadeOut).slideUp("slow");
        });
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1000);
        return false;
    });

    $("#a1_technicians").click(function () {
        $("#mob_options").fadeOut("slow", function () {
            var fadeOut = { opacity: 0, transition: 'opacity 1s' };
            $("#mob_top_bg_color").css(fadeOut).slideUp("slow");
        });
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1000);
        return false;
    });

    // Anchors nav mobile
    $("#a1_how_it_works_mob").click(function () {
        $("#mob_options").fadeOut("slow", function () {
            var fadeOut = { opacity: 0, transition: 'opacity 1s' };
            $("#mob_top_bg_color").css(fadeOut).slideUp("slow");
        });
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1000);
        return false;
    });
    $("#a1_why_use_fyxme_mob").click(function () {
        $("#mob_options").fadeOut("slow", function () {
            var fadeOut = { opacity: 0, transition: 'opacity 1s' };
            $("#mob_top_bg_color").css(fadeOut).slideUp("slow");
        });
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1000);
        return false;
    });
    $("#a1_technicians_mob").click(function () {
        $("#mob_options").fadeOut("slow", function () {
            var fadeOut = { opacity: 0, transition: 'opacity 1s' };
            $("#mob_top_bg_color").css(fadeOut).slideUp("slow");
        });
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1000);
        return false;
    });
    
    // Calculate nb of characters left for damage description
    $('#txtDamageDesc_01').keyup(function () { TruncateText(500, 'txtDamageDesc_01', 'damageDescCharactersLeft_01'); });

});

function browseFile(input) {

    document.getElementById(input).click();
}

function TruncateText(nbCar, field, div) {

    var nbCharLeft = nbCar - ($('#' + field).val()).length;
    if (nbCharLeft >= 0) {
        $('#' + div).html("<span>" + nbCharLeft + "</span> characters left");
    }
    else {
        $('#' + div).html("<strong>0</strong> characters left");
    }
}